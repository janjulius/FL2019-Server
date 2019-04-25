using System;
using System.Collections.Generic;

namespace FLServer.Models
{
    public partial class Purchase
    {
        public int PurchaseId { get; set; }
        public DateTime PurchaseDate { get; set; }
        public int? UserId { get; set; }
        public int? UserId1 { get; set; }
    }
}
