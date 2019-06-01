using LiteNetLib;
using System;
using System.Collections.Generic;
using System.Text;
using FLServer.Models;

namespace FL_Master_Server.Player
{
    class NetworkUser
    {
        public NetPeer Peer { get; set; }
        public User User { get; set; }
        
        public NetworkUser(NetPeer peer, User user) { }
    }
}
