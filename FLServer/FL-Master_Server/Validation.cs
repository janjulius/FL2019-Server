using FL_Master_Server.Player;
using LiteNetLib;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public bool ValidateSender(NetPeer sender, string name)
        {
            NetworkUser u = MasterServer.Instance.NetworkUsers.Where(a => a.Peer == sender).FirstOrDefault();
            if (u == null)
                return false;
            return u.User.Username == name;
        }
    }
}
