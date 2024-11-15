using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace netflix_clone.Models
{
    public class PaymentMethod
    {
        public int PaymentMethodId { get; set; }
        public int UserId { get; set; }  // Foreign Key to User
        public string CardNumber { get; set; }
        public DateTime ExpirationDate { get; set; }

        // Navigation Property (many-to-one relationship with User)
        public User User { get; set; }
    }
}
