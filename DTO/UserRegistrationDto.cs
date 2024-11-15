using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace netflix_clone.DTO
{
    public class UserRegistrationDto
    {
        // Email address (validation included)
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        // Password (validation for length)
        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string Password { get; set; }

        // Confirm password (must match the password)
        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }

        // Subscription plan (e.g., Basic, Premium)
        [Required]
        public string SubscriptionType { get; set; }

        // Card number for payment
        [Required]
        [CreditCard]
        public string CardNumber { get; set; }

        // Card expiry date
        [Required]
        public DateTime CardExpiryDate { get; set; }
        
    }
}