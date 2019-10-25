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
    public static class Util
    {
        public static NetworkUser GetNetworkUser(NetPeer fpeer)
        {
            NetworkUser result = MasterServer.Instance.NetworkUsers.Where(nu => nu.Peer == fpeer).FirstOrDefault();
            if (result != null)
                return result;

            return null;
        }

        public static NetworkUser GetNetworkUser(User user)
        {
            NetworkUser result = MasterServer.Instance.NetworkUsers.Where(nu => nu.User.UniqueIdentifier == user.UniqueIdentifier
                                                    && nu.Peer != null).FirstOrDefault();
            if (result != null)
                return result;

            return null;
        }

        public static NetworkUser GetNetworkUser(string name)
        {
            NetworkUser result = MasterServer.Instance.NetworkUsers.Where(usr => usr.User.Username == name).FirstOrDefault();

            if (result != null)
                return result;

            return null;
        }

        public static bool IsOnline(User user)
        {
            NetworkUser result = MasterServer.Instance.NetworkUsers.Where(usr => usr.User.Username == user.Username).FirstOrDefault();

            return result != null;
        }

        public static bool IsOnline(string username)
        {
            NetworkUser result = MasterServer.Instance.NetworkUsers.Where(usr => usr.User.Username == username).FirstOrDefault();

            return result != null;
        }
    }
}
