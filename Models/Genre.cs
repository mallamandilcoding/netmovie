namespace netflix_clone.Models
{
    public class Genre
    {
        public int GenreId { get; set; }
        public string GenreName { get; set; }

        // One-to-many relationship with Movie
        public ICollection<Movie> Movies { get; set; }
    }
}