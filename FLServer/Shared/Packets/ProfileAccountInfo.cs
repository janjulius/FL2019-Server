using Shared.Constants;
using System.Runtime.InteropServices;

namespace Shared.Packets
{
    public struct ProfileAccountInfo
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = PacketConstants.DefaultStringSize)]
        public string Username;
        public int Avatar;
        public int Level;
        public int Exp;
        public int RankedElo;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = PacketConstants.DefaultStringSize)]
        public string Rank;
        public double LastOnline;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = PacketConstants.DefaultStringSize)]
        public string ErrorMessage;

        public ProfileAccountInfo(string username, int avatar, int level, int exp, int rankedelo, string rank, double lastOnline, string errorMessage)
        {
            Username = username;
            Avatar = avatar;
            Level = level;
            Exp = exp;
            RankedElo = rankedelo;
            Rank = rank;
            LastOnline = lastOnline;
            ErrorMessage = errorMessage;
        }
    }
}