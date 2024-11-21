using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using netflix_clone.Models;
using netflix_clone.data;

namespace netflix_clone.Controllers
{
    [Route("[controller]")]
    public class WatchlistController : Controller
    {
        private readonly ILogger<WatchlistController> _logger;
        private readonly ApplicationDbContext _context;

        public WatchlistController(ILogger<WatchlistController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }


        public IActionResult WatchlistAdd(int movieid)
        {
            var userId = HttpContext.Session.GetString("UserId");
            // var movieId = movieid;
            DateTime AddedAt = DateTime.UtcNow;
            var Status = "towatch";
            var watchlistadd = new WatchListMovie
            {
                MovieId = movieid,
                AddedAt = AddedAt,
                Status = Status,
                UserId = int.Parse(userId)
            };
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}