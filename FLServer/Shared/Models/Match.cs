using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FLServer.Models
{
    public partial class Match
    {
        [Key]
        public int MatchId { get; set; }
        public int Winner { get; set; }
        public int MatchTime { get; set; }
        public DateTime MatchPlayed { get; set; }
        public Map Map { get; set; }
        public Gamemode Gamemode { get; set; }

        public ICollection<Team> Teams { get; set; }
    }
}
