using Shared.Constants;
using Shared.Packets.UserState;
using System;
using System.Runtime.InteropServices;

namespace Shared.Packets
{
    [Serializable]
    public struct ProfilePartInfo
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 100)]
        public string UserName;
        public int Currency;
        public int PremiumCurrency;
        public int Avatar;
        public int Level;
        public int Exp;
        public int AmountOfFriends;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = PacketConstants.maxFriends)]
        public FriendSlotPacket[] Friends;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = PacketConstants.CharacterCount)]
        public bool[] OwnedCharacters;
        public int AmountOfNotifications;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = PacketConstants.MaxIncomingNotifications)]
        public NotificationPacket[] Notifications;

        public ProfilePartInfo(string userName, int currency, int premiumCurrency, int avatar,
            int level, int exp, int amountoffriends, FriendSlotPacket[] friends, bool[] ownedCharacters, int amountOfNotifications,
            NotificationPacket[] notifications) : this()
        {
            UserName = userName;
            Currency = currency;
            PremiumCurrency = premiumCurrency;
            Avatar = avatar;
            Level = level;
            Exp = exp;
            AmountOfFriends = amountoffriends;
            Friends = new FriendSlotPacket[PacketConstants.maxFriends];
            for (int i = 0; i < friends.Length; i++) { Friends[i] = friends[i]; }
            OwnedCharacters = ownedCharacters;
            AmountOfNotifications = amountOfNotifications;
            Notifications = new NotificationPacket[PacketConstants.MaxIncomingNotifications];
            for (int i = 0; i < notifications.Length; i++) { Notifications[i] = notifications[i]; }
        }
    }
}
