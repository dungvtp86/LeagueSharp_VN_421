#region

using LeagueSharp.Common;

#endregion

namespace Teemo___The_Satan_Yordle
{
    public class ItemHandler
    {
        public static Items.Item Dfg, Botrk, Frostclaim, Youmuus, Hextech, Cutlass;
        private static Spell Q { get { return SpellHandler.R; } }
        private static readonly Menu Menu = Program.Menu;

        public static void ItemHandlerLoad()
        {

            Dfg = Utility.Map.GetMap().Type == Utility.Map.MapType.TwistedTreeline ? new Items.Item(3188, 750) : new Items.Item(3128, 750);
            //  DFG = new Items.Item(3128, 750);
            Cutlass = new Items.Item(3144, 450);
            Hextech = new Items.Item(3146, 700);
            Frostclaim = new Items.Item(3092, 850);
            Botrk = new Items.Item(3153, 450);
            Youmuus = new Items.Item(3142, 650);

        }

        public static void ItemUse()
        {
            var tg = TargetSelector.GetTarget(Q.Range, TargetSelector.DamageType.Physical);

            if (tg == null) return;

            if (Dfg.IsReady() && Menu.Item("DFG1").GetValue<bool>())
                Dfg.Cast(tg);
            if (Cutlass.IsReady() && Menu.Item("Cutlass1").GetValue<bool>())
                Cutlass.Cast(tg);
            if (Hextech.IsReady() && Menu.Item("Hextech1").GetValue<bool>())
                Hextech.Cast(tg);
            if (Frostclaim.IsReady() && Menu.Item("Frostclaim1").GetValue<bool>())
                Frostclaim.Cast(tg);
            if (Botrk.IsReady() && Menu.Item("Botrk1").GetValue<bool>())
                Botrk.Cast(tg);
            if (Youmuus.IsReady() && Menu.Item("Youmuus1").GetValue<bool>())
                Youmuus.Cast();
        }

    }
}
