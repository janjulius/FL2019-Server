using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace FLServer.Models
{
    public partial class Team
    {
        [Key]
        public int TeamId { get; set; }
        public Match Match { get; set; }
        [ForeignKey("Match")]
        public int MatchFK { get; set; }
        
    }
}
