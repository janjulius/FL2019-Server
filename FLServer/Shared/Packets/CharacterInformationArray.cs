using Shared.Packets.Generics;
using System.Runtime.InteropServices;

namespace Shared.Packets
{
    public struct CharacterInformationArray
    {
        int CharCount;
        //[MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(ArrayMarshaler<CharacterInformation>))]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Constants.PacketConstants.CharacterCount)]
        public CharacterInformation[] chars;

        public CharacterInformationArray(int charCount, CharacterInformation[] chars)
        {
            CharCount = charCount;
            this.chars = chars;
        }
    }
}
