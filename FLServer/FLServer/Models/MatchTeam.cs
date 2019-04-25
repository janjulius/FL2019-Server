using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FLServer.Models
{
    public partial class MatchTeam
    {
        [Key] 
        public int MatchTeamId { get; set; }
        public int MatchId { get; set; }
        public int TeamId { get; set; }
    }
}
