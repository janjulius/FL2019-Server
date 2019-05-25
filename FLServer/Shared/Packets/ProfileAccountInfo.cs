using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Packets
{
    public class ProfileAccountInfo : Packet
    {
        public string Username { get; internal set; } = "Not found";
        public int Avatar { get; internal set; }
        public int Level { get; internal set; } = 0;
        public int Exp { get; internal set; } = 0;
        public string LastOnline { get; internal set; }
        public string ErrorMessage { get; internal set; }

        public ProfileAccountInfo()
        {
        }

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
