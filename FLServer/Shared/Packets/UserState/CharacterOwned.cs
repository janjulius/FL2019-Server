using Shared.Constants;
using System;
using System.Runtime.InteropServices;

namespace Shared.Packets.UserState
{
    [Serializable]
    public struct CharacterOwned
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = PacketConstants.DefaultStringSize)]
        public string OwnerUsername;
        public int Id;
        public bool State;
        public bool PremiumPayment;

        public CharacterOwned(string ownerUsername, int id, bool state, bool premiumPayment)
        {
            OwnerUsername = ownerUsername;
            Id = id;
            State = state;
            PremiumPayment = premiumPayment;
        }
    }
}
