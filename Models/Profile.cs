namespace netflix_clone.Models
{
    public class Profile
    {
        public int Id { get; set; }
        // public int UserId { get; set; }  // Foreign Key to User
        public string ProfileName { get; set; }
        public string ProfilePicture { get; set; }

        // One-to-many relationship with Watchlist
        // public ICollection<WatchList> Watchlists { get; set; }

        // Navigation Property to User
        // public User User { get; set; }
    }
}