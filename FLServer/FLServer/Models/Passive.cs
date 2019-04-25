using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace FLServer.Models
{
    public partial class Passive
    {
        [Key]
        public int PassiveId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Character Character { get; set; }
        [ForeignKey("Character")]
        public int CharacterFK { get; set; }

        
    }
}
