using LiteNetLib.Utils;
using Shared.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Extensions
{
    public static class NetDataWriterExtension
    {
        public static void PutFriendSlotPacket(this NetDataWriter ndw, FriendSlotPacket fsp)
        {
            ndw.Put(fsp.Name); ndw.Put(fsp.Status); ndw.Put(fsp.AvatarId);
        }

        public static void PutFriendSlotPackets(this NetDataWriter ndw, FriendSlotPacket[] fsp)
        {
            ndw.PutArray(fsp.Select(friend => friend.Name).ToArray());
            ndw.PutArray(fsp.Select(friend => friend.Status).ToArray());
            ndw.PutArray(fsp.Select(friend => friend.AvatarId).ToArray());
        }

    }
}