﻿#region LICENSE

/*
 Copyright 2014 - 2014 LeagueSharp
 Interrupter.cs is part of LeagueSharp.Common.
 
 LeagueSharp.Common is free software: you can redistribute it and/or modify
 it under the terms of the GNU General Public License as published by
 the Free Software Foundation, either version 3 of the License, or
 (at your option) any later version.
 
 LeagueSharp.Common is distributed in the hope that it will be useful,
 but WITHOUT ANY WARRANTY; without even the implied warranty of
 MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 GNU General Public License for more details.
 
 You should have received a copy of the GNU General Public License
 along with LeagueSharp.Common. If not, see <http://www.gnu.org/licenses/>.
*/

#endregion

#region

using System;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace LeagueSharp.Common
{
    public delegate void OnPosibleToInterruptH(Obj_AI_Base unit, InterruptableSpell spell);

    public delegate void OnPossibleToInterruptH(Obj_AI_Base unit, InterruptableSpell spell);

    public enum InterruptableDangerLevel
    {
        Low,
        Medium,
        High,
    }

    public struct InterruptableSpell
    {
        public string BuffName;
        public string ChampionName;
        public InterruptableDangerLevel DangerLevel;
        public int ExtraDuration;
        public SpellSlot Slot;
        public string SpellName;
    }

    /// <summary>
    ///     This class allows you to easily interrupt interruptable spells like Katarina's ult.
    /// </summary>
    public static class Interrupter
    {
        public static List<InterruptableSpell> Spells = new List<InterruptableSpell>();

        static Interrupter()
        {
            #region Varus

            Spells.Add(
                new InterruptableSpell
                {
                    ChampionName = "Varus",
                    SpellName = "VarusQ",
                    DangerLevel = InterruptableDangerLevel.Low,
                    Slot = SpellSlot.Q,
                    BuffName = "VarusQ"
                });

            #endregion

            #region Urgot

            Spells.Add(
                new InterruptableSpell
                {
                    ChampionName = "Urgot",
                    SpellName = "UrgotSwap2",
                    DangerLevel = InterruptableDangerLevel.High,
                    Slot = SpellSlot.R,
                    BuffName = "UrgotSwap2"
                });

            #endregion

            #region Caitlyn

            Spells.Add(
                new InterruptableSpell
                {
                    ChampionName = "Caitlyn",
                    SpellName = "CaitlynAceintheHole",
                    DangerLevel = InterruptableDangerLevel.High,
                    Slot = SpellSlot.R,
                    BuffName = "CaitlynAceintheHole",
                    ExtraDuration = 600
                });

            #endregion

            #region Warwick

            Spells.Add(
                new InterruptableSpell
                {
                    ChampionName = "Warwick",
                    SpellName = "InfiniteDuress",
                    DangerLevel = InterruptableDangerLevel.High,
                    Slot = SpellSlot.R,
                    BuffName = "infiniteduresssound"
                });

            #endregion

            #region Shen

            Spells.Add(
                new InterruptableSpell
                {
                    ChampionName = "Shen",
                    SpellName = "ShenStandUnited",
                    DangerLevel = InterruptableDangerLevel.Low,
                    Slot = SpellSlot.R,
                    BuffName = "NobodyPlaysShen:^)"
                });

            #endregion

            #region Malzahar

            Spells.Add(
                new InterruptableSpell
                {
                    ChampionName = "Malzahar",
                    SpellName = "AlZaharNetherGrasp",
                    DangerLevel = InterruptableDangerLevel.High,
                    Slot = SpellSlot.R,
                    BuffName = "alzaharnethergraspsound",
                });

            #endregion

            #region Nunu

            Spells.Add(
                new InterruptableSpell
                {
                    ChampionName = "Nunu",
                    SpellName = "AbsoluteZero",
                    DangerLevel = InterruptableDangerLevel.High,
                    Slot = SpellSlot.R,
                    BuffName = "AbsoluteZero",
                });

            #endregion

            #region Pantheon

            Spells.Add(
                new InterruptableSpell
                {
                    ChampionName = "Pantheon",
                    SpellName = "PantheonRJump",
                    DangerLevel = InterruptableDangerLevel.High,
                    Slot = SpellSlot.R,
                    BuffName = "PantheonRJump"
                });

            #endregion

            #region Karthus

            Spells.Add(
                new InterruptableSpell
                {
                    ChampionName = "Karthus",
                    SpellName = "KarthusFallenOne",
                    DangerLevel = InterruptableDangerLevel.High,
                    Slot = SpellSlot.R,
                    BuffName = "karthusfallenonecastsound"
                });

            #endregion

            #region Velkoz

            Spells.Add(
                new InterruptableSpell
                {
                    ChampionName = "Velkoz",
                    SpellName = "VelkozR",
                    DangerLevel = InterruptableDangerLevel.High,
                    Slot = SpellSlot.R,
                    BuffName = "VelkozR",
                });

            #endregion

            #region Galio

            Spells.Add(
                new InterruptableSpell
                {
                    ChampionName = "Galio",
                    SpellName = "GalioIdolOfDurand",
                    DangerLevel = InterruptableDangerLevel.High,
                    Slot = SpellSlot.R,
                    BuffName = "GalioIdolOfDurand",
                    ExtraDuration = 200,
                });

            #endregion

            #region MissFortune

            Spells.Add(
                new InterruptableSpell
                {
                    ChampionName = "MissFortune",
                    SpellName = "MissFortuneBulletTime",
                    DangerLevel = InterruptableDangerLevel.High,
                    Slot = SpellSlot.R,
                    BuffName = "missfortunebulletsound",
                });

            #endregion

            #region Fiddlesticks

            Spells.Add(
                new InterruptableSpell
                {
                    ChampionName = "FiddleSticks",
                    SpellName = "Drain",
                    DangerLevel = InterruptableDangerLevel.Medium,
                    Slot = SpellSlot.W,
                    BuffName = "Drain",
                });

            Spells.Add(
                new InterruptableSpell
                {
                    ChampionName = "FiddleSticks",
                    SpellName = "Crowstorm",
                    DangerLevel = InterruptableDangerLevel.High,
                    Slot = SpellSlot.R,
                    BuffName = "Crowstorm",
                });

            #endregion

            #region Katarina

            Spells.Add(
                new InterruptableSpell
                {
                    ChampionName = "Katarina",
                    SpellName = "KatarinaR",
                    DangerLevel = InterruptableDangerLevel.High,
                    Slot = SpellSlot.R,
                    BuffName = "katarinarsound"
                });

            #endregion

            #region MasterYi

            Spells.Add(
                new InterruptableSpell
                {
                    ChampionName = "MasterYi",
                    SpellName = "Meditate",
                    BuffName = "Meditate",
                    Slot = SpellSlot.W,
                    DangerLevel = InterruptableDangerLevel.Low,
                });

            #endregion

            #region Xerath

            Spells.Add(
                new InterruptableSpell
                {
                    ChampionName = "Xerath",
                    SpellName = "XerathLocusOfPower2",
                    BuffName = "XerathLocusOfPower2",
                    Slot = SpellSlot.R,
                    DangerLevel = InterruptableDangerLevel.Low,
                });

            #endregion

            #region Janna

            Spells.Add(
                new InterruptableSpell
                {
                    ChampionName = "Janna",
                    SpellName = "ReapTheWhirlwind",
                    BuffName = "ReapTheWhirlwind",
                    Slot = SpellSlot.R,
                    DangerLevel = InterruptableDangerLevel.Low,
                });

            #endregion

            #region Lucian

            Spells.Add(
                new InterruptableSpell
                {
                    ChampionName = "Lucian",
                    SpellName = "LucianR",
                    DangerLevel = InterruptableDangerLevel.High,
                    Slot = SpellSlot.R,
                    BuffName = "LucianR"
                });

            #endregion

            #region TwistedFate

            Spells.Add(
                new InterruptableSpell
                {
                    ChampionName = "TwistedFate",
                    SpellName = "Destiny",
                    DangerLevel = InterruptableDangerLevel.Medium,
                    Slot = SpellSlot.R,
                    BuffName = "Destiny"
                });

            #endregion

            Game.OnGameUpdate += Game_OnGameUpdate;
        }

        public static event OnPossibleToInterruptH OnPossibleToInterrupt;

        private static void FireOnInterruptable(Obj_AI_Base unit, InterruptableSpell spell)
        {
            if (OnPossibleToInterrupt != null)
            {
                OnPossibleToInterrupt(unit, spell);
            }
        }

        private static void Game_OnGameUpdate(EventArgs args)
        {
            foreach (var enemy in ObjectManager.Get<Obj_AI_Hero>().Where(e => e.IsValidTarget()))
            {
                var enemy1 = enemy;
                foreach (var spell in
                    Spells.Where(
                        spell =>
                            (enemy1.LastCastedspell() != null &&
                             String.Equals(
                                 enemy1.LastCastedspell().Name, spell.SpellName,
                                 StringComparison.CurrentCultureIgnoreCase) &&
                             Environment.TickCount - enemy1.LastCastedSpellT() < 350 + spell.ExtraDuration) ||
                            (spell.BuffName != null && enemy1.HasBuff(spell.BuffName, true))))
                {
                    FireOnInterruptable(enemy, spell);
                }
            }
        }

        public static bool IsChannelingImportantSpell(this Obj_AI_Hero unit)
        {
            return
                Spells.Where(spell => spell.ChampionName == unit.ChampionName)
                    .Any(
                        spell =>
                            (unit.LastCastedspell() != null &&
                             String.Equals(
                                 unit.LastCastedspell().Name, spell.SpellName, StringComparison.CurrentCultureIgnoreCase) &&
                             Environment.TickCount - unit.LastCastedSpellT() < 350 + spell.ExtraDuration) ||
                            (spell.BuffName != null && unit.HasBuff(spell.BuffName, true)) ||
                            (ObjectManager.Player.NetworkId == unit.NetworkId &&
                             LastCastedSpell.LastCastPacketSent != null &&
                             LastCastedSpell.LastCastPacketSent.Slot == spell.Slot &&
                             Environment.TickCount - LastCastedSpell.LastCastPacketSent.Tick < 150 + Game.Ping));
        }
    }
}