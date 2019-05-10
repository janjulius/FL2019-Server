using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace FLServer.Models
{
    public partial class Map
    {
        [Key]
        public int MapId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Image { get; set; }
        public Match Match { get; set; }
        [ForeignKey("Match")]
        public int MapFK {get; set;}
    }
}
