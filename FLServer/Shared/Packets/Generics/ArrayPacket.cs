using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Packets.Generics
{
    public struct ArrayPacket<T> where T : struct
    {
        T[] Array;

        public ArrayPacket(T[] arr)
        {
            Array = arr;
        }
    }
}
