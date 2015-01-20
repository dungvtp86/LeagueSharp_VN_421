#region
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using Color = System.Drawing.Color;
#endregion

namespace JeonUtility
{
    class Program
    {
        #region variable declaration
        public static Menu baseMenu;
        public static Obj_AI_Hero Player = ObjectManager.Player;
        public static System.Drawing.Rectangle Monitor = System.Windows.Forms.Screen.PrimaryScreen.Bounds;

        public static SpellSlot[] SSpellSlots = { SpellSlot.Summoner1, SpellSlot.Summoner2 };
        public static SpellSlot[] SpellSlots = { SpellSlot.Q, SpellSlot.W, SpellSlot.E, SpellSlot.R };
        public static SpellSlot smiteSlot = SpellSlot.Unknown;
        public static SpellSlot igniteSlot = SpellSlot.Unknown;
        public static SpellSlot defslot = SpellSlot.Unknown;
        public static Spell smite;
        public static Spell ignite;
        public static Spell defspell;
        public static Spell jumpspell;




        public static bool canw2j = false;
        public static bool rdyw2j = false;
        public static bool rdyward = false;
        public static bool text_Isrender = false;
        public static bool textsmite_Isrender = false;

        public static int req_ignitelevel { get { return Jlib.getm_value("igniteLv"); } }

        public static float pastTime = 0;

        public static List<timer_clock> timerlist = new List<timer_clock>();

 
        public static String[] DefSpellstr = { "barrier", "heal" };

        public static Render.Text text_notifier = new Render.Text("Ulti no di , chet ba no roi :))!", Player, new Vector2(0, 50), (int)40, ColorBGRA.FromRgba(0xFF00FFBB));
        public static Render.Text text_help = new Render.Text("Moi nguoi can giup do!", Player, new Vector2(0, 50), (int)40, ColorBGRA.FromRgba(0xFF00FFBB));
        public static Render.Text text_smite = new Render.Text("Tu dong trung phat!", Player, new Vector2(55, 50), (int)40, ColorBGRA.FromRgba(0xFF0000FF));

        #region wardtracker
        public static List<Ward> wardlist = new List<Ward>();
        public static string[] wardnames = { "SightWard", "VisionWard", "Jack In The Box", "Cupcake Trap", "Noxious Trap" };
        public enum wardtype
        {
            Pink,
            Green,
            Mushroom,
            ShacoBox,
            Trap
        }

        public class Ward
        {
            public wardtype type;
            public int id;
            public float time;
            public Vector3 position;
            public bool show = true;
            public float endtiem;

            public Render.Text timer { get; set; }

            public Ward()
            {
                timer = new Render.Text("", new Vector2(0, 0), 32
                    , SharpDX.Color.White)
                {
                    VisibleCondition =
                    condition =>
                          (int)(endtiem - Game.Time) > 0 && show,

                    PositionUpdate = delegate
                    {
                        Vector2 vec2 = Drawing.WorldToScreen(new Vector3(position.X, position.Y + 25, position.Z));
                        return vec2;
                    },
                    TextUpdate = () => Convert.ToString((int)(endtiem - Game.Time)),
                    OutLined = true,
                    Centered = true
                };

                timer.Add();
                if (!show)
                    timer.Remove();
            }
        }
        #endregion wardtracker
        #endregion

        private static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += OnGameLoad;
        }

        private static void OnGameLoad(EventArgs args)
        {
            string version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            Game.PrintChat("<font color ='#33FFFF'>Hỗ trợ" +version+" </font>Loaded!");
            setSmiteSlot();
            setIgniteSlot();
            setDefSpellSlot();

            #region 
            #region  Main Menu
            baseMenu = new Menu("Hỗ Trợ HuyNK Series", "JeonUtility", true);
            baseMenu.AddToMainMenu();
            baseMenu.AddItem(new MenuItem("base_stat", "Di chuyển vị trí").SetValue(true));
            baseMenu.AddItem(new MenuItem("x", "HUD_X").SetValue(new Slider(600, 0, Monitor.Width)));
            baseMenu.AddItem(new MenuItem("y", "HUD_Y").SetValue(new Slider(250, 0, Monitor.Height)));

            var menu_smite = new Menu("Trừng Phạt", "Smite");
            var menu_ignite = new Menu("Thiêu Đốt", "Ignite");
            var menu_tracker = new Menu("Tracker", "Tracker");
            var menu_j2w = new Menu("Cắm mắt nhảy", "Jump2Ward");
            var menu_st = new Menu("Hỗ trợ E củ Twicth và Kalista", "Stacks");
            var menu_ins = new Menu("Dùng Item và Phép", "Item & Spell");
            var menu_noti = new Menu("Thông báo", "Notifier");
            var menu_jtimer = new Menu("Thời gian rừng", "JungleTimer");
            var menu_draw = new Menu("Hiển thị", "Draw");
            #endregion

            #region  menu for smite
            baseMenu.AddSubMenu(menu_smite);
            menu_smite.AddItem(new MenuItem("AutoSmite", "Tự động trừng phạt").SetValue(true));
            menu_smite.AddItem(new MenuItem("smite_enablekey", "Luôn dùng:").SetValue(new KeyBind('K', KeyBindType.Toggle)));// 32 - Space
            menu_smite.AddItem(new MenuItem("smite_holdkey", "Giữ phím:").SetValue(new KeyBind(32, KeyBindType.Press)));// 32 - Space
            #endregion

            #region  menu for ignite
            baseMenu.AddSubMenu(menu_ignite);
            menu_ignite.AddItem(new MenuItem("AutoIgnite", "Tự thiêu đôt").SetValue(true));
            menu_ignite.AddItem(new MenuItem("igniteLv", "Dùng cho lv Level :").SetValue(new Slider(1, 1, 18)));
            #endregion

            #region  menu for tracker
            baseMenu.AddSubMenu(menu_tracker);
            menu_tracker.AddItem(new MenuItem("tracker_enemyspells", "Phép của địch").SetValue(true));
            menu_tracker.AddItem(new MenuItem("tracker_ward", "Mắt").SetValue(true));

            #endregion

            #region  menu for Jump2Ward
            baseMenu.AddSubMenu(menu_j2w);
            menu_j2w.AddItem(new MenuItem("j2w_bool", "Cắm mắt nhảy").SetValue(true));
            menu_j2w.AddItem(new MenuItem("j2w_hkey", "Phím : ").SetValue(new KeyBind('T', KeyBindType.Press)));
            menu_j2w.AddItem(new MenuItem("j2w_info", "Thông tin").SetValue(false));
            #endregion

            #region  menu for stacks
            baseMenu.AddSubMenu(menu_st);
            menu_st.AddItem(new MenuItem("st_bool", "Hiện Damge").SetValue(true));
            menu_st.AddItem(new MenuItem("st_twitch", "Tự động Twitch(E)").SetValue(true));
            menu_st.AddItem(new MenuItem("st_kalista", "Tự động Kalista(E)").SetValue(true));
            #endregion

            #region  menu for UseItem&Spell
            baseMenu.AddSubMenu(menu_ins);


            var menu_Potion = new Menu("Phần trăm HP/Mana", "Potion");
            menu_ins.AddSubMenu(menu_Potion);
            menu_Potion.AddItem(new MenuItem("useitem_flask", "Bình").SetValue(true));
            menu_Potion.AddItem(new MenuItem("useitem_p_fla", "Dùng khi máu(%)").SetValue(new Slider(50, 0, 100)));
            menu_Potion.AddItem(new MenuItem("useitem_hppotion", "Phần trăm máu").SetValue(true));
            menu_Potion.AddItem(new MenuItem("useitem_p_hp", "Dùng khi máu(%)").SetValue(new Slider(50, 0, 100)));
            menu_Potion.AddItem(new MenuItem("useitem_manapotion", "Phần trăm mana").SetValue(true));
            menu_Potion.AddItem(new MenuItem("useitem_p_mana", "Dùng khi Mana(%)").SetValue(new Slider(50, 0, 100)));

            var menu_items = new Menu("Hỗ trợ Item", "Items");
            menu_ins.AddSubMenu(menu_items);
            var item_zhonya = new Menu("Đồng hồ cát", "Zhonya");
            menu_items.AddSubMenu(item_zhonya);
            item_zhonya.AddItem(new MenuItem("useitem_zhonya", "Dùng đồng hồ cát").SetValue(true));
            item_zhonya.AddItem(new MenuItem("useitem_p_zhonya", "Dùng khi máu(%)").SetValue(new Slider(15, 0, 100)));

            var item_botrk = new Menu("Gươm vô danh", "BOTRK");
            menu_items.AddSubMenu(item_botrk);
            item_botrk.AddItem(new MenuItem("useitem_botrk", "Dùng gươm vô danh").SetValue(true));
            item_botrk.AddItem(new MenuItem("useitem_p_botrk", "dùng khi máu(%)").SetValue(new Slider(20, 0, 100)));
            item_botrk.AddItem(new MenuItem("useitem_botrk_atg", "Chống bị dồn damge").SetValue(true));
            item_botrk.AddItem(new MenuItem("useitem_botrk_atg_p", "Dùng khi tầm đánh :").SetValue(new Slider(150, 100, 450)));

            var item_mikaels = new Menu("Chuông bảo hộ Mikaels", "Mikaels");
            menu_items.AddSubMenu(item_mikaels);
            item_mikaels.AddItem(new MenuItem("useitem_mikaels", "Bật?").SetValue(true));
            item_mikaels.AddItem(new MenuItem("useitem_p_mikaels", "Dùm khi máu(%)").SetValue(new Slider(15, 0, 100)));
            item_mikaels.AddItem(new MenuItem("useitem_p_mikaels_delay", "Độ trể dùng(ms)").SetValue(new Slider(100, 0, 1000)));

            #region mikaels_cc
            var menu_mikaels_cc = new Menu("mikael_cc", "Sử dụng khi bị CC");
            item_mikaels.AddSubMenu(menu_mikaels_cc);
            menu_mikaels_cc.AddItem(new MenuItem("mikaels_cc_bool", "Bật CC?").SetValue(true));
            menu_mikaels_cc.AddItem(new MenuItem("mikaels_cc_stun", "Choáng").SetValue(true));
            menu_mikaels_cc.AddItem(new MenuItem("mikaels_cc_fear", "Sợ hãi").SetValue(true));
            menu_mikaels_cc.AddItem(new MenuItem("mikaels_cc_charm", "Hôn gió (Ahri").SetValue(true));
            menu_mikaels_cc.AddItem(new MenuItem("mikaels_cc_taunt", "Khiêu khích").SetValue(true));
            menu_mikaels_cc.AddItem(new MenuItem("mikaels_cc_snare", "Trói").SetValue(true));
            menu_mikaels_cc.AddItem(new MenuItem("mikaels_cc_silence", "Câm lặng").SetValue(false));
            menu_mikaels_cc.AddItem(new MenuItem("mikaels_cc_polymorph", "Biến hình (Lulu)").SetValue(true));
            #endregion


            var item_qs = new Menu("Khăn giải thuật", "QuickSilver");
            menu_items.AddSubMenu(item_qs);
            item_qs.AddItem(new MenuItem("useitem_qs_bool", "Dùng khăn giải thuật").SetValue(true));
            item_qs.AddItem(new MenuItem("useitem_p_qs_delay", "Độ trể dùng(ms)").SetValue(new Slider(100, 0, 1000)));
            #region qs_cc
            var menu_quicksilver_cc = new Menu("Dùng khi bị  CC", "Use On CC");
            item_qs.AddSubMenu(menu_quicksilver_cc);
            menu_quicksilver_cc.AddItem(new MenuItem("qs_cc_stun", "Choáng").SetValue(true));
            menu_quicksilver_cc.AddItem(new MenuItem("qs_cc_fear", "Sợ hãi").SetValue(true));
            menu_quicksilver_cc.AddItem(new MenuItem("qs_cc_charm", "Hôn gió").SetValue(true));
            menu_quicksilver_cc.AddItem(new MenuItem("qs_cc_taunt", "Khiêu khích").SetValue(true));
            menu_quicksilver_cc.AddItem(new MenuItem("qs_cc_snare", "Trói").SetValue(true));
            menu_quicksilver_cc.AddItem(new MenuItem("qs_cc_silence", "Câm lặng").SetValue(false));
            menu_quicksilver_cc.AddItem(new MenuItem("qs_cc_polymorph", "Biến hình(lulu)").SetValue(true));
            menu_quicksilver_cc.AddItem(new MenuItem("qs_cc_suppression", "Áp chế").SetValue(true));
            menu_quicksilver_cc.AddItem(new MenuItem("qs_cc_zedutl", "Zed ulti").SetValue(true));
            #endregion

            var menu_spell = new Menu("Dùng phép", "Spell");
            menu_ins.AddSubMenu(menu_spell);
            menu_spell.AddItem(new MenuItem("usespell", "Bật ?").SetValue(true));
            menu_spell.AddItem(new MenuItem("usespell_hp", "Dùng khi máu(%)").SetValue(new Slider(10, 0, 100)));

            #endregion

            #region  menu for notifier
            baseMenu.AddSubMenu(menu_noti);
            menu_noti.AddItem(new MenuItem("noti_karthus", "Karthus Ultimate").SetValue(true));
            menu_noti.AddItem(new MenuItem("noti_ez", "Ez,Jinx,Ashe Ultimate").SetValue(true));
            menu_noti.AddItem(new MenuItem("noti_cait", "CaitUltimate").SetValue(true));
            menu_noti.AddItem(new MenuItem("noti_shen", "ShenUltimate").SetValue(true));
            menu_noti.AddItem(new MenuItem("noti_shenhp", "Cảnh báo khi máu nạn nhân còn(%)").SetValue(new Slider(10, 0, 100)));
            #endregion

            #region  menu for JungleTimer
            baseMenu.AddSubMenu(menu_jtimer);
            menu_jtimer.AddItem(new MenuItem("jt_active", "Kích hoạt").SetValue(true));
            menu_jtimer.AddItem(new MenuItem("jt_active_minimap", "Hiển thị trong bản đồ nhỏ").SetValue(true));
            menu_jtimer.AddItem(new MenuItem("jt_font_size", "Kích cỡ (cần load lại bằng F9 hoặc F8)").SetValue(new Slider(32, 8, 72)));
            menu_jtimer.AddItem(new MenuItem("jt_font_size_minimap", "Kích cỡ chữ").SetValue(new Slider(10, 8, 32)));

            #endregion

            #region menu for Draw
            baseMenu.AddSubMenu(menu_draw);
            menu_draw.AddItem(new MenuItem("draw_turret", "Hiện tầm đánh Trụ").SetValue(true));

            #endregion

            #endregion

            #region 타이머
            timer_clock Baron = new timer_clock { Position = new Vector3(4910f, 10268f, -71.24f),name = "SRU_BaronSpawn",
                respawntime = 420};
            timer_clock Dragon = new timer_clock { Position = new Vector3(9836f, 4408f, -71.24f), name = "SRU_Dragon" ,
                respawntime = 360};
            timer_clock top_crab = new timer_clock { Position = new Vector3(4266f, 9634f, -67.87f), name = "Sru_Crab",
                respawntime = 180,Range=3000};
            timer_clock down_crab = new timer_clock { Position = new Vector3(10524f, 5116f, -62.81f), name = "Sru_Crab",
                respawntime = 180,Range=3000};

            timer_clock bteam_Razorbeak = new timer_clock { Position = new Vector3(6974f, 5460f, 54f), name = "SRU_Razorbeak" };
            timer_clock bteam_Red = new timer_clock { Position = new Vector3(7796f, 4028f, 54f), name = "SRU_Red",
                respawntime = 300};
            timer_clock bteam_Krug = new timer_clock { Position = new Vector3(8394f, 2750f, 50f), name = "SRU_Krug" };
            timer_clock bteam_Blue = new timer_clock { Position = new Vector3(3832f, 7996f, 52f), name = "SRU_Blue",
                respawntime = 300 };
            timer_clock bteam_Gromp = new timer_clock { Position = new Vector3(2112f, 8372f, 51.7f), name = "SRU_Gromp" };
            timer_clock bteam_Wolf = new timer_clock { Position = new Vector3(3844f, 6474f, 52.46f), name = "SRU_Murkwolf" };

            timer_clock pteam_Razorbeak = new timer_clock { Position = new Vector3(7856f, 9492f, 52.33f), name = "SRU_Razorbeak" };
            timer_clock pteam_Red = new timer_clock { Position = new Vector3(7124f, 10856f, 56.34f), name = "SRU_Red",
                respawntime = 300 };
            timer_clock pteam_Krug = new timer_clock { Position = new Vector3(6495f, 12227f, 56.47f), name = "SRU_Krug" };
            timer_clock pteam_Blue = new timer_clock { Position = new Vector3(10850f, 6938f, 51.72f), name = "SRU_Blue",
                respawntime = 300 };
            timer_clock pteam_Gromp = new timer_clock { Position = new Vector3(12766f, 6464f, 51.66f), name = "SRU_Gromp" };
            timer_clock pteam_Wolf = new timer_clock { Position = new Vector3(10958f, 8286f, 62.46f), name = "SRU_Murkwolf" };


            #endregion

            Game.OnGameUpdate += OnGameUpdate;
            GameObject.OnCreate += OnCreate;
            GameObject.OnDelete += OnDelete;
            Drawing.OnEndScene += OnDraw_EndScene;
            //Drawing.OnDraw += OnDraw;
        }

        private static void OnDraw_EndScene(EventArgs args)
        {
            if (Jlib.getm_bool("draw_turret"))
            {
                foreach (var t in ObjectManager.Get<Obj_AI_Turret>().Where(t => !t.IsDead && t.IsEnemy))
                {
                    Drawing.DrawCircle(t.Position, 775, System.Drawing.Color.White);
                }
            }

            #region wardtracker
            foreach (var ward in wardlist)
            {
                var ratio = (int)(ward.endtiem - Game.Time) / ward.time;

                var bar_start = new Vector2(Drawing.WorldToScreen(ward.position).X - 25, Drawing.WorldToScreen(ward.position).Y);
                var bar_end = new Vector2(bar_start.X + (50 * ratio), bar_start.Y);

                var bar_out_start = new Vector2(bar_start.X - 1, bar_end.Y - 1);
                var bar_out_end = new Vector2(bar_start.X + 52, bar_start.Y - 1);

                if (ward.endtiem <= Game.Time)
                    wardlist.Remove(ward);

                Color color = Color.Pink;

                switch (ward.type)
                {
                    case wardtype.Green:
                        color = Color.Green;
                        break;
                    case wardtype.Pink:
                        color = Color.Blue;
                        break;
                    case wardtype.ShacoBox:
                        color = Color.Red;
                        break;
                    case wardtype.Mushroom:
                        color = Color.Purple;
                        break;
                    case wardtype.Trap:
                        color = Color.Yellow;
                        break;
                }
                Utility.DrawCircle(ward.position, 60, color, 5, 5);
                Utility.DrawCircle(ward.position, 60, color, 5, 5, true);
                if (ward.type != wardtype.Pink)
                {
                    Drawing.DrawLine(bar_out_start, bar_out_end, 10, Color.Chartreuse);
                    Drawing.DrawLine(bar_start, bar_end, 8, Color.Chartreuse);
                }
            }
            #endregion wardtracker
        }
        private static void OnGameUpdate(EventArgs args)
        {

            #region get info
            float Player_baseAD = Player.BaseAttackDamage;
            float Player_addAD = Player.FlatPhysicalDamageMod;
            float Player_totalAD = Player_baseAD + Player_addAD;
            float Player_totalAP = Player.FlatMagicDamageMod;
            #endregion

                #region AutoIgnite
                if (Jlib.getm_bool("AutoIgnite") && igniteSlot != SpellSlot.Unknown &&
                    Player.Level >= req_ignitelevel)
                {
                    float ignitedamage;
                    bool IgniteReady = false;
                    ignitedamage = setigniteDamage();
                    foreach (var hero in ObjectManager.Get<Obj_AI_Hero>()
                        .Where(hero => hero != null && hero.IsValid && !hero.IsDead && Player.ServerPosition.Distance(hero.ServerPosition) < ignite.Range
                        && !hero.IsMe && !hero.IsAlly && (hero.Health + hero.HPRegenRate * 2) <= ignitedamage))
                    {

                        if (Player.Spellbook.CanUseSpell(igniteSlot) == SpellState.Ready)
                        {
                            IgniteReady = true;
                        }
                        if (IgniteReady)
                        {
                            setIgniteSlot();
                            Player.Spellbook.CastSpell(igniteSlot, hero);
                        }
                    }
                }
                #endregion

                #region Spelltracker
                if (Jlib.getm_bool("tracker_enemyspells"))
                {
                    foreach (var target in
                        ObjectManager.Get<Obj_AI_Hero>().Where(hero => hero != null && hero.IsValid && (!hero.IsMe && hero.IsHPBarRendered)))
                    {

                        int X = 10;
                        int Y = 40;
                        foreach (var sSlot in SSpellSlots)
                        {
                            var spell = target.Spellbook.GetSpell(sSlot);
                            var t = spell.CooldownExpires - Game.Time;
                            if (t < 0)
                            {

                                Drawing.DrawText(target.HPBarPosition.X + X + 85, target.HPBarPosition.Y + Y, System.Drawing.Color.FromArgb(255, 0, 255, 0), filterspellname(spell.Name));
                            }
                            else
                            {
                                Drawing.DrawText(target.HPBarPosition.X + X + 85, target.HPBarPosition.Y + Y, System.Drawing.Color.Red, filterspellname(spell.Name));
                            }

                            Y += 15;
                        }
                        Y = 40;
                        foreach (var slot in SpellSlots)
                        {
                            var spell = target.Spellbook.GetSpell(slot);
                            var t = spell.CooldownExpires - Game.Time;
                            if (t < 0)
                            {
                                Drawing.DrawText(target.HPBarPosition.X + X, target.HPBarPosition.Y + Y, System.Drawing.Color.FromArgb(255, 0, 255, 0), Convert.ToString(spell.Level));
                            }
                            else
                            {
                                Drawing.DrawText(target.HPBarPosition.X + X, target.HPBarPosition.Y + Y, System.Drawing.Color.Red, Convert.ToString(spell.Level));
                            }
                            X += 20;
                        }
                    }
                }

                #endregion


                #region ward tracker
                foreach (var ward in ObjectManager.Get<Obj_AI_Base>().Where(t => wardnames.Contains(t.Name) && t.IsEnemy))
                {
                    if (!wardlist.Any(w => w.id == ward.NetworkId) && ward.Mana > 0 && ward.MaxHealth == 3)
                    {
                        wardlist.Add(new Ward
                        {
                            position = ward.Position,
                            type = wardtype.Green,
                            id = ward.NetworkId,
                            time = ward.MaxMana,
                            endtiem = Game.Time + ward.Mana
                        });
                    }
                    if (!wardlist.Any(w => w.id == ward.NetworkId) && ward.MaxHealth == 5)
                    {
                        wardlist.Add(new Ward
                        {
                            position = ward.Position,
                            type = wardtype.Pink,
                            id = ward.NetworkId,
                            time = ward.MaxMana,
                            endtiem = float.MaxValue
                        });
                    }
                    if (!wardlist.Any(w => w.id == ward.NetworkId) && ward.Name == "Jack In The Box")
                    {
                        wardlist.Add(new Ward
                        {
                            position = ward.Position,
                            type = wardtype.ShacoBox,
                            id = ward.NetworkId,
                            time = ward.MaxMana,
                            endtiem = Game.Time + ward.Mana
                        });
                    }
                    if (!wardlist.Any(w => w.id == ward.NetworkId) && ward.Name == "Cupcake Trap")
                    {
                        wardlist.Add(new Ward
                        {
                            position = ward.Position,
                            type = wardtype.Trap,
                            id = ward.NetworkId,
                            time = 4 * 60,
                            endtiem = Game.Time + 240
                        });
                    }
                    if (!wardlist.Any(w => w.id == ward.NetworkId) && ward.Name == "Noxious Trap")
                    {
                        wardlist.Add(new Ward
                        {
                            position = ward.Position,
                            type = wardtype.Mushroom,
                            id = ward.NetworkId,
                            time = ward.MaxMana,
                            endtiem = Game.Time + ward.Mana
                        });
                    }
                }
            #endregion wardtracker

                #region Jump2Ward (Jax,Kata,LeeSin)
                if (Jlib.getm_bool("j2w_bool"))
                {
                    List<String> champs = new List<String>();
                    champs.Add("LeeSin"); champs.Add("Katarina"); champs.Add("Jax");
                    setj2wslots(champs);
                    if (canw2j)
                    {
                        checkE();
                        checkWard();
                        if (rdyw2j && baseMenu.Item("j2w_hkey").GetValue<KeyBind>().Active)
                        {
                            Vector3 cursor = Game.CursorPos;
                            Vector3 myPos = Player.ServerPosition;
                            Player.IssueOrder(GameObjectOrder.MoveTo, cursor);
                            foreach (var target in ObjectManager.Get<Obj_AI_Base>().Where(ward => ward.IsVisible && ward.IsAlly && !ward.IsMe &&
                                Vector3.DistanceSquared(cursor, ward.ServerPosition) <= 200 * 200 &&
                                Vector3.Distance(ward.Position, Player.Position) <= 700 && ward.Name.IndexOf("Turret") == -1))
                            {
                                jumpspell.CastOnUnit(target);
                            }

                            if (rdyward)
                            {
                                Player.Spellbook.CastSpell(Items.GetWardSlot().SpellSlot, cursor);
                            }
                        }
                    }


                    if (Jlib.getm_bool("j2w_info"))
                    {
                        Game.PrintChat("Tuong : " + Player.ChampionName);
                        Game.PrintChat("Co the^? : " + canw2j);
                        Game.PrintChat("Phe'p : " + jumpspell.Slot.ToString());
                        Game.PrintChat("WardStack : " + Items.GetWardSlot().Stacks);
                        baseMenu.Item("j2w_info").SetValue<bool>(false);
                    }

                }

                #endregion

                #region  Stacks
                if (Jlib.getm_bool("st_twitch") && Player.ChampionName == "Twitch")
                {
                    Spell E = new Spell(SpellSlot.E, 1200);
                    var target = TargetSelector.GetTarget(E.Range, TargetSelector.DamageType.Physical);
                    if (target.IsValidTarget(E.Range))
                    {
                        foreach (var venoms in target.Buffs.Where(venoms => venoms.DisplayName == "TwitchDeadlyVenom"))
                        {
                            var damage = getTwitEDmg(target, venoms.Count, Player_addAD, Player_totalAP, E.Level);
                            //Game.PrintChat("d:{0} hp:{1}",damage,target.Health);
                            if (damage >= target.Health && E.IsReady())
                                E.Cast();

                            if (Jlib.getm_bool("st_bool"))
                            {
                                String t_damage = Convert.ToInt64(damage).ToString() + "(" + venoms.Count + ")";
                                Drawing.DrawText(target.HPBarPosition.X, target.HPBarPosition.Y - 5, Color.Red, t_damage);
                            }
                        }
                    }
                }

                if (Jlib.getm_bool("st_kalista") && Player.ChampionName == "Kalista")
                {
                    Spell E = new Spell(SpellSlot.E, 900);
                    if (E.IsReady())
                    {
                        var target = TargetSelector.GetTarget(E.Range, TargetSelector.DamageType.Physical);
                        if (target.IsValidTarget(E.Range))
                        {
                            foreach (var venoms in target.Buffs.Where(venoms => venoms.DisplayName == "KalistaExpungeMarker"))
                            {
                                var damage = getKaliDmg(target, venoms.Count, Player_totalAD, E.Level);
                                if (damage >= target.Health)
                                    E.Cast();
                                if (Jlib.getm_bool("st_bool"))
                                {
                                    String t_damage = Convert.ToInt64(damage).ToString() + "(" + venoms.Count + ")";
                                    Drawing.DrawText(target.HPBarPosition.X, target.HPBarPosition.Y - 5, Color.Red, t_damage);
                                }
                            }
                        }

                        Obj_AI_Base mob = GetNearest(Player.ServerPosition);
                        foreach (var venoms in mob.Buffs.Where(venoms => venoms.DisplayName == "KalistaExpungeMarker"))
                        {
                            var damage = getKaliDmg(mob, venoms.Count, Player_totalAD, E.Level);
                            if (damage >= mob.Health && Vector3.Distance(mob.Position, Player.Position) <= 900
                                && (mob.Name.Contains("SRU_Dragon") || mob.Name.Contains("SRU_Baron")))
                                E.Cast();

                            if (Jlib.getm_bool("st_bool"))
                            {
                                String t_damage = Convert.ToInt64(damage).ToString() + "(" + venoms.Count + ")";
                                Drawing.DrawText(mob.HPBarPosition.X, mob.HPBarPosition.Y - 5, Color.Red, t_damage);
                            }
                        }
                    }
                }
                #endregion

                #region Items&spells
                //item

                int tempItemid = 3157;
                if (Jlib.getm_bool("useitem_zhonya") && Items.HasItem(tempItemid) && Items.CanUseItem(tempItemid))
                {
                    foreach (var p_item in Player.InventoryItems.Where(item => item.Id == ItemId.Zhonyas_Hourglass))
                    {
                        if (Player.HealthPercentage() <= (float)Jlib.getm_value("useitem_p_zhonya")
                            && !Player.Buffs.Any(buff => buff.DisplayName == "Chrono Shift"))
                            Player.Spellbook.CastSpell(p_item.SpellSlot);
                    }

                }


                tempItemid = Convert.ToInt32(ItemId.Blade_of_the_Ruined_King);
                if (Jlib.getm_bool("useitem_botrk") && Items.HasItem(tempItemid) && Items.CanUseItem(tempItemid)
                                                            && Player.HealthPercentage() <= (float)Jlib.getm_value("useitem_p_botrk"))
                {
                    Obj_AI_Hero target = null;
                    Double max_healpoint = 0;
                    foreach (var p_item in Player.InventoryItems.Where(item => item.Id == ItemId.Blade_of_the_Ruined_King))
                    {
                        if (ObjectManager.Get<Obj_AI_Hero>().Any(h => h.IsEnemy && !h.IsDead && h.IsVisible &&
                            Vector3.Distance(h.Position, Player.Position) <= Jlib.getm_value("useitem_botrk_atg_p")) && Jlib.getm_bool("useitem_botrk_atg"))
                            Player.Spellbook.CastSpell(p_item.SpellSlot, target);

                        foreach (var hero in ObjectManager.Get<Obj_AI_Hero>().Where(h => h.IsEnemy && h.IsValid && h.IsVisible && Vector3.Distance(h.Position, Player.Position) <= 450))
                        {
                            var healpoint = Player.CalcDamage(hero, Damage.DamageType.Physical, hero.MaxHealth * 0.1);
                            if (max_healpoint < healpoint)
                            {
                                max_healpoint = healpoint;
                                target = hero;
                            }
                        }
                        Player.Spellbook.CastSpell(p_item.SpellSlot, target);
                    }
                }

                tempItemid = Convert.ToInt32(ItemId.Mikaels_Crucible);
                if (Jlib.getm_bool("useitem_mikaels") && Items.HasItem(tempItemid) && Items.CanUseItem(tempItemid) && !Utility.InShop(Player))
                {
                    List<BuffType> bufflist = new List<BuffType>();
                    getbufflist(bufflist, ItemId.Mikaels_Crucible);
                    foreach (var p_item in Player.InventoryItems.Where(item => item.Id == ItemId.Mikaels_Crucible))
                    {
                        foreach (var hero in ObjectManager.Get<Obj_AI_Hero>().Where(h => h.IsAlly && !h.IsMe && h.IsValid && h.IsVisible && Vector3.Distance(h.Position, Player.Position) <= 800))
                        {
                            if (hero.HealthPercentage() <= (float)Jlib.getm_value("useitem_p_mikaels"))
                                Player.Spellbook.CastSpell(p_item.SpellSlot, hero);

                            if (Jlib.getm_bool("mikaels_cc_bool"))
                            {
                                foreach (var buff in hero.Buffs)
                                {
                                    if (bufflist.Any(b => b == buff.Type))
                                        Utility.DelayAction.Add(Jlib.getm_value("useitem_p_mikaels_delay"), () => { Player.Spellbook.CastSpell(p_item.SpellSlot, hero); });
                                }
                            }
                        }
                    }
                }
                tempItemid = Convert.ToInt32(ItemId.Quicksilver_Sash);
                int tempItemid2 = Convert.ToInt32(ItemId.Mercurial_Scimitar);
                if (Jlib.getm_bool("useitem_qs_bool") && !Utility.InShop(Player))
                {
                    if ((Items.HasItem(tempItemid) && Items.CanUseItem(tempItemid)) || ((Items.HasItem(tempItemid2) && Items.CanUseItem(tempItemid2))))
                    {
                        List<BuffType> bufflist = new List<BuffType>();
                        getbufflist(bufflist, ItemId.Quicksilver_Sash);
                        foreach (var p_item in Player.InventoryItems.Where(item => (item.Id == ItemId.Quicksilver_Sash || item.Id == ItemId.Mercurial_Scimitar)))
                        {
                            foreach (var buff in Player.Buffs)
                            {
                                Utility.DelayAction.Add(Jlib.getm_value("useitem_p_qs_delay"), () =>
                                {
                                    if (bufflist.Any(b => b == buff.Type))
                                        Player.Spellbook.CastSpell(p_item.SpellSlot);
                                    if (buff.DisplayName == "ZedUltExecute")
                                        Player.Spellbook.CastSpell(p_item.SpellSlot);
                                    if (buff.DisplayName == "FizzChurnTheWatersCling")
                                        Player.Spellbook.CastSpell(p_item.SpellSlot);
                                });
                            }
                        }
                    }
                }


                //potions
                tempItemid = Convert.ToInt32(ItemId.Crystalline_Flask);
                if (Jlib.getm_bool("useitem_flask") && Items.HasItem(tempItemid) && Items.CanUseItem(tempItemid))
                {
                    foreach (var p_item in Player.InventoryItems.Where(item => item.Id == ItemId.Crystalline_Flask && !Player.HasBuff("ItemCrystalFlask")))
                    {
                        if (Player.HealthPercentage() <= (float)Jlib.getm_value("useitem_p_fla") && !Utility.InShop(Player))
                            Player.Spellbook.CastSpell(p_item.SpellSlot);
                    }
                }

                tempItemid = Convert.ToInt32(ItemId.Health_Potion);
                if (Jlib.getm_bool("useitem_hppotion"))
                {
                    ItemId item = ItemId.Health_Potion;
                    if (Player.InventoryItems.Any(t => (t.Id == ItemId.Health_Potion || Convert.ToInt32(t.Id) == 2010)))
                    {
                        if (Player.InventoryItems.First(t => (t.Id == ItemId.Health_Potion || Convert.ToInt32(t.Id) == 2010)).Id != ItemId.Health_Potion)
                            item = ItemId.Unknown;

                        if (Player.HealthPercentage() <= (float)Jlib.getm_value("useitem_p_hp") && Player.InventoryItems.Any(t => (t.Id == ItemId.Health_Potion || Convert.ToInt32(t.Id) == 2010)))
                        {
                            if (!Player.HasBuff("ItemMiniRegenPotion") && item == ItemId.Unknown)
                                Player.Spellbook.CastSpell(Player.InventoryItems.First(t => Convert.ToInt32(t.Id) == 2010).SpellSlot);
                            if (!Player.HasBuff("Health Potion") && item == ItemId.Health_Potion)
                                Player.Spellbook.CastSpell(Player.InventoryItems.First(t => t.Id == ItemId.Health_Potion).SpellSlot);

                        }
                    }
                }



                tempItemid = Convert.ToInt32(ItemId.Mana_Potion);
                if (Jlib.getm_bool("useitem_manapotion") && Items.HasItem(tempItemid) && Items.CanUseItem(tempItemid))
                {
                    foreach (var p_item in Player.InventoryItems.Where(item => item.Id == ItemId.Mana_Potion && !Player.HasBuff("Mana Potion") && !Player.HasBuff("ItemCrystalFlask")))
                    {
                        if (Player.ManaPercentage() <= (float)Jlib.getm_value("useitem_p_mana") && !Utility.InShop(Player))
                        {
                            Player.Spellbook.CastSpell(p_item.SpellSlot);
                        }
                    }
                }
                //spell
                if (Jlib.getm_bool("usespell") && defslot != SpellSlot.Unknown)
                {
                    if (Player.HealthPercentage() <= (float)Jlib.getm_value("usespell_hp") && !Utility.InShop(Player))
                    {
                        if (Player.Spellbook.CanUseSpell(defslot) == SpellState.Ready)
                            Player.Spellbook.CastSpell(defslot);
                    }
                }

                #endregion

                #region ultnotifier
                //Karthus
                if (Player.ChampionName == "Karthus")
                {
                    if (Jlib.getm_bool("noti_karthus") && Player.Spellbook.CanUseSpell(SpellSlot.R) == SpellState.Ready)
                    {
                        Spell R = new Spell(SpellSlot.R, 100000);
                        var target = TargetSelector.GetTarget(R.Range, TargetSelector.DamageType.Magical);
                        var damage = R.GetDamage(target);


                        if (target.IsValidTarget(R.Range) && target.IsVisible && damage >= target.Health + target.HPRegenRate * 3)
                        {
                            if (!text_Isrender)
                                text_notifier.Add();
                            text_Isrender = true;
                        }
                    }
                    else
                    {
                        text_notifier.Remove();
                        text_Isrender = false;
                    }
                }
                //cait
                if (Player.ChampionName == "Caitlyn")
                {
                    if (Jlib.getm_bool("noti_cait") && Player.Spellbook.CanUseSpell(SpellSlot.R) == SpellState.Ready)
                    {
                        Spell R = new Spell(SpellSlot.R, 1500 + (500 * Player.Spellbook.GetSpell(SpellSlot.R).Level));
                        var target = TargetSelector.GetTarget(R.Range, TargetSelector.DamageType.Physical);
                        var damage = R.GetDamage(target);


                        if (target.IsValidTarget(R.Range) && target.IsVisible && damage >= target.Health + target.HPRegenRate)
                        {
                            if (!text_Isrender)
                                text_notifier.Add();
                            text_Isrender = true;
                            Jlib.targetPing(target.Position.To2D());


                        }
                    }
                    else
                    {
                        text_notifier.Remove();
                        text_Isrender = false;
                    }
                }
                //ez
                if (Player.ChampionName == "Ezreal" || Player.ChampionName == "Jinx" || Player.ChampionName == "Ashe")
                {
                    if (Jlib.getm_bool("noti_ez") && Player.Spellbook.CanUseSpell(SpellSlot.R) == SpellState.Ready)
                    {
                        Spell R = new Spell(SpellSlot.R, 100000);
                        var target = TargetSelector.GetTarget(R.Range, TargetSelector.DamageType.Magical);
                        var damage = R.GetDamage(target);


                        if (target.IsValidTarget(R.Range) && target.IsVisible && damage >= target.Health + target.HPRegenRate * (2000f / Vector3.Distance(Player.ServerPosition, target.ServerPosition))) // time=speed/distance
                        {
                            if (!text_Isrender)
                                text_notifier.Add();
                            text_Isrender = true;
                            Jlib.targetPing(target.Position.To2D());
                        }
                    }
                    else
                    {
                        text_notifier.Remove();
                        text_Isrender = false;
                    }
                }
                //shen
                if (Player.ChampionName == "Shen")
                {
                    if (Jlib.getm_bool("noti_shen") && Player.Spellbook.CanUseSpell(SpellSlot.R) == SpellState.Ready)
                    {

                        foreach (var hero in ObjectManager.Get<Obj_AI_Hero>().Where(hero => hero.IsAlly && !hero.IsMe && !hero.IsDead && hero.IsValid))
                        {
                            if (hero.HealthPercentage() <= Jlib.getm_value("noti_shenhp"))
                            {
                                if (!text_Isrender)
                                    text_help.Add();
                                text_Isrender = true;

                                Jlib.targetPing(hero.Position.To2D(), Packet.PingType.AssistMe);
                            }
                            else
                            {
                                text_help.Remove();
                                text_Isrender = false;
                            }
                        }
                    }
                    else
                    {
                        text_help.Remove();
                        text_Isrender = false;
                    }
                }
                #endregion

                #region 정글타이머 - JungleTimer

                if (Game.Time - pastTime >= 1)
                {
                    pastTime = Game.Time;
                    foreach (var t in timerlist)
                    {
                        if (CheckMonster(t.name, t.Position, t.Range))
                        {
                            t.spawntime = t.respawntime;
                            t.show = false;
                        }
                        else
                        {
                            t.spawntime -= 1;

                            if (t.show)
                            {
                                if (!baseMenu.Item("jt_active").GetValue<bool>())
                                    t.show = false;
                            }
                            else if (baseMenu.Item("jt_active").GetValue<bool>())
                                t.show = true;
                        }


                        if (!baseMenu.Item("jt_active_minimap").GetValue<bool>())
                            t.minimap_show = false;
                        else
                            t.minimap_show = true;
                    }
                }
                #endregion

                #region Status on hud
                if (Jlib.getm_bool("base_stat"))
                {
                    

                    int x = Monitor.Width - Jlib.getm_value("x");
                    int y = Monitor.Height - Jlib.getm_value("y");
                    int interval = 20;
                    int i = 0;

                    Drawing.DrawText(x, y, Color.Wheat, " HuyNK " + Player.ChampionName);
                    i++;


                    if (smiteSlot != SpellSlot.Unknown)
                    {
                        addText(y + (interval * i), (Jlib.getm_bool("AutoSmite") && smiteSlot != SpellSlot.Unknown), "TU DONG TRUNG PHAT");
                        i++;
                    }

                    if (igniteSlot != SpellSlot.Unknown)
                    {
                        addText(y + (interval * i), (Jlib.getm_bool("AutoIgnite") && igniteSlot != SpellSlot.Unknown), "TU DONG THIEU DOT");
                        i++;
                    }

                    if (jumpspell != null)
                    {
                        addText(y + (interval * i), (Jlib.getm_bool("j2w_bool") && jumpspell != null), "CAM MAY NHẢY");
                        i++;
                    }
                    if (defslot != SpellSlot.Unknown)
                    {
                        addText(y + (interval * i), (Jlib.getm_bool("usespell") && defslot != SpellSlot.Unknown), string.Format("SpellCast{0}", filterspellname(Player.Spellbook.GetSpell(defslot).Name).ToUpper()));
                        i++;
                    }
                    if (Items.HasItem(Convert.ToInt32(ItemId.Crystalline_Flask)))
                    {
                        addText(y + (interval * i), (Jlib.getm_bool("useitem_flask")), string.Format("Use Flask({0}%)", Jlib.getm_value("useitem_p_flask")));
                        i++;
                    }
                    if (Items.HasItem(Convert.ToInt32(ItemId.Health_Potion)))
                    {
                        addText(y + (interval * i), (Jlib.getm_bool("useitem_hppotion")), string.Format("Use HP Potion({0}%)", Jlib.getm_value("useitem_p_hp")));
                        i++;
                    }
                    if (Items.HasItem(Convert.ToInt32(ItemId.Mana_Potion)))
                    {
                        addText(y + (interval * i), (Jlib.getm_bool("useitem_manapotion")), string.Format("Use Mana Potion({0}%)", Jlib.getm_value("useitem_p_mana")));
                        i++;
                    }
                    if (Items.HasItem(Convert.ToInt32(ItemId.Zhonyas_Hourglass)))
                    {
                        addText(y + (interval * i), (Jlib.getm_bool("useitem_zhonya")), string.Format("UseZhonya({0}%)", Jlib.getm_value("useitem_p_zhonya")));
                        i++;
                    }
                    if (Items.HasItem(Convert.ToInt32(ItemId.Blade_of_the_Ruined_King)))
                    {
                        addText(y + (interval * i), (Jlib.getm_bool("useitem_botrk")), string.Format("UseBOTRK({0}%)", Jlib.getm_value("useitem_p_botrk")));
                        i++;
                    }
                    if (Items.HasItem(Convert.ToInt32(ItemId.Mikaels_Crucible)))
                    {
                        addText(y + (interval * i), (Jlib.getm_bool("useitem_mikaels")), string.Format("Use Mikaels({0}%{1}", Jlib.getm_value("useitem_p_mikaels"),
                            Jlib.getm_bool("mikaels_cc_bool") ? ",CC)" : ")"));
                        i++;
                    }
                    if (Items.HasItem(Convert.ToInt32(ItemId.Quicksilver_Sash)))
                    {
                        addText(y + (interval * i), (Jlib.getm_bool("useitem_qs_bool")), string.Format("Use QS(delay:{0})", Jlib.getm_value("useitem_p_qs_delay")));
                        i++;
                    }
                    if (Items.HasItem(Convert.ToInt32(ItemId.Mercurial_Scimitar)))
                    {
                        addText(y + (interval * i), (Jlib.getm_bool("useitem_qs_bool")), string.Format("Use Scimitar(delay:{0})", Jlib.getm_value("useitem_p_qs_delay")));
                        i++;
                    }
                    //champ
                    #region stack
                    if (Player.ChampionName == "Twitch")
                    {
                        addText(y + (interval * i), (Jlib.getm_bool("st_twitch")), "CastTwitchE");
                        i++;
                    }
                    if (Player.ChampionName == "Kalista")
                    {
                        addText(y + (interval * i), (Jlib.getm_bool("st_kalista")), "CastKalistaE");
                        i++;
                    }
                    #endregion
                    #region notifier
                    if (Player.ChampionName == "Karthus")
                    {
                        addText(y + (interval * i), (Jlib.getm_bool("noti_karthus")), "UltNotifiler");
                    }
                    if (Player.ChampionName == "Ezreal" || Player.ChampionName == "Jinx" || Player.ChampionName == "Ashe")
                    {
                        addText(y + (interval * i), (Jlib.getm_bool("noti_ez")), "UltNotifiler");
                    }
                    if (Player.ChampionName == "Caitlyn")
                    {
                        addText(y + (interval * i), (Jlib.getm_bool("noti_cait")), "UltNotifiler");
                    }
                    if (Player.ChampionName == "Shen")
                    {
                        addText(y + (interval * i), (Jlib.getm_bool("noti_shen")), "UltNotifiler");
                    }
                    #endregion
                }
                #endregion

                #region AutoSmite
                if (Jlib.getm_bool("AutoSmite") && smiteSlot != SpellSlot.Unknown)
                {
                    if ((baseMenu.Item("smite_holdkey").GetValue<KeyBind>().Active || baseMenu.Item("smite_enablekey").GetValue<KeyBind>().Active))
                    {
                        double smitedamage;
                        bool smiteReady = false;
                        smitedamage = setSmiteDamage();
                        Drawing.DrawText(Player.HPBarPosition.X + 55, Player.HPBarPosition.Y + 25, System.Drawing.Color.Red, "AutoSmite!");

                        Obj_AI_Base mob = GetNearest(Player.ServerPosition);

                        if (Player.Spellbook.CanUseSpell(smiteSlot) == SpellState.Ready && Vector3.Distance(Player.ServerPosition, mob.ServerPosition) < smite.Range)
                        {
                            smiteReady = true;
                        }

                        if (smiteReady && mob.Health < smitedamage)
                        {
                            setSmiteSlot();
                            Player.Spellbook.CastSpell(smiteSlot, mob);
                        }
                    }
                }
                #endregion
        }

        // Addional Function //
        #region 스마이트함수 - Smite Function

        public static readonly int[] SmitePurple = { 3713, 3726, 3725, 3726, 3723 };
        public static readonly int[] SmiteGrey = { 3711, 3722, 3721, 3720, 3719 };
        public static readonly int[] SmiteRed = { 3715, 3718, 3717, 3716, 3714 };
        public static readonly int[] SmiteBlue = { 3706, 3710, 3709, 3708, 3707 };
        private static readonly string[] MinionNames =
        {
        "TT_Spiderboss", "TTNGolem", "TTNWolf", "TTNWraith",
            "SRU_Blue", "SRU_Gromp", "SRU_Murkwolf", "SRU_Razorbeak", "SRU_Red", "SRU_Krug", "SRU_Dragon", "SRU_BaronSpawn", "Sru_Crab"
        };


        public static void setSmiteSlot()
        {
            foreach (var spell in Player.Spellbook.Spells.Where(spell => String.Equals(spell.Name, smitetype(), StringComparison.CurrentCultureIgnoreCase)))
            {

                smiteSlot = spell.Slot;
                smite = new Spell(smiteSlot, 700);
                return;
            }
        }
        public static string smitetype()
        {
            if (Player.InventoryItems.Any(item => SmiteBlue.Any(t => t == Convert.ToInt32(item.Id))))
            {
                return "s5_summonersmiteplayerganker";
            }
            if (Player.InventoryItems.Any(item => SmiteRed.Any(t => t == Convert.ToInt32(item.Id))))
            {
                return "s5_summonersmiteduel";
            }
            if (Player.InventoryItems.Any(item => SmiteGrey.Any(t => t == Convert.ToInt32(item.Id))))
            {
                return "s5_summonersmitequick";
            }
            if (Player.InventoryItems.Any(item => SmitePurple.Any(t => t == Convert.ToInt32(item.Id))))
            {
                return "itemsmiteaoe";
            }
            return "summonersmite";
        }
        public static Obj_AI_Minion GetNearest(Vector3 pos)
        {
            return
                ObjectManager.Get<Obj_AI_Minion>().OrderBy(t=>Player.Distance(t.Position))
                    .First(minion => minion.IsValid &&
                        MinionNames.Any(name => minion.Name.StartsWith(name)) &&
                        !MinionNames.Any(name => minion.Name.Contains("Mini")));
        }
        public static double setSmiteDamage()
        {
            int level = Player.Level;
            int[] damage =
            {
                20*level + 370,
                30*level + 330,
                40*level + 240,
                50*level + 100
            };
            return damage.Max();
        }
        public static void testFind(Vector3 pos)
        {
            double? nearest = null;
            var minions =
                ObjectManager.Get<Obj_AI_Minion>()
                    .Where(minion => minion.IsValid);
            var objAiMinions = minions as Obj_AI_Minion[] ?? minions.ToArray();
            Obj_AI_Minion sMinion = objAiMinions.FirstOrDefault();
            foreach (Obj_AI_Minion minion in minions)
            {
                double distance = Vector3.Distance(pos, minion.Position);
                if (nearest == null || nearest > distance)
                {
                    nearest = distance;
                    sMinion = minion;
                }
            }
            Game.PrintChat("Minion name is: " + sMinion.Name);
        }
        #endregion

        #region  Ignite
        public static void setIgniteSlot()
        {
            foreach (var spell in Player.Spellbook.Spells.Where(spell => String.Equals(spell.Name, "summonerdot", StringComparison.CurrentCultureIgnoreCase)))
            {
                igniteSlot = spell.Slot;
                ignite = new Spell(smiteSlot, 600);
                return;
            }
        }

        public static float setigniteDamage()
        {
            float dmg = 50 + 20 * Player.Level;
            return dmg;
        }
        #endregion

        #region  Tracker
        public static string filterspellname(String a)
        {
            switch (a)
            {
                case "s5_summonersmiteplayerganker":
                    a = "BSmite"; break;
                case "s5_summonersmiteduel":
                    a = "RSmite"; break;
                case "s5_summonersmitequick":
                    a = "Smite"; break;
                case "itemsmiteaoe":
                    a = "Smite"; break;
                default:
                    break;
            }
            a = a.Replace("summoner", "").Replace("dot", "ignite");

            return a;
        }
        
        private static void OnCreate(GameObject sender, EventArgs args)
        {

        }

        private static void OnDelete(GameObject sender, EventArgs args)
        {
            if (wardnames.Contains(sender.Name))
            {
                foreach (var ward in wardlist.Where(w => w.id == sender.NetworkId))
                {
                    ward.show = false;
                    wardlist.Remove(ward);
                }
            }
        }
        #endregion

        #region  J2W
        public static void setj2wslots(List<String> a)
        {
            foreach (String champname in a)
            {
                if (champname == Player.ChampionName)
                {
                    canw2j = true;
                    switch (champname)
                    {
                        case "LeeSin":
                            jumpspell = new Spell(SpellSlot.W, 700);
                            return;
                        case "Katarina":
                            jumpspell = new Spell(SpellSlot.E, 700);
                            return;
                        case "Jax":
                            jumpspell = new Spell(SpellSlot.Q, 700);
                            return;
                    }
                }
            }
        }
        public static void checkE()
        {
            if (Player.ChampionName == "LeeSin")
            {
                rdyw2j = jumpspell.IsReady() && Player.Spellbook.GetSpell(SpellSlot.W).Name == "BlindMonkWOne";
            }
            else
            {
                rdyw2j = jumpspell.IsReady();
            }
        }

        public static void checkWard()
        {
            var Slot = Items.GetWardSlot();
            rdyward = !(Slot == null || Slot.Stacks == 0);
        }


        #endregion

        #region  Item & Spell
        public static void getbufflist(List<BuffType> list,ItemId whatitem)
        {
            if (whatitem == ItemId.Mikaels_Crucible)
            {
                if (Jlib.getm_bool("mikaels_cc_stun"))
                    list.Add(BuffType.Stun);
                if (Jlib.getm_bool("mikaels_cc_fear"))
                {
                    list.Add(BuffType.Fear);
                    list.Add(BuffType.Flee);
                }
                if (Jlib.getm_bool("mikaels_cc_charm"))
                    list.Add(BuffType.Charm);
                if (Jlib.getm_bool("mikaels_cc_taunt"))
                    list.Add(BuffType.Taunt);
                if (Jlib.getm_bool("mikaels_cc_snare"))
                    list.Add(BuffType.Snare);
                if (Jlib.getm_bool("mikaels_cc_silence"))
                    list.Add(BuffType.Silence);
                if (Jlib.getm_bool("mikaels_cc_polymorph"))
                    list.Add(BuffType.Polymorph);
            }
            if (whatitem == ItemId.Quicksilver_Sash)
            {
                if (Jlib.getm_bool("qs_cc_stun"))
                    list.Add(BuffType.Stun);
                if (Jlib.getm_bool("qs_cc_fear"))
                {
                    list.Add(BuffType.Fear);
                    list.Add(BuffType.Flee);
                }
                if (Jlib.getm_bool("qs_cc_charm"))
                    list.Add(BuffType.Charm);
                if (Jlib.getm_bool("qs_cc_taunt"))
                    list.Add(BuffType.Taunt);
                if (Jlib.getm_bool("qs_cc_snare"))
                    list.Add(BuffType.Snare);
                if (Jlib.getm_bool("qs_cc_silence"))
                    list.Add(BuffType.Silence);
                if (Jlib.getm_bool("qs_cc_polymorph"))
                    list.Add(BuffType.Polymorph);
                if (Jlib.getm_bool("qs_cc_suppression"))
                    list.Add(BuffType.Suppression);
            }
           
        }
        public static void setDefSpellSlot()
        {
            foreach (var spell in Player.Spellbook.Spells.Where(spell => spell.Name.Contains(DefSpellstr[0]) || spell.Name.Contains(DefSpellstr[1])))
            {
                defslot = spell.Slot;
                defspell = new Spell(defslot);
                return;
            }
        }
        #endregion

        #region  Stack
        public static double getKaliDmg(Obj_AI_Base target,int count,double AD,int s_level)
        {
            double[] spell_basedamage ={0,20,30,40,50,60};
            double[] spell_perdamage ={0,0.25,0.30,0.35,0.40,0.45};
            double eDmg = AD * 0.60 + spell_basedamage[s_level];
            count -= 1;
            eDmg = eDmg + count*(eDmg * spell_perdamage[s_level]);
            return Player.CalcDamage(target, Damage.DamageType.Physical,eDmg);
        }
        public static double getTwitEDmg(Obj_AI_Base target, int count, double AD,double AP, int s_level)
        {
            double[] spell_basedamage = { 0, 20, 35, 50, 65, 80 };
            double[] spell_stackdamage = { 0, 15, 20, 25, 30, 35 };
            double eDmg = spell_basedamage[s_level] + count * (spell_stackdamage[s_level] + (AP * 0.2) + (AD * 0.25));
            return Player.CalcDamage(target, Damage.DamageType.Physical, eDmg);
        }
        #endregion

        #region  JungleTimer
        public static bool CheckMonster(String TargetName, Vector3 BasePosition ,int Range = 1000)
        {

            var minions = ObjectManager.Get<Obj_AI_Minion>()
                .Where(minion => minion.IsValid && !minion.IsDead && minion.Name.StartsWith(TargetName));
            var objAiMinions = minions as Obj_AI_Minion[] ?? minions.ToArray();

            if (!objAiMinions.Any(m => m.Distance(BasePosition) < Range))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public static String Clockstring(int spawntime)
        {
            var min = spawntime / 60;
            var sec = spawntime - min * 60;
            if (spawntime > 0)
            {
                return String.Format("{0:00}:{1:00}", min, sec);
            }
            else
                return "";
        }

        public class timer_clock
        {
            public Vector3 Position;
            public string name;
            public bool show = false;
            public bool minimap_show = true;
            public int respawntime = 100;
            public int spawntime = 0;
            public int Range = 1000;
            public Render.Text timer { get; set; }
            public Render.Text timer_minimap { get; set; }

            public timer_clock()
            {

                timer = new Render.Text(Position.To2D(), "", baseMenu.Item("jt_font_size").GetValue<Slider>().Value
                    , SharpDX.Color.White)
                {
                    VisibleCondition =
                    condition =>
                          show,

                    PositionUpdate = delegate
                    {
                        Vector2 vec2 = Drawing.WorldToScreen(Position);
                        return vec2;
                    },
                    TextUpdate = () => Clockstring(spawntime),
                    OutLined = true,
                    Centered = true
                };

                timer_minimap = new Render.Text(Position.To2D(), "", 
                    baseMenu.Item("jt_font_size_minimap").GetValue<Slider>().Value
                    , SharpDX.Color.White)
                {
                    VisibleCondition =
                    condition =>
                          show && minimap_show,

                    PositionUpdate = delegate
                    {
                        Vector2 v2 = Drawing.WorldToMinimap(Position);
                        return v2;
                    },
                    TextUpdate = () => Clockstring(spawntime),
                    OutLined = true,
                    Centered = true
                };
                timer_minimap.Add();

                timer.Add();
                timer_minimap.Add();
                timerlist.Add(this);
            }
        }
        #endregion

        #region  Status
        public static void addText(float y,bool a,String b)
        {
            Drawing.DrawText(Monitor.Width - Jlib.getm_value("x"), y, a ? Color.FromArgb(0, 255, 0) : Color.Red,
                b+"[" + (a ? "ON" :"OFF") + "]");
        }
        #endregion


    }
}



