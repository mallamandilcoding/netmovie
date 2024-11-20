namespace netflix_clone.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public int GenreId { get; set; }  // Foreign Key to Genre
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime ReleaseDate { get; set; }
        public float Rating { get; set; }
        public int Duration { get; set; }
        public string Director { get; set; }
        public string Cast { get; set; }
        public string Language { get; set; }
        public string AgeRating { get; set; }
        public string ImageUrl { get; set; }  // URL or path to the image
        public string VideoUrl { get; set; }      // URL or path to the video file
        // One-to-many relationship with MovieFeatures
        public ICollection<MoviesFeatures> MoviesFeatures { get; set; }

        // Many-to-many relationship with WatchlistMovie
        public ICollection<WatchListMovie> WatchlistMovies { get; set; }

        // Navigation Property to Genre
        public Genre Genre { get; set; }
    }
}