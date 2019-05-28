using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace FLWeb.Models
{
    public class Admin
    {
        [Key]
        public int UserId { get; set; }
    }
}
