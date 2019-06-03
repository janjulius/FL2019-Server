using FL_Master_Server.Player;
using LiteNetLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace FL_Master_Server
{
    internal sealed class Validation
    {
        public bool ValidateNetworkUser(NetPeer frompeer, NetworkUser user)
        {
            if (Constants.ValidateNetworkUsers)
                return (frompeer == user.Peer) && user.User != null;
            return true;
        }
    }
}
