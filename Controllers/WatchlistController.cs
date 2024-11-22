using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using netflix_clone.Models;
using netflix_clone.data;
using Microsoft.EntityFrameworkCore;


namespace netflix_clone.Controllers
{
    // [Route("[controller]")]
    public class WatchlistController : Controller
    {
        private readonly ILogger<WatchlistController> _logger;
        private readonly ApplicationDbContext _context;

        public WatchlistController(ILogger<WatchlistController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult index()
        {
            // Retrieve the UserId from the session
            var userId = HttpContext.Session.GetString("UserId");
            var userIdInt = int.Parse(userId);

            // var watchlist = _context.Watchlists
            //  .Include(w => w.User)     // Include the User entity
            //  .Include(w => w.Movies)   // Include the Movies entity
            //  .Where(w => w.UserId == userIdInt) // Filter by user

            //  .ToList();

            // var watchlist = _context.Watchlists
            // .Where(w => w.UserId == userIdInt)    // Filter by UserId
            // .SelectMany(w => w.Movies)         // Flatten the Movies collection
            // .ToList();


            var watchlistMovies = (from watchlist in _context.Watchlists
                                   join user in _context.Users on watchlist.UserId equals user.Id
                                   join movie in _context.Movies on watchlist.MovieId equals movie.Id
                                   where watchlist.UserId == userIdInt  // Filter by specific UserId
                                   select new
                                   {
                                       WatchlistId = watchlist.Id,
                                       UserName = user.Id,
                                       MovieId = watchlist.MovieId,
                                       MovieTitle = movie.Title,
                                       MovieImageUrl = movie.ImageUrl,
                                       MovieVideoUrl = movie.VideoUrl,
                                       MovieDescription = movie.Description,
                                       Status = watchlist.Status
                                   }).ToList();


            return View(watchlistMovies); // Pass the movie list to the view
        }

        // 


        public async Task<IActionResult> WatchlistAdd(int movieid)
        {
            // Retrieve the UserId from the session
            var userId = HttpContext.Session.GetString("UserId");

            // Check if UserId is null or empty
            if (string.IsNullOrEmpty(userId))
            {
                // You can handle the case when the user is not logged in, e.g., redirect to login page
                return RedirectToAction("Login", "User");
            }

            // Convert the userId to an integer
            var userIdInt = int.Parse(userId);

            // Prepare the new WatchListMovie object
            var watchlistadd = new WatchList
            {
                MovieId = movieid,
                AddedAt = DateTime.UtcNow,
                Status = "towatch", // Set the status as "towatch"
                UserId = userIdInt
            };

            // Add the new movie to the Watchlist
            _context.Watchlists.Add(watchlistadd);

            // Save the changes asynchronously
            await _context.SaveChangesAsync();

            // Redirect to the Watchlist Index page after adding the movie
            return RedirectToAction("Index", "Watchlist");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}