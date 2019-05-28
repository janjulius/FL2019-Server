using FLServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Matches
{
    public static class Teams
    {

        public static Team Create(List<Player> players)
        {
            using (FLDBContext ctx = new FLDBContext())
            {
                Team team = new Team()
                {
                    Players = players
                };
                ctx.Team.Add(team); ctx.SaveChanges();
                return team;
            }
        }

        public static Team Create(Player players)
        {
            using (FLDBContext ctx = new FLDBContext())
            {
                Team team = new Team()
                {
                    Players = new List<Player>() { players }
                };
                ctx.Team.Add(team); ctx.SaveChanges();
                return team;
            }
        }
    }
}
