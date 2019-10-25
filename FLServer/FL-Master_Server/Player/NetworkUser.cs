using LiteNetLib;
using System;
using System.Collections.Generic;
using System.Text;
using FLServer.Models;
using FL_Master_Server.Net;
using Shared.Users;
using Shared.Packets.Generics;
using Shared.Packets.UserState;

namespace FL_Master_Server.Player
{
    public class NetworkUser
    {
        public NetPeer Peer { get; set; }
        public User User { get; set; }
        
        public NetworkUser(NetPeer peer, User user)
        {
            Peer = peer;
            User = user;
        }

        /// <summary>
        /// TODO correct packets PACKETS
        /// </summary>
        /// <param name="inter"></param>
        public void RefreshInterface(RefreshableInterface inter)
        {
            switch (inter)
            {
                case RefreshableInterface.Client:
                    NetEvent.SendNetworkEvent(this, DeliveryMethod.ReliableOrdered, (ushort)RefreshableInterface.Client, UserMethods.GetUserAsProfilePartInfoPacket(User));
                    break;
                case RefreshableInterface.FriendsList:
                    NetEvent.SendNetworkEvent(this, DeliveryMethod.ReliableOrdered, (ushort)RefreshableInterface.FriendsList, UserMethods.GetUserAsProfilePartInfoPacket(User));
                    break;

                case RefreshableInterface.Notifications:
                    NetEvent.SendNetworkEvent(this, DeliveryMethod.ReliableOrdered, (ushort)RefreshableInterface.FriendsList, new ArrayPacket<NotificationPacket>(UserMethods.GetNotificationAsPackets(User)));
                    break;

                case RefreshableInterface.ProfilePart:

                    break;
            }
        }

        public void SendConsoleMessage(string msg)
        {
            NetEvent.SendNetworkEventString(this, DeliveryMethod.ReliableOrdered, 20000, msg);
        }
    }
}
