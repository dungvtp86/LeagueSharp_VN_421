#region

using LeagueSharp;
using LeagueSharp.Common;

#endregion

namespace Teemo___The_Satan_Yordle
{
    public class Modes
    {
        public static Obj_AI_Hero Player { get { return ObjectManager.Player; } }
        private static readonly Menu Menu = Program.Menu;
        private static Spell Q { get { return SpellHandler.Q; } }
        private static Spell W { get { return SpellHandler.W; } }
        private static Spell R { get { return SpellHandler.R; } }

        public static void AAreset()
        {
            

        }

        public static void Combo()
        {

            var TG = TargetSelector.GetTarget(Q.Range, TargetSelector.DamageType.Physical);
            var PacketE = Menu.Item("Packets").GetValue<bool>();
            var shroomOnC = Menu.Item("ShroomOn").GetValue<StringList>().SelectedIndex;
            if (shroomOnC == 1)
                Shroom.AutoR();

            if (TG == null || Player.IsDead) return;

            ItemHandler.ItemUse();

            if (Menu.Item("UseW").GetValue<bool>() && W.IsReady())
            {
                if (TG.Distance(ObjectManager.Player) > Orbwalking.GetRealAutoAttackRange(ObjectManager.Player))

                    W.Cast();
            }

            if (Menu.Item("UseR").GetValue<bool>() && R.IsReady())
            {
                if (TG.IsValidTarget(R.Range))
                    R.Cast(TG.Position, PacketE);
            }

        }

        public static void Harass()
        {
            var TG = TargetSelector.GetTarget(Q.Range, TargetSelector.DamageType.Physical);
            var TGW = TargetSelector.GetTarget(Orbwalking.GetRealAutoAttackRange(ObjectManager.Player) + 150, TargetSelector.DamageType.Physical);
            var PacketE = Menu.Item("Packets").GetValue<bool>();

            if (Player.Mana / Player.MaxMana * 100 < Menu.Item("HarassMana").GetValue<Slider>().Value) return;
            if (Menu.Item("UseWH").GetValue<bool>() && R.IsReady())
            {
                if (TGW.Distance(ObjectManager.Player) >= Orbwalking.GetRealAutoAttackRange(ObjectManager.Player))
                    W.Cast();
            }
            if (Menu.Item("UseRH").GetValue<bool>() && R.IsReady())
            {
                if (TG.IsValidTarget(R.Range))
                    R.Cast(TG.Position, PacketE);
            }


        }



    }
}
