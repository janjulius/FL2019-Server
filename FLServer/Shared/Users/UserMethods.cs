using FLServer.Models;
using Microsoft.EntityFrameworkCore;
using Shared.Packets;
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
                int uId = ctx.User.Where(u => u == source).First().UserId;
                int fId = ctx.User.Where(f => f == target).First().UserId;

                if (ctx.UserFriend.Where(a => a.UserId == uId && a.FriendId == fId).Any())
                    return false;

                ctx.UserFriend.Add(
                    new UserFriend()
                    {
                        UserId = uId,
                        FriendId = fId
                    });
                ctx.SaveChanges();
            }
            return true;
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

        public static string[] GetFriends(string name)
        {
            using (FLDBContext ctx = new FLDBContext())
            {
                User user = ctx.User.Where(u => u.Username == name).First();

                IEnumerable<UserFriend> res = ctx.UserFriend.Where(u => u.UserId == user.UserId).AsEnumerable();
                string[] arr = new string[res.Count()];
                for(int i = 0; i < arr.Length; i++)
                {
                    arr[i] = ctx.User.Where(a => a.UserId == res.ElementAt(i).FriendId).First().Username;
                }
                return arr;
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
                    ctx.User.Where(a => a.UserId == res.ElementAt(i).FriendId).First().Avatar);
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

        public static ProfilePartInfo GetUserAsProfilePartInfoPacket(string name)
        {
            using (FLDBContext ctx = new FLDBContext())
            {
                User u = GetUserByUsername(name);
                FriendSlotPacket[] friends = GetFriendsAsPacket(name);
                ProfilePartInfo result = new ProfilePartInfo(u.Username, u.Balance, u.PremiumBalance
                    , u.Avatar, u.Level, u.Exp, friends.Length, friends, u.OwnedCharacters.ToArray());

                return result;
            }
        }

        public static ProfilePartInfo GetUserAsProfilePartInfoPacket(User user)
        {
            using (FLDBContext ctx = new FLDBContext())
            {
                FriendSlotPacket[] friends = GetFriendsAsPacket(user.Username);
                ProfilePartInfo result = new ProfilePartInfo(user.Username, user.Balance, user.PremiumBalance
                    , user.Avatar, user.Level, user.Exp, friends.Length, friends, user.OwnedCharacters.ToArray());

                return result;
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
                catch {
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

        public static void ResetOwnedCharacters(User user)
        {
            using (FLDBContext ctx = new FLDBContext())
            {
                char[] arr = new char[Constants.PacketConstants.CharacterCount];
                for (int a = 0; a < arr.Length; a++)
                    arr[a] = '0';
                user.OwnedCharactersString = arr.ToString();
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
                        Verified = true,
                        Rights = 0
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
                ctx.Entry(u).State = EntityState.Modified;
                Levels.ProgressCalculator calc = new Levels.ProgressCalculator();
                u.Exp = exp;
                u.Level = calc.GetLevelByExperience(u.Exp);
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

        public static ProfileAccountInfo GetProfileAccountInfoPacket(string id)
        {
            using (FLDBContext ctx = new FLDBContext())
            {
                User u = GetUserByUsername(id);
                    return new ProfileAccountInfo(u.Username,
                        u.Avatar,
                        u.Level,
                        u.Exp,
                        u.LastOnline.ToString(),
                        String.Empty);
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
    }
}
