namespace netflix_clone.Models
{
    public class WatchList
    {
      public int Id { get; set; }
        public int ProfileId { get; set; }  // Foreign Key to Profile
        public DateTime AddedAt { get; set; }
        public string Name { get; set; }

        // Many-to-one relationship with Profile
        public Profile Profile { get; set; }

        // one-to-many relationship with Movie
        public ICollection<WatchListMovie> WatchListMovies { get; set; }  
    }

  
}