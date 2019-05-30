using FLServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Matches
{
    public static class Players
    {
        public static Player Create(User user, Character character, FLServer.Models.Stats stats)
        {
            using (FLDBContext ctx = new FLDBContext())
            {
                Player p = new Player()
                {
                    User = user,
                    Character = character,
                    Stats = stats
                };
                ctx.Player.Add(p);
                ctx.SaveChanges();
                return p;
            }
        }


    }
}
