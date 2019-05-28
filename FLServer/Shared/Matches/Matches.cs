using FLServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Matches
{
    public static class Matches
    {
        public static Match SaveMatch(List<FLServer.Models.Team> teams, Map map, Gamemode gamemode, int winner, int time)
        {
            using (FLDBContext ctx = new FLDBContext())
            {
                Match m = new FLServer.Models.Match()
                {
                    Teams = teams,
                    MatchPlayed = DateTime.UtcNow,
                    MatchTime = time,
                    Winner = winner,
                    Map = map,
                    Gamemode = gamemode
                };
                ctx.Match.Add(m); ctx.SaveChanges();
                return m;
            }
        }


    }
}
