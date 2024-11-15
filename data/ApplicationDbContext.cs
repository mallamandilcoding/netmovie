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
        public DbSet<WatchListMovie> WatchlistMovies { get; set; }


  // Configure relationships
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        //  // Ignore navigation properties for User entity
        modelBuilder.Entity<User>()
            .Ignore(u => u.PaymentMethods)  // Ignore PaymentMethods navigation property
            .Ignore(u => u.Profiles)        // Ignore Profiles navigation property
            .Ignore(u => u.Subscription);   // Ignore Subscription navigation property


        // // One-to-many relationship between User and PaymentMethod
        // modelBuilder.Entity<User>()
        //     .HasMany(u => u.PaymentMethods)  // A User has many PaymentMethods
        //     .WithOne(pm => pm.User)          // Each PaymentMethod is associated with one User
        //     .HasForeignKey(pm => pm.UserId)  // Foreign key in PaymentMethod
        //     .OnDelete(DeleteBehavior.Cascade); // Optional: Delete PaymentMethods when User is deleted

        // // One-to-one relationship between User and Subscription
        // modelBuilder.Entity<User>()
        //     .HasOne(u => u.Subscription)    // A User has one Subscription
        //     .WithOne(s => s.User)           // A Subscription has one User
        //     .HasForeignKey<Subscription>(s => s.UserId)  // Foreign key in Subscription
        //     .OnDelete(DeleteBehavior.Cascade);  // Optional: Delete Subscription when User is deleted

        // // One-to-many relationship between User and Profile
        // modelBuilder.Entity<User>()
        //     .HasMany(u => u.Profiles)  // A User can have many Profiles
        //     .WithOne(p => p.User)      // Each Profile is associated with one User
        //     .HasForeignKey(p => p.UserId)
        //     .OnDelete(DeleteBehavior.Cascade);
    }


    }


    
}
