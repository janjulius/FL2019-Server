using FL_Master_Server.Player;
using FLServer.Models;
using LiteNetLib;
using LiteNetLib.Utils;
using Shared.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace FL_Master_Server.Net
{
    public static class NetEvent
    {
        public static void SendNetworkEventString(NetworkUser target, DeliveryMethod dm, ushort msgid, string msg)
        {
            NetDataWriter writer = new NetDataWriter();
            writer.Put(msgid);
            writer.Put(msg);
            target.Peer.Send(writer, dm);
        }

        public static void SendNetworkEvent<T>(NetworkUser target, DeliveryMethod dm, ushort msgid, T packet) where T : struct
        {
            NetDataWriter writer = new NetDataWriter();
            writer.Put(msgid);
            writer.PutPacketStruct(packet);
            target.Peer.Send(writer, dm);
        }

        public static void SendNetworkEvent<T>(User target, DeliveryMethod dm, ushort msgid, T packet) where T : struct
        {
            NetworkUser user = Util.GetNetworkUserFromUser(target);
            if (user != null)
                SendNetworkEvent(user, dm, msgid, packet);
        }

        public static void SendNetworkEvent<T>(NetPeer target, DeliveryMethod dm, ushort msgid, T packet) where T : struct
        {
            NetworkUser user = Util.GetNetworkUserFromPeer(target);
            if (user != null)
                SendNetworkEvent(user, dm, msgid, packet);
        }
    }
}
