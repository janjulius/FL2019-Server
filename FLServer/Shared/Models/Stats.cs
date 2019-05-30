using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FLServer.Models
{
    public class Stats
    {
        [Key]
        public int StatsId { get; set; }

        public int DamageDealt { get; set; }
        public int DamageTaken { get; set; }
        public int HighestPercentage { get; set; }
        public int Kills { get; set; }
        public int Deaths { get; set; }

    }
}