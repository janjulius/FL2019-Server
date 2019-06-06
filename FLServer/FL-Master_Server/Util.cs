using FL_Master_Server.Player;
using FLServer.Models;
using LiteNetLib;
using LiteNetLib.Utils;
using Shared.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FL_Master_Server
{
    internal sealed class Util
    {
        public NetworkUser GetNetworkUserFromPeer(NetPeer fpeer)
        {
            NetworkUser result = MasterServer.Instance.NetworkUsers.Where(nu => nu.Peer == fpeer).FirstOrDefault();
            if (result != null)
                return result;

            return null;
        }

        public NetworkUser GetNetworkUserFromUser(User user)
        {
            NetworkUser result = MasterServer.Instance.NetworkUsers.Where(nu => nu.User.UniqueIdentifier == user.UniqueIdentifier
                                                    && nu.Peer != null).FirstOrDefault();
            if (result != null)
                return result;

            return null;
        }

        public NetworkUser GetNetworkUserFromUsername(string name)
        {
            NetworkUser result = MasterServer.Instance.NetworkUsers.Where(usr => usr.User.Username == name).FirstOrDefault();

            if (result != null)
                return result;

            return null;
        }
    }
}
