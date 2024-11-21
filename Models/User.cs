namespace netflix_clone.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }       // Password (hashed)
        public DateTime DateJoined { get; set; }   // Date user joined

        // One-to-many relationship with PaymentMethod
        public ICollection<PaymentMethod> PaymentMethods { get; set; }

        // One-to-many relationship with Profile
        public ICollection<Profile> Profiles { get; set; }

        // One-to-one relationship with Subscription
        public Subscription Subscription { get; set; }

        // One-to-many relationship
        public ICollection<WatchListMovie> WatchListMovies { get; set; }

    }
}
