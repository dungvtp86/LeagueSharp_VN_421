#region

using LeagueSharp;
using LeagueSharp.Common;

#endregion


namespace Teemo___The_Satan_Yordle
{
    public class SpellHandler
    {
        
        public static Spell Q, W, R; // Don't need E 

        public static void SpellHandlerLoad()
        {
            Q = new Spell(SpellSlot.Q, 580);
            W = new Spell(SpellSlot.W);
            R = new Spell(SpellSlot.R, 230);
            Q.SetTargetted(0f, 2000f);
            R.SetSkillshot(0.1f, 75f, float.MaxValue, false, SkillshotType.SkillshotCircle);
        }
    }
}
