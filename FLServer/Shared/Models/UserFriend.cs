using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FLServer.Models
{
    public partial class UserFriend
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public int FriendId { get; set; }
        public DateTime AddedDate { get; set; }
    }
}
