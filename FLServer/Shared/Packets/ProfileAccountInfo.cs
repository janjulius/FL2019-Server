using Shared.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Packets
{
    public struct ProfileAccountInfo
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = PacketConstants.DefaultStringSize)]
        public string Username;
        public int Avatar;
        public int Level;
        public int Exp;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = PacketConstants.DefaultStringSize)]
        public string LastOnline;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = PacketConstants.DefaultStringSize)]
        public string ErrorMessage;

        public ProfileAccountInfo(string username, int avatar, int level, int exp, string lastOnline, string errorMessage)
        {
            Username = username;
            Avatar = avatar;
            Level = level;
            Exp = exp;
            LastOnline = lastOnline;
            ErrorMessage = errorMessage;
        }
    }
}
