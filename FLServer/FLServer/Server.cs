using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FLServer
{
    class Server
    {
        public Server()
        {

        }

        public void Run() { }

        internal ProgramResult AddNewUser(string v)
        {
            using (var ctx = new PlayerContext())
            {
                var usr = ctx.Usertest.Where(user => user.Username == v);

                if (usr.Any())
                    return new ProgramResult(false, "User exists");

                ctx.Usertest.Add(new Usertest()
                {
                    Username = v,
                    Level = 99
                });
                ctx.SaveChanges();
                return new ProgramResult(true, "User added");
            }
            return new ProgramResult(false);
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
