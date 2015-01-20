﻿#region LICENSE

/*
 Copyright 2014 - 2014 LeagueSharp
 CustomEvents.cs is part of LeagueSharp.Common.
 
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

#endregion

namespace LeagueSharp.Common
{
    public static class CustomEvents
    {
        public class Game
        {
            public delegate void OnGameEnded(EventArgs args);

            public delegate void OnGameLoaded(EventArgs args);

            static Game()
            {
                LeagueSharp.Game.OnGameProcessPacket += Game_OnGameProcessPacket;

                if (LeagueSharp.Game.Mode == GameMode.Running)
                {
                    Utility.DelayAction.Add(0, () => Game_OnGameStart(new EventArgs())); //Otherwise the .ctor didn't return yet and no callback will occur
                }
                else
                {
                    LeagueSharp.Game.OnGameStart += Game_OnGameStart;
                }
            }

            /// <summary>
            /// OnGameLoad is getting called when you get ingame (doesn't matter if started or restarted while game is already running) and when reloading an assembly
            /// </summary>
            public static event OnGameLoaded OnGameLoad;

            /// <summary>
            /// OnGameEnd is getting called when the game ends. Same as Game.OnGameEnd but this one works :^).
            /// </summary>
            public static event OnGameEnded OnGameEnd;
            
            private static void Game_OnGameStart(EventArgs args)
            {
                if (OnGameLoad != null)
                {
                    OnGameLoad(new EventArgs());
                }
            }

            private static void Game_OnGameProcessPacket(GamePacketEventArgs args)
            {
                //Game end packet TODO: Update this
                /*if (args.PacketData[0] == Packet.S2C.GameEnd.Header)
                {
                    if (OnGameEnd != null)
                    {
                        OnGameEnd(new EventArgs());
                    }
                }*/
            }
        }

        public class Unit
        {
            public delegate void OnDashed(Obj_AI_Base sender, Dash.DashItem args);

            public delegate void OnLeveledUp(Obj_AI_Base sender, OnLevelUpEventArgs args);

            public delegate void OnLeveledUpSpell(Obj_AI_Base sender, OnLevelUpSpellEventArgs args);


            static Unit()
            {
                LeagueSharp.Game.OnGameProcessPacket += PacketHandler;

                //Initializes ondash class:
                ObjectManager.Player.IsDashing();
            }

            /// <summary>
            /// OnLevelUpSpell gets called after you leveled a spell
            /// </summary>
            public static event OnLeveledUpSpell OnLevelUpSpell;

            private static void PacketHandler(GamePacketEventArgs args)
            {
                return;//BROKEN on 4.21
                /*if (OnLevelUpSpell != null)
                {
                    if (args.PacketData[0] == 0x15)
                    {
                        var unit =
                            ObjectManager.GetUnitByNetworkId<Obj_AI_Base>(BitConverter.ToInt32(args.PacketData, 1));
                        int id = args.PacketData[5];
                        int lvl = args.PacketData[6];
                        int pts = args.PacketData[7];
                        OnLevelUpSpell(
                            unit, new OnLevelUpSpellEventArgs { SpellId = id, SpellLevel = lvl, Remainingpoints = pts });
                    }
                }
                if (OnLevelUp != null && args.PacketData[0] == Packet.S2C.LevelUp.Header)
                {
                    var dp = Packet.S2C.LevelUp.Decoded(args.PacketData);
                    OnLevelUp(dp.Unit, new OnLevelUpEventArgs { NewLevel = dp.Level, RemainingPoints = dp.PointsLeft });
                }*/
            }

            /// <summary>
            /// Gets called when a unit gets a level up
            /// </summary>
            public static event OnLeveledUp OnLevelUp;

            /// <summary>
            /// OnDash is getting called when a unit dashes.
            /// </summary>
            public static event OnDashed OnDash;

            public static void TriggerOnDash(Obj_AI_Base sender, Dash.DashItem args)
            {
                var dashHandler = OnDash;
                if (dashHandler != null)
                {
                   dashHandler(sender, args);
                }
            }

            public class OnLevelUpEventArgs : EventArgs
            {
                public int NewLevel;
                public int RemainingPoints;
            }

            public class OnLevelUpSpellEventArgs : EventArgs
            {
                public int Remainingpoints;
                public int SpellId;
                public int SpellLevel;

                internal OnLevelUpSpellEventArgs() { }
            }
        }
    }
}
