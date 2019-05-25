namespace Shared.Packets
{
    public class FriendSlotPacket : Packet
    {
        public string Name { get; set; }
        public string Status { get;set; }
        public int AvatarId { get; set; }

        public FriendSlotPacket(string name, string status, int avatarId)
        {
            Name = name;
            Status = status;
            AvatarId = avatarId;
        }
    }
}