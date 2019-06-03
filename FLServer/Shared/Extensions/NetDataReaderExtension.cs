using LiteNetLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Extensions
{
    public static class NetDataReaderExtension
    {
        public static T GetPacketStruct<T>(this NetPacketReader npr) where T : struct
        {
            return npr.GetRemainingBytes().ToStructure<T>();
        }
    }
}
