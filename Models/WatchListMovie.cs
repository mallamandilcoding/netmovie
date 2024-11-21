using System.ComponentModel.DataAnnotations;

namespace netflix_clone.Models
{
    public class WatchListMovie
    {

        [Key]
        public int WatchlistMovieId { get; set; }
        // public int WatchlistId { get; set; }  // Foreign Key to Watchlist

        public int UserId { get; set; } //Foreign Key to user
        public int MovieId { get; set; }      // Foreign Key to Movie
        public DateTime AddedAt { get; set; }
        public string Status { get; set; }    // Status (To Watch, Watched)

        // Composite Primary Key: WatchlistId + MovieId
        // public WatchList WatchList { get; set; }
        public Movie Movie { get; set; }
        public User User { get; set; }

    }
}