using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FLServer.Models
{
    public partial class TeamPlayer
    {
        [Key]
        public int TeamPlayerId { get; set; }
        public int TeamId { get; set; }
        public int PlayerId { get; set; }

    }
}
