#region

using System;
using System.Collections.Generic;
using System.Linq;

using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using Color = System.Drawing.Color;



#endregion


namespace Teemo___The_Satan_Yordle
{
    public class Shroom
    {
        
        public static Obj_AI_Hero Player { get { return ObjectManager.Player; } }
        public static ShroomTables ShroomPositions;
        private static readonly Menu Menu = Program.Menu;

        private static Spell R { get { return SpellHandler.R; }}


        public static void ShroomLoad()
        {
            ShroomPositions = new ShroomTables();
            Drawing.OnDraw += Drawing_OnDraw;

          //  AutoR();
        }

        private static void Drawing_OnDraw(EventArgs args)
        {
            var high1 = Menu.Item("ShroomH1").GetValue<bool>();
            var mid1 = Menu.Item("ShroomM1").GetValue<bool>();

            if (!high1 && !mid1 || Player.IsDead) return;
            if (Menu.Item("ShroomH1").GetValue<bool>())
            {
                foreach (
                    var pos in
                        ShroomPositions.HighPriority.Where(
                            shrom =>
                                shrom.Distance(ObjectManager.Player.Position) <=
                                Menu.Item("ShroomV").GetValue<Slider>().Value))
                {
                    Utility.DrawCircle(pos, 50, Color.Red);
                }
            }

            if (!Menu.Item("ShroomM1").GetValue<bool>()) return;
            foreach (
                var pos in
                    ShroomPositions.MediumPriority.Where(
                        shrom =>
                            shrom.Distance(ObjectManager.Player.Position) <=
                            Menu.Item("ShroomV").GetValue<Slider>().Value))
            {
                Utility.DrawCircle(pos, 50, Color.Yellow);
            }
        }

        public static void AutoR()
        {
            var high = Menu.Item("ShroomH").GetValue<bool>();
            var mid = Menu.Item("ShroomM").GetValue<bool>();

            if (!high && !mid) return;
             var packetE = Menu.Item("Packets").GetValue<bool>();
            
            if (Menu.Item("ShroomH").GetValue<bool>())
            {
                foreach (var place in ShroomPositions.HighPriority.Where(pos => pos.Distance(Player.Position) <= R.Range && !IsShroomed(pos)))
                {
                    R.Cast(place, packetE);
                }
            }
            if (!Menu.Item("ShroomM").GetValue<bool>()) return;
            foreach (var place in ShroomPositions.MediumPriority.Where(pos => pos.Distance(Player.Position) <= R.Range && !IsShroomed(pos)))
            {
                R.Cast(place, packetE);
            }
        }


        /* 
================================================================================================================================================
* Shroom Pos
* Made by : LXMedia1 in UltimateCarry2
================================================================================================================================================
*/


        
        private static bool IsShroomed(Vector3 position)
        {
            return ObjectManager.Get<Obj_AI_Base>().Where(obj => obj.Name == "Noxious Trap").Any(obj => position.Distance(obj.Position) <= 250);
        }

        public class ShroomTables
        {
            public List<Vector3> HighPriority = new List<Vector3>();
            public List<Vector3> MediumPriority = new List<Vector3>();


            public ShroomTables()
            {
                CreateTables();
                var templist = (from pos in HighPriority
                                let x = pos.X
                                let y = pos.Y
                                let z = pos.Z
                                select new Vector3(x, z, y)).ToList();
                HighPriority = templist;
                templist = (from pos in MediumPriority
                            let x = pos.X
                            let y = pos.Y
                            let z = pos.Z
                            select new Vector3(x, z, y)).ToList();
                MediumPriority = templist;


            }

         private void CreateTables()
            {
                //My own location.
                HighPriority.Add(new Vector3(10406, 50.08506f, 3050));
                HighPriority.Add(new Vector3(10202, -71.2406f, 4844));
                HighPriority.Add(new Vector3(11222, -2.869444f, 5592));
                HighPriority.Add(new Vector3(10032, 49.70721f, 6610));
                HighPriority.Add(new Vector3(8580, -50.36785f, 5560));
                HighPriority.Add(new Vector3(11960, 52.09994f, 7400));
                HighPriority.Add(new Vector3(4804, 40.283f, 8334));
                HighPriority.Add(new Vector3(6264, -62.41959f, 9332));
                HighPriority.Add(new Vector3(4724, -71.2406f, 10024));
                HighPriority.Add(new Vector3(3636, -8.188844f, 9348));
                HighPriority.Add(new Vector3(4452, 56.8484f, 11810));
                HighPriority.Add(new Vector3(2848, 51.84816f, 7362));

                MediumPriority.Add(new Vector3(10910, -26.55536f, 3456));
                MediumPriority.Add(new Vector3(11844, -67.9031f, 3902));
                MediumPriority.Add(new Vector3(9430, -71.2406f, 5664));
                MediumPriority.Add(new Vector3(10004, 51.98113f, 7218));
                MediumPriority.Add(new Vector3(10236, 49.54523f, 8794));
                MediumPriority.Add(new Vector3(12608, 51.69598f, 7278));
                MediumPriority.Add(new Vector3(11548, 52.21631f, 7656));
                MediumPriority.Add(new Vector3(12534, 51.7294f, 5160));
                MediumPriority.Add(new Vector3(11748, -63.7501f, 3202));
                MediumPriority.Add(new Vector3(7830, 51.69787f, 5726));
                MediumPriority.Add(new Vector3(8644, 52.32272f, 4836));
                MediumPriority.Add(new Vector3(6568, 48.527f, 4702));
                MediumPriority.Add(new Vector3(7408, 52.50325f, 2650));
                MediumPriority.Add(new Vector3(5514, 51.38131f, 3544));
                MediumPriority.Add(new Vector3(8398, -71.2406f, 6456));
                MediumPriority.Add(new Vector3(6552, -71.2406f, 8324));
                MediumPriority.Add(new Vector3(4636, 51.24505f, 6164));
                MediumPriority.Add(new Vector3(4778, 52.83177f, 7654));
                MediumPriority.Add(new Vector3(2212, 50.37255f, 7548));
                MediumPriority.Add(new Vector3(3272, 51.84087f, 7152));
                MediumPriority.Add(new Vector3(2304, 53.16499f, 9716));
                MediumPriority.Add(new Vector3(3944, -22.74386f, 11382));
                MediumPriority.Add(new Vector3(2794, 21.37915f, 11938));
                MediumPriority.Add(new Vector3(2978, -70.66211f, 11060));
                MediumPriority.Add(new Vector3(7056, 52.86944f, 9068));
                MediumPriority.Add(new Vector3(6208, 54.84456f, 10086));
                MediumPriority.Add(new Vector3(8264, 49.92609f, 10242));
                MediumPriority.Add(new Vector3(7304, 56.4768f, 12466));
                MediumPriority.Add(new Vector3(9310, 53.27245f, 11322));

                MediumPriority.Add(new Vector3(9196, 58.74224f, 2126));
                MediumPriority.Add(new Vector3(7044, 52.57567f, 3070));
                MediumPriority.Add(new Vector3(7754, 55.90973f, 11818));
                MediumPriority.Add(new Vector3(5666, 52.8381f, 12748));
                MediumPriority.Add(new Vector3(5344, -71.2406f, 9232));
            }
        }

        

    }
}
