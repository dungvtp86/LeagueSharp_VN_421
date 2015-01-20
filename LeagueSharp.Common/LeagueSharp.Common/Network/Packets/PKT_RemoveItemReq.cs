using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp.Network.Serialization;

namespace LeagueSharp.Network.Packets
{
    [PacketAttribute(0x72, typeof(Byte))]
    public class PKT_RemoveItemReq : Packet
    {
        [SerializeAttribute(0, 3, new uint[] { 0x6501D62E, 2, 1, 0x87CFCD92, 0xFE0A65A2, 0, unchecked((uint)-1), 0x21BD274B })]
        public byte Slot { get; set; }

        [SerializeAttribute(3, 1)]
        public bool GrantGold { get; set; }
    }
}
