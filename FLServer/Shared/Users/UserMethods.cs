﻿using FLServer.Models;
using Microsoft.EntityFrameworkCore;
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

        public static void CreateNewUser(string username, string password)
        {
            using (FLDBContext ctx = new FLDBContext())
            {
                ctx.User.Add(new User()
                {
                    Username = username,
                    Password = Security.Security.GetHashString(password),
                    Email = "admin@thwamp.com",
                    UniqueIdentifier = Guid.NewGuid().ToString(),
                    CreationDate = DateTime.UtcNow,
                    Level = 0,
                    NormalElo = 1250,
                    RankedElo = 1250,
                    Verified = true
                });
                ctx.SaveChanges();
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

    }
}
