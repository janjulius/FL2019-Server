using System;
using System.Collections.Generic;

namespace FLServer.Models
{
    public partial class UserFriend
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int FriendId { get; set; }
    }
}
