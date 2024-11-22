namespace netflix_clone.Models
{
  public class WatchList
  {
    public int Id { get; set; }
    // public int ProfileId { get; set; }  // Foreign Key to Profile
    public DateTime AddedAt { get; set; }
    // public string Name { get; set; }

    public int UserId { get; set; } //Foreign Key to user
    public int MovieId { get; set; }      // Foreign Key to Movie
    public string Status { get; set; }    // Status (To Watch, Watched)


    //one to one
    public User User { get; set; }
    // One Watchlist can have many Movies
    public ICollection<Movie> Movies { get; set; }

  }


}