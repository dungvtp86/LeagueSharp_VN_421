
#region

using System;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;


#endregion

namespace Teemo___The_Satan_Yordle
{
    public class Program
    {
        public static Obj_AI_Hero Player { get { return ObjectManager.Player; } }
        public static Menu Menu;
        private static Spell Q { get { return SpellHandler.Q; } }
        private static Spell W { get { return SpellHandler.W; } }
        private static Spell R { get { return SpellHandler.R; } }

        public const string VersionE = "1.1.0";

        static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += Game_OnGameLoad;
        }


        private static void Game_OnGameLoad(EventArgs args)
        {
            if (Player.ChampionName != "Teemo") //Satan
                return;


            //Base menu
            Menu = new Menu("Teemo Satan Yordle", "Teemo Satan Yordle", true);

            #region TargetSelector and Orbwalker

            Menu.AddSubMenu(new Menu("Target Selector", "Target Selector"));
            TargetSelector.AddToMenu(Menu.SubMenu("Target Selector"));
            Menu.AddSubMenu(new Menu("Orbwalker", "Orbwalker"));
            new Orbwalking.Orbwalker(Menu.SubMenu("Orbwalker"));

            #endregion
            MenuHandler.MenuHandlerRun();
            Menu.AddToMainMenu();

            SpellHandler.SpellHandlerLoad();
            Shroom.ShroomLoad();
            ItemHandler.ItemHandlerLoad();

            Drawing.OnDraw += Drawing_OnDraw;
            Game.OnGameUpdate += Game_OnGameUpdate;
            AntiGapcloser.OnEnemyGapcloser += AntiGapcloser_OnEnemyGapcloser;
            Orbwalking.AfterAttack += Orbwalking_AfterAttack;


            Game.PrintChat("<font color=\"#33CC00\">Teemo</font> - Satan Yordle v" + VersionE + " By <font color=\"#0066FF\">E2Slayer</font>");
        }



        /* 
         ========================
         * Drawing Part 
         ========================
         */



        private static void Drawing_OnDraw(EventArgs args)
        {
            if (Player.IsDead)
                return;

            var drawq = Menu.Item("DrawQ").GetValue<Circle>();
            var drawr = Menu.Item("DrawR").GetValue<Circle>();

            if (drawq.Active)
                Utility.DrawCircle(Player.Position, Q.Range, drawq.Color);
            if (drawr.Active)
                Utility.DrawCircle(Player.Position, R.Range, drawr.Color);

        } // Drawing End

        /* 
         ========================
         * GameUpdate
         ========================
         */
        private static void Game_OnGameUpdate(EventArgs args)
        {
            if (Player.IsDead) return;

            var shroomOnC = Menu.Item("ShroomOn").GetValue<StringList>().SelectedIndex;
            if (shroomOnC == 0) // 0 is Shroom Always On
                Shroom.AutoR();

            if (Menu.Item("ComboActive").GetValue<KeyBind>().Active)
                Modes.Combo();
            else if (Menu.Item("HarassActive").GetValue<KeyBind>().Active || Menu.Item("HarassActiveT").GetValue<KeyBind>().Active)
                Modes.Harass();
        }



        /* 
       ========================
       * Use Q to Gap-Closer
      * Probably needs some improvements, keep working
       ========================
       */

        private static void AntiGapcloser_OnEnemyGapcloser(ActiveGapcloser gapcloser)
        {
            
            if (!Menu.Item("GapQ").GetValue<bool>()) return;
            var PacketE = Menu.Item("Packets").GetValue<bool>();
            if (Q.IsReady() && gapcloser.Sender.IsValidTarget(Q.Range))//&& Player.Distance(gapcloser.Sender, true) <= 500 
            {
                Q.Cast(gapcloser.Sender, PacketE);
            }
        }

        /* 
             
          
   ========================
   * AA reset
   ========================
   */
        private static void Orbwalking_AfterAttack(AttackableUnit unit, AttackableUnit target)
        {
            var ComA = Menu.Item("ComboActive").GetValue<KeyBind>().Active;
            var HarA = Menu.Item("HarassActive").GetValue<KeyBind>().Active;
            var HarAT = Menu.Item("HarassActiveT").GetValue<KeyBind>().Active;
            var PacketE = Menu.Item("Packets").GetValue<bool>();
            var target1 = TargetSelector.GetTarget(Q.Range, TargetSelector.DamageType.Physical);
            var minion1 = MinionManager.GetMinions(Q.Range, MinionTypes.All, MinionTeam.Enemy, MinionOrderTypes.MaxHealth);

            if (Menu.Item("LaneClearA").GetValue<KeyBind>().Active && Menu.Item("UseQL").GetValue<bool>() && Q.IsReady())
            {
                foreach (
                    Obj_AI_Base minions in
                        minion1.Where(
                            minions => minions.Health < ObjectManager.Player.GetSpellDamage(minions, SpellSlot.Q)))
                {
                    Q.Cast(minions, PacketE);
                }
            }
                
            if ((ComA || HarA || HarAT) && unit.IsMe && (target is Obj_AI_Hero))
            {
                if (Menu.Item("UseQ").GetValue<bool>() && ComA && Q.IsReady())
                    Q.Cast(target1, PacketE);
                else if (Menu.Item("UseQH").GetValue<bool>() && (HarA || HarAT) && Q.IsReady())
                {
                    if (Player.Mana / Player.MaxMana * 100 < Menu.Item("HarassMana").GetValue<Slider>().Value) return;
                    Q.Cast(target1, PacketE);
                }
            }
        }

  

    }// class end
}
