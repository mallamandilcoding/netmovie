using System.ComponentModel.DataAnnotations;

namespace netflix_clone.Models
{
    public class MoviesFeatures
    {
        [Key]
        public int FeatureId { get; set; }
        public int MovieId { get; set; }  // Foreign Key to Movie
        public string FeatureType { get; set; }
        public string Description { get; set; }

        // Navigation Property (many-to-one relationship with Movie)
        public Movie Movie { get; set; }
    }
}