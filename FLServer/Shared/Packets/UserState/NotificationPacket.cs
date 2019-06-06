using Shared.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Packets.UserState
{
    public struct NotificationPacket
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = PacketConstants.DefaultStringSize)]
        public string From;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = PacketConstants.DefaultStringSize)]
        public string Content;
        public int NotificationType; //0 = friend request, 1 = notification

        public NotificationPacket(string from, string content, int type)
        {
            From = from;
            Content = content;
            NotificationType = type;
        }
    }
}
