using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Extensions
{
    public static class StructExtension
    {
        public static byte[] ToByteArray<T>(this T structure) where T : struct
        {
            var bufferSize = Marshal.SizeOf(structure);
            var byteArray = new byte[bufferSize];

            IntPtr handle = Marshal.AllocHGlobal(bufferSize);
            Marshal.StructureToPtr(structure, handle, true);
            Marshal.Copy(handle, byteArray, 0, bufferSize);

            return byteArray;
        }

        public static T ToStructure<T>(this byte[] byteArray) where T : struct
        {
            var packet = new T();
            var bufferSize = Marshal.SizeOf(packet);
            IntPtr handle = Marshal.AllocHGlobal(bufferSize);
            Marshal.Copy(byteArray, 0, handle, bufferSize);

            return Marshal.PtrToStructure<T>(handle);
        }
    }
}
