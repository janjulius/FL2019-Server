using FLServer.Models;
using Microsoft.EntityFrameworkCore;
using Shared.Constants;
using Shared.Models;
using Shared.Packets;
using Shared.Packets.UserState;
using Shared.Ranked;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Users
{
    public static class UserMethods
    {
        public static bool UserExists(string name)
        {
            using (FLDBContext ctx = new FLDBContext())
            {
                IQueryable<User> usr = ctx.User.Where(user => user.Username == name);

                if (usr.Any())
                    return true;
            }
            return false;
        }

        public static void UpdateLastLogin(string username)
        {
            using (FLDBContext ctx = new FLDBContext())
            {
                ctx.User.Where(u => u.Username == username).First().LastOnline = DateTime.UtcNow;
            }
        }

        public static bool AddFriend(User source, User target)
        {
            using (FLDBContext ctx = new FLDBContext())
            {
                if (source != target)
                {

                    int uId = ctx.User.Where(u => u == source).First().UserId;
                    int fId = ctx.User.Where(f => f == target).First().UserId;

                    if (ctx.UserFriend.Where(a => a.UserId == uId && a.FriendId == fId).Any())
                        return false;
                    
                    ctx.UserFriend.Add(
                        new UserFriend()
                        {
                            UserId = uId,
                            FriendId = fId,
                            AddedDate = DateTime.Now
                        });
                    ctx.SaveChanges();
                }
            }
            return true;
        }

        public static void RemoveRequest(User source, User target, int reqType)
        {
            using (FLDBContext ctx = new FLDBContext())
            {
                FriendRequest req = ctx.FriendRequest.Where(r => r.From == source.UserId && r.To == target.UserId && r.RequestType == reqType).FirstOrDefault();
                if(req != null)
                    ctx.FriendRequest.Remove(req);
                ctx.SaveChanges();
            }
        }

        public static bool RemoveFriend(User user, User target)
        {
            using (FLDBContext ctx = new FLDBContext())
            {
                int uId = ctx.User.Where(u => u == user).First().UserId;
                int tId = ctx.User.Where(f => f == target).First().UserId;

                if (ctx.UserFriend.Where(a => a.UserId == uId && a.FriendId == tId).Any())
                {
                    UserFriend todelete = ctx.UserFriend.Where(a => a.UserId == uId && a.FriendId == tId).FirstOrDefault();
                    ctx.UserFriend.Remove(todelete);
                }
                ctx.SaveChanges();
            }
            return true;
        }

        public static FriendRequest[] GetIncomingFriendRequests(User u)
        {
            using (FLDBContext ctx = new FLDBContext())
            {
                return ctx.FriendRequest.Where(req => req.To == u.UserId).AsEnumerable().ToArray();
            }
        }

        public static void CreateFriendRequest(User u, User target)
        {
            using (FLDBContext ctx = new FLDBContext())
            {
                FriendRequest rq = new FriendRequest()
                {
                    From = u.UserId,
                    To = target.UserId,
                    RequestDate = DateTime.Now,
                    RequestType = 0
                };
                ctx.FriendRequest.Add(rq);
                ctx.SaveChanges();
            }
        }

        public static FriendSlotPacket[] GetFriendsAsPacket(string name)
        {
            using (FLDBContext ctx = new FLDBContext())
            {
                User user = ctx.User.Where(u => u.Username == name).First();

                IEnumerable<UserFriend> res = ctx.UserFriend.Where(u => u.UserId == user.UserId).AsEnumerable();
                FriendSlotPacket[] arr = new FriendSlotPacket[res.Count()];
                for (int i = 0; i < arr.Length; i++)
                {
                    arr[i] = new FriendSlotPacket(ctx.User.Where(a => a.UserId == res.ElementAt(i).FriendId).First().Username,
                    ctx.User.Where(a => a.UserId == res.ElementAt(i).FriendId).First().Status,
                    ctx.User.Where(a => a.UserId == res.ElementAt(i).FriendId).First().Avatar,
                    res.ElementAt(i).AddedDate.ToOADate());
                }
                return arr;
            }
        }

        public static void AddPremiumBalance(User target, int v)
        {
            using (FLDBContext ctx = new FLDBContext())
            {
                target.PremiumBalance = target.PremiumBalance + v;
                if (target.PremiumBalance <= 0)
                    target.PremiumBalance = 0;
                ctx.Entry(target).State = EntityState.Modified;
                ctx.SaveChanges();
            }
        }

        public static void AddBalance(User target, int v)
        {
            using (FLDBContext ctx = new FLDBContext())
            {
                target.Balance = target.Balance + v;
                if (target.Balance <= 0)
                    target.Balance = 0;

                ctx.Entry(target).State = EntityState.Modified;
                ctx.SaveChanges();
            }
        }

        public static void SetRights(User target, int v)
        {
            using (FLDBContext ctx = new FLDBContext())
            {
                target.Rights = v;
                ctx.Entry(target).State = EntityState.Modified;
                ctx.SaveChanges();
            }
        }

        public static ProfilePartInfo GetUserAsProfilePartInfoPacket(User u)
        {
            using (FLDBContext ctx = new FLDBContext())
            {
                User user = GetUserByUsername(u.Username);
                FriendSlotPacket[] friends = GetFriendsAsPacket(user.Username);
                NotificationPacket[] notifs = GetNotificationAsPackets(user);
                ProfilePartInfo result = new ProfilePartInfo(user.Username, user.Balance, user.PremiumBalance
                    , user.Avatar, user.Level, user.Exp, user.RankedElo, user.Rank, friends.Length, friends, user.OwnedCharacters.ToArray(), notifs.Length, notifs);

                return result;
            }
        }

        public static NotificationPacket[] GetNotificationAsPackets(User user)
        {
            using (FLDBContext ctx = new FLDBContext())
            {
                IEnumerable<FriendRequest> res = ctx.FriendRequest.Where(u => u.To == user.UserId).AsEnumerable();
                NotificationPacket[] arr = new NotificationPacket[res.Count()];
                for (int i = 0; i < arr.Length; i++)
                {
                    arr[i] = new NotificationPacket(ctx.User.Where(usr => usr.UserId == res.ElementAt(i).From).FirstOrDefault().Username,
                        res.ElementAt(i).Content, res.ElementAt(i).RequestType);
                }
                return arr;
            }
        }

        public static bool CreateRequest(User source, User target, int type)
        {
            using (FLDBContext ctx = new FLDBContext())
            {
                if (source != target)
                {
                    FriendRequest req = new FriendRequest()
                    {
                        From = source.UserId,
                        To = target.UserId,
                        RequestDate = DateTime.Now,
                        RequestType = type
                    };

                    if (!ctx.FriendRequest.Contains(req))
                        return false;
                    else
                        ctx.FriendRequest.Add(req);

                    ctx.SaveChanges();
                }
            }
            return true;
        }

        public static bool CreateNotification(User source, User target, int type, string content)
        {
            using (FLDBContext ctx = new FLDBContext())
            {
                    FriendRequest notification = new FriendRequest()
                    {
                        From = source.UserId,
                        To = target.UserId,
                        RequestDate = DateTime.Now,
                        RequestType = type,
                        Content = content
                    };

                    if (ctx.FriendRequest.Contains(notification))
                        return false;
                    else
                        ctx.FriendRequest.Add(notification);

                    ctx.SaveChanges();
                
            }
            return true;
        }

        public static User[] BroadcastAll(User user, string msg)
        {
            using (FLDBContext ctx = new FLDBContext())
            {
                IQueryable<User> filteredUsers = ctx.User.Where(u => u.Rights >= 0);
                foreach (var player in filteredUsers)
                {
                    CreateNotification(user, player, 1, msg);
                }
                return filteredUsers.ToArray();
            }
        }

        public static void TruncateNotifications()
        {
            using (FLDBContext ctx = new FLDBContext())
            {
                ctx.FriendRequest.RemoveRange(ctx.FriendRequest);
                ctx.SaveChanges();
            }
        }

        public static bool CreateNotification(User source, User[] targets, int type, string content)
        {
            using (FLDBContext ctx = new FLDBContext())
            {
                foreach (User target in targets)
                {
                    FriendRequest notification = new FriendRequest()
                    {
                        From = source.UserId,
                        To = target.UserId,
                        RequestDate = DateTime.Now,
                        RequestType = type,
                        Content = content
                    };

                    if (!ctx.FriendRequest.Contains(notification))
                        return false;
                    else
                        ctx.FriendRequest.Add(notification);

                    ctx.SaveChanges();
                }
            }
            
            return true;
        }

        public static void SetElo(User user, int v)
        {
            using (FLDBContext ctx = new FLDBContext())
            {
                user.RankedElo = v;
                user.Rank = Ranked.RankCalculator.GetNewRank(user.RankedElo, user.Rank);
                ctx.Entry(user).State = EntityState.Modified;
                ctx.SaveChanges();
            }
        }

        /// <summary>
        /// gets the user by name
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public static User GetUserByUsername(string username)
        {
            using (FLDBContext ctx = new FLDBContext())
            {
                User u = null;
                try
                {
                    u = ctx.User.Where(n => n.Username == username).First();
                }
                catch
                {
                    return null;
                }
                return u;
            }
        }

        public static User GetUserByUniqueIdentifier(string uid)
        {
            using (FLDBContext ctx = new FLDBContext())
            {
                return ctx.User.Where(u => u.UniqueIdentifier == uid).First();
            }
        }

        public static User GetUserById(int id)
        {
            using (FLDBContext ctx = new FLDBContext())
            {
                return ctx.User.Where(u => u.UserId == id).FirstOrDefault();
            }
        }

        public static void ResetOwnedCharacters(User user)
        {
            using (FLDBContext ctx = new FLDBContext())
            {
                char[] arr = new char[Constants.PacketConstants.CharacterCount];
                for (int a = 0; a < arr.Length; a++)
                    arr[a] = '0';
                user.OwnedCharactersString = new string(arr);
                ctx.Entry(user).State = EntityState.Modified;
                ctx.SaveChanges();
            }
        }

        public static void CreateNewUser(string username, string password)
        {
            using (FLDBContext ctx = new FLDBContext())
            {
                try
                {
                    ctx.User.Add(new User()
                    {
                        Username = username,
                        Password = Security.Security.GetHashString(password),
                        Email = $"{username}{password}@thwamp.com",
                        UniqueIdentifier = Guid.NewGuid().ToString(),
                        CreationDate = DateTime.UtcNow,
                        Level = 0,
                        NormalElo = 1250,
                        RankedElo = 1250,
                        Rank = "IV",
                        Verified = true,
                        Rights = 0,
                        OwnedCharacters = new bool[Constants.PacketConstants.CharacterCount].ToList()
                    });
                    ctx.SaveChanges();
                }
                catch (Exception e)
                {

                }
            }
        }

        public static void AddExp(string username, int exp)
        {
            using (FLDBContext ctx = new FLDBContext())
            {
                User u = GetUserByUsername(username);
                ctx.Entry(u).State = EntityState.Modified;
                Levels.ProgressCalculator calc = new Levels.ProgressCalculator();
                u.Exp = u.Exp + exp;
                u.Level = calc.GetLevelByExperience(u.Exp);
                ctx.SaveChanges();
            }
        }

        public static void SetExp(string username, int exp)
        {
            using (FLDBContext ctx = new FLDBContext())
            {
                User u = GetUserByUsername(username);
                Levels.ProgressCalculator calc = new Levels.ProgressCalculator();
                u.Exp = exp;
                u.Level = calc.GetLevelByExperience(u.Exp);
                ctx.Entry(u).State = EntityState.Modified;
                ctx.SaveChanges();
            }
        }

        public static void SetLevel(string username, int lvl)
        {
            using (FLDBContext ctx = new FLDBContext())
            {
                User u = GetUserByUsername(username);
                ctx.Entry(u).State = EntityState.Modified;
                Levels.ProgressCalculator calc = new Levels.ProgressCalculator();
                u.Level = lvl;
                u.Exp = calc.getExperienceByLevel(u.Level);
                ctx.SaveChanges();
            }
        }

        public static int GetLevel(string username)
        {
            using (FLDBContext ctx = new FLDBContext())
            {
                return GetUserByUsername(username).Level;
            }
        }

        public static int GetExp(string username)
        {
            using (FLDBContext ctx = new FLDBContext())
            {
                return GetUserByUsername(username).Exp;
            }
        }

        public static ProfileAccountInfo GetProfileAccountInfoPacket(string id, User sender)
        {
            using (FLDBContext ctx = new FLDBContext())
            {
                User u = null;
                u = GetUserByUsername(id);
                if (u != null)
                    return new ProfileAccountInfo(u.Username,
                        u.Avatar,
                        u.Level,
                        u.Exp,
                        u.RankedElo,
                        u.Rank,
                        u.LastOnline.ToOADate(),
                        String.Empty);
                else
                {
                    u = GetUserByUsername(sender.Username);
                    return new ProfileAccountInfo(u.Username, u.Avatar, u.Level, u.Exp, u.RankedElo, u.Rank, u.LastOnline.ToOADate(), string.Empty);
                }
            }
        }

        public static void SetAvatar(string username, int id)
        {
            using (FLDBContext ctx = new FLDBContext())
            {
                User u = GetUserByUsername(username);
                u.Avatar = id;
                ctx.Entry(u).State = EntityState.Modified;
                ctx.SaveChanges();
            }
        }

        public static void SetStatusText(string name, string text)
        {
            using (FLDBContext ctx = new FLDBContext())
            {
                User u = GetUserByUsername(name);
                u.Status = text;
                ctx.Entry(u).State = EntityState.Modified;
                ctx.SaveChanges();
            }
        }

        public static void AddMatch(User u, Match m)
        {
            using (FLDBContext ctx = new FLDBContext())
            {
                u.Matches.Add(m);
                ctx.Entry(u).State = EntityState.Modified;

                ctx.SaveChanges();
            }
        }

        public static IEnumerable<Match> GetMatches(User u)
        {
            using (FLDBContext ctx = new FLDBContext())
            {
                return u.Matches.AsEnumerable();
            }
        }

        public static void SetCharacterOwnedState(User u, int referenceId, bool v)
        {
            using (FLDBContext ctx = new FLDBContext())
            {
                var set = new List<bool>(u.OwnedCharacters);
                set[referenceId] = v;
                u.OwnedCharacters = set;
                ctx.Entry(u).State = EntityState.Modified;
                ctx.SaveChanges();
            }
        }

        public static void SaveMessageToDatabase(int senderId, int receiverId, string messageText, double timeStamp)
        {
            using (FLDBContext ctx = new FLDBContext())
            {
                try
                {
                    ctx.Message.Add(new Models.Message
                    {
                        SenderId = senderId,
                        ReceiverId = receiverId,
                        MessageText = messageText,
                        TimeStamp = DateTime.FromOADate(timeStamp)
                    });
                    ctx.SaveChanges();
                }
                catch (Exception e)
                {

                }
            }
        }

        public static Packets.Message[] GetLatestMessages(User user, User target)
        {
            Packets.Message[] arr = new Packets.Message[PacketConstants.MaxMessages];
            using (FLDBContext ctx = new FLDBContext())
            {
                IEnumerable<Models.Message> collection = ctx.Message.Where(m => (m.SenderId == user.UserId && m.ReceiverId == target.UserId)
                    || (m.SenderId == target.UserId && m.ReceiverId== user.UserId)).AsEnumerable();

                    if (collection.Any())
                    {
                        int maxMessages = collection.Count() <= PacketConstants.MaxMessages ? collection.Count() : PacketConstants.MaxMessages;
                        var data = collection.Reverse().Take(maxMessages).Reverse().ToArray();
                        for (int i = 0; i < data.Count(); i++)
                        {
                            arr[i] = new Packets.Message(GetUserById(data[i].ReceiverId).Username, data[i].MessageText, data[i].TimeStamp.ToOADate());
                        }
                    }
                
                return arr;
            }
        }

        public static int GetRankEloOfUser(string username)
        {
            using (FLDBContext ctx = new FLDBContext())
            {
                return GetUserByUsername(username).RankedElo;
            }
        }

        public static string GetRankOfUser(string username)
        {
            using (FLDBContext ctx = new FLDBContext())
            {
                return GetUserByUsername(username).Rank;
            }
        }

        //public static void SetRankEloAndRankOfUserAfterMatch(string username, double currentELO, double opponentELO, bool win)
        //{
        //    RankCalculator rc = new RankCalculator();
        //    using (FLDBContext ctx = new FLDBContext())
        //    {
        //       User u = GetUserByUsername(username);
        //       u.RankedElo = Convert.ToInt32(rc.GetNewELO(currentELO, opponentELO, win));
        //       u.Rank = rc.GetCurrentAbsoluteRank(rc.newElo);
        //       ctx.Entry(u).State = EntityState.Modified;
        //       ctx.SaveChanges();
        //    }
        //}
     
    }
}