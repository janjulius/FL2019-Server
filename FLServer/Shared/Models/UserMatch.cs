using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FLServer.Models
{
    public partial class UserMatch
    {
        [Key]
        public int UserMatchId { get; set; }
        public int UserId { get; set; }
        public int MatchId { get; set; }
    }
}
