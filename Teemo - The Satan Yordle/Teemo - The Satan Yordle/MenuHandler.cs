#region

using LeagueSharp;
using LeagueSharp.Common;

using Color = System.Drawing.Color;


#endregion

namespace Teemo___The_Satan_Yordle
{
    public class MenuHandler
    {


        private static Obj_AI_Hero Player = ObjectManager.Player;
        private static readonly Menu Menu = Program.Menu;

        private static Menu _combo;
        private static MenuItem _comboq, _combow, _combor, _comboActive;

        private static Menu _harass;
        private static MenuItem _harassq, _harassw, _harassr, _harassMana, _harassActive, _harassToggle;

        private static Menu _lineclear;
        private static MenuItem _lineclearq, _lineclearw, _lineclearr, _lineclearActive;

        private static Menu _rsetting;
        private static MenuItem _shroomh, _shroomm, _shroomType;

        private static Menu _misc;
        private static MenuItem _packet, _gabcloser;

        private static Menu _drawing;
        private static MenuItem _drwaingq, _drwaingr, _drwaingshroomh, _drwaingshroomm, _drwaingshroomV;

        
        public static void MenuHandlerRun()
        {



      
            _combo = (new Menu("Combo", "Combo"));
            _comboq = (new MenuItem("UseQ", "Use Q").SetValue(true));
            _combow = (new MenuItem("UseW", "Use W").SetValue(true));
            _combor = (new MenuItem("UseR", "Use R").SetValue(true));
            _comboActive = (new MenuItem("ComboActive", "Combo").SetValue(new KeyBind(32, KeyBindType.Press)));

            _harass = (new Menu("Harass", "Harass"));
            _harassq = (new MenuItem("UseQH", "Use Q").SetValue(true));
            _harassw = (new MenuItem("UseWH", "Use W").SetValue(false));
            _harassr = (new MenuItem("UseRH", "Use R").SetValue(true));
            _harassMana = (new MenuItem("HarassMana", "Only Harass if mana >").SetValue(new Slider(0, 0, 100)));
            _harassActive = (new MenuItem("HarassActive", "Harass").SetValue(new KeyBind("C".ToCharArray()[0], KeyBindType.Press)));
            _harassToggle = (new MenuItem("HarassActiveT", "Harass (Toggle)").SetValue(new KeyBind("Y".ToCharArray()[0], KeyBindType.Toggle, false)));

            _lineclear = (new Menu("LaneClear", "LaneClear"));
            _lineclearq = (new MenuItem("UseQL", "Use Q").SetValue(false));
            _lineclearw = (new MenuItem("UseWL", "Use WL").SetValue(false));
            _lineclearr = (new MenuItem("UseRL", "Use RL").SetValue(false));
            _lineclearActive = (new MenuItem("LaneClearA", "LaneClear").SetValue(new KeyBind("X".ToCharArray()[0], KeyBindType.Press)));

            _rsetting = (new Menu("R Settings", "Rsettings"));
            _shroomh = (new MenuItem("ShroomH", "Auto Use R on High Priorities").SetValue(true));
            _shroomm = (new MenuItem("ShroomM", "Auto Use R on Midium Priorities").SetValue(true));
            _shroomType = (new MenuItem("ShroomOn", "Auto Use R").SetValue(new StringList(new[] { "Always", "Only Combo", "No" }, 0)));

            _misc = (new Menu("Misc", "Misc"));
            _packet = (new MenuItem("Packets", "Packet Casting").SetValue(false));
            _gabcloser = (new MenuItem("GapQ", "Use Q for Gap Closer").SetValue(true));

            _drawing = new Menu("Drawings", "Drawing");
            _drwaingq = new MenuItem("DrawQ", "Q Range").SetValue(new Circle(false, Color.Lime));
            _drwaingr = new MenuItem("DrawR", "R Range").SetValue(new Circle(false, Color.Azure));
            _drwaingshroomh = new MenuItem("ShroomH1", "Shroom High Priorities").SetValue(true);
            _drwaingshroomm = new MenuItem("ShroomM1", "Shroom Midium Priorities").SetValue(true);
            _drwaingshroomV = new MenuItem("ShroomV", "Shroom Vision Range").SetValue(new Slider(1500, 4000, 1000));

           
            //Combo
            Menu.AddSubMenu(_combo);
            Menu.SubMenu("Combo").AddItem(_comboq);
            Menu.SubMenu("Combo").AddItem(_combow);
            Menu.SubMenu("Combo").AddItem(_combor);
            Menu.SubMenu("Combo").AddItem(_comboActive);

            
            //Harass
            Menu.AddSubMenu(_harass);
            Menu.SubMenu("Harass").AddItem(_harassq);
            Menu.SubMenu("Harass").AddItem(_harassw);
            Menu.SubMenu("Harass").AddItem(_harassr);
            Menu.SubMenu("Harass").AddItem(_harassMana);
            Menu.SubMenu("Harass").AddItem(_harassActive);
            Menu.SubMenu("Harass").AddItem(_harassToggle);
              
            Menu.AddSubMenu(_lineclear);
            Menu.SubMenu("LaneClear").AddItem(_lineclearq);
            Menu.SubMenu("LaneClear").AddItem(_lineclearActive);

            
            Menu.AddSubMenu(new Menu("Items", "Items"));
            Menu.SubMenu("Items").AddItem(new MenuItem("DFG1", "Deathfire Grasp").SetValue(true));
            Menu.SubMenu("Items").AddItem(new MenuItem("Cutlass1", "Bilgewater's Cutlass").SetValue(true));
            Menu.SubMenu("Items").AddItem(new MenuItem("Hextech1", "Hextech Gunblade").SetValue(true));
            Menu.SubMenu("Items").AddItem(new MenuItem("Frostclaim1", "Frost Queen's Claim").SetValue(true));
            Menu.SubMenu("Items").AddItem(new MenuItem("Botrk1", "Blade of the Ruined King").SetValue(true));
            Menu.SubMenu("Items").AddItem(new MenuItem("Youmuus1", "Youmuu's Ghostblade").SetValue(true));
           
            

            //R settings
            Menu.AddSubMenu(_rsetting);
            Menu.SubMenu("Rsettings").AddItem(_shroomh);
            Menu.SubMenu("Rsettings").AddItem(_shroomm);
            Menu.SubMenu("Rsettings").AddItem(_shroomType);


            //Misc
            Menu.AddSubMenu(_misc);
               Menu.SubMenu("Misc").AddItem(_packet);
              Menu.SubMenu("Misc").AddItem(_gabcloser);


              //Drawings
              Menu.AddSubMenu(_drawing);
              Menu.SubMenu("Drawing").AddItem(_drwaingq);
              Menu.SubMenu("Drawing").AddItem(_drwaingr);

              Menu.SubMenu("Drawing").AddItem(_drwaingshroomh);
              Menu.SubMenu("Drawing").AddItem(_drwaingshroomm);
              Menu.SubMenu("Drawing").AddItem(_drwaingshroomV);


           


        }
    }
}
