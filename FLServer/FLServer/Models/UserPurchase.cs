using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FLServer.Models
{
    class UserPurchase
    {
        [Key] public int UserPurchaseId { get; set; }
        public int UserId { get; set; }
        public int PurchaseId { get; set; }
    }
}
