using System;
using System.Runtime.InteropServices;

namespace Shared.Packets
{
    [Serializable]
    public struct FriendSlotPacket
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 100)]
        public string Name;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 100)]
        public string Status;
        public int AvatarId;

        public FriendSlotPacket(string name, string status, int avatarId)
        {
            Name = name;
            Status = status;
            AvatarId = avatarId;
        }
    }
}