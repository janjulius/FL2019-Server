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
        public ICollection<Player> Players { get; set; }
    }
}
