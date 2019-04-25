
using System;
using System.Collections.Generic;

namespace FLServer.Models
{
    public partial class UserFriend
    {
        public int Id { get; set; }
        public int UserId { get; set; }
         public string Username {get;set;}
         public int Exp {get;set;}
        public int Level {get;set;}
        public DateTime CreationDate { get; set; }

        public List<Usertest> Friends { get; set; }

        public int FriendId { get; set; }

    }
}
