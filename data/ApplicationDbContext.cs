using Microsoft.EntityFrameworkCore;
using netflix_clone.Models;

namespace netflix_clone.data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        // DbSets for your entities
        public DbSet<User> Users { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<WatchList> Watchlists { get; set; }
        public DbSet<MoviesFeatures> MoviesFeatures { get; set; }
        // public DbSet<WatchListMovie> WatchlistMovies { get; set; }


        // Configure relationships


    }



}
