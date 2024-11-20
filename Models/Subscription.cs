namespace netflix_clone.Models
{
    public class Subscription
    {
        public int Id { get; set; }
        public int UserId { get; set; }  // Foreign Key to User
        public string SubscriptionType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        // public decimal Price { get; set; }
        // public string PaymentMethod { get; set; }
        // public string Status { get; set; }

        // Navigation Property (One-to-one relationship with User)
        public User User { get; set; }
    }
}