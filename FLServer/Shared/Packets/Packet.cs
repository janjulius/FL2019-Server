using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Packets
{
    public abstract class Packet
    {
        public Packet() { }
        
        public int GetPropertySize()
        {
            return GetType().GetProperties().Length;
        }
    }
}
