using Shared.Constants;
using System.Runtime.InteropServices;

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
