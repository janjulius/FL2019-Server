
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace FLServer
{
    class Usertest
    {
        [Key]
        public int UserId { get; set; }
         public string Username {get;set;}
         public int Exp {get;set;}
        public int Level {get;set;}
        public DateTime CreationDate { get; set; }

        public List<Usertest> Friends { get; set; }
    }
}
