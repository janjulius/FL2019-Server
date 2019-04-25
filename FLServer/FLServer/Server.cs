
using FLServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace FLServer
{
    class Server
    {
        public Server()
        {

        }

        public void Run() { }

        internal ProgramResult AddNewUser(string name, string password)
        {
            using (var ctx = new FLDBContext())
            {
                var usr = ctx.User.Where(user => user.Username == name);
            
                if (usr.Any())
                    return new ProgramResult(false, "User exists");
            
                ctx.User.Add(new User()
                {
                    Username = name,
                    Level = 99,
                    Password = GetHashString(password),
                    CreationDate = DateTime.UtcNow
                });
                ctx.SaveChanges();
            }
            return new ProgramResult(true, "User added");
        }

        internal ProgramResult AddFriend(string name, string toAdd)
        {
            using (var ctx = new FLDBContext())
            {
                var uId = ctx.User.Where(u => u.Username == name).First().UserId;
                var fId = ctx.User.Where(f => f.Username == toAdd).First().UserId;

                if (ctx.UserFriend.Where(a => a.UserId == uId && a.FriendId == fId).Any())
                    return new ProgramResult(false, "Friend already added");

                ctx.UserFriend.Add(
                    new UserFriend() {
                        UserId = uId, FriendId = fId
                    });
                ctx.SaveChanges();
            }
            return new ProgramResult(true, $"User {name} added {toAdd}");
        }
        
        internal ProgramResult GetFriends(string name)
        {
            using (var ctx = new FLDBContext())
            {
                var user = ctx.User.Where(u => u.Username == name).First();

                var res = ctx.UserFriend.Where(u => u.UserId == user.UserId).AsEnumerable();
                StringBuilder sb = new StringBuilder();
                
                foreach(var r in res)
                {
                    sb.Append(
                    ctx.User.Where(a => a.UserId == r.FriendId).First().Username + ",");
                }
                return new ProgramResult(true, sb.ToString());
            }
        }

        public ProgramResult GetUserLvl(int level)
        {
            using (var ctx = new FLDBContext())
            {
                var result = ctx.User.Where(user => user.Level == level && user.Username.StartsWith('J'));

                foreach(var r in result)
                {
                    Console.WriteLine($"{r.Username}");
                    
                }
                return new ProgramResult(true);
            }
        }

        public static byte[] GetHash(string inputString)
        {
            HashAlgorithm algorithm = SHA256.Create();
            return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
        }

        public static string GetHashString(string inputString)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in GetHash(inputString))
                sb.Append(b.ToString("X2"));

            return sb.ToString();
        }

        internal sealed class ProgramResult
        {
            bool Result { get; set; }
            string Info { get; set; }
            public ProgramResult(bool r)
            {
                Result = r;
            }
            public ProgramResult(string info)
            {
                Info = info;
            }
            public ProgramResult(bool r, string info)
            {
                Result = r;
                Info = info;
            }
            public override string ToString()
            {
                return Result ? $@"Success: {Info}" : $@"Something went wrong: {Info}";
            }
        }
    }
}
