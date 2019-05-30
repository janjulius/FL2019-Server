using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FLServer.Models
{
    public partial class Player
    {
        [Key]
        public int PlayerId { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
        public int CharacterId { get; set; }
        public Character Character { get; set; }
        public int StatsId { get; set; }
        public Stats Stats { get; set; }
    }
}
