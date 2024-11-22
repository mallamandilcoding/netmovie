using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using netflix_clone.data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using netflix_clone.Attributes;

namespace netflix_clone.Controllers
{
    // [Route("[controller]")]
    // [Authorize]
    // [AuthorizeUser]
    public class MovieController : Controller
    {
        private readonly ILogger<MovieController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly string ffmpegPath = "/usr/local/bin/ffmpeg"; // Update with your ffmpeg path


        public MovieController(ILogger<MovieController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }


        // public IActionResult Index()
        // {
        //     var email = HttpContext.Session.GetString("Email");
        //     ViewBag.Email = email; // Show the user's email on the dashboard


        //     return View();
        // }

        // GET: Movies
        // [HttpGet("movies")]
        public async Task<IActionResult> Index(string searchQuery = "")
        {
            var userId = HttpContext.Session.GetString("UserId");
            // Fetch all movies including their related Genre
            var moviesQuery = _context.Movies.Include(m => m.Genre).AsQueryable();//Instead of fetching the data immediately, the code uses an IQueryable to apply a conditional filter only if a search query is provided.

            // Apply search filter if a search query is provided
            if (!string.IsNullOrEmpty(searchQuery))
            {
                moviesQuery = moviesQuery.Where(m =>
                    m.Title.Contains(searchQuery) ||
                    m.Genre.GenreName.Contains(searchQuery));
            }

            // Execute the query and retrieve the movies
            var movies = await moviesQuery.ToListAsync();

            // Pass the search query back to the view for display
            ViewBag.SearchQuery = searchQuery;
            ViewBag.UserId = userId;

            return View(movies);
        }

        public async Task<IActionResult> Details(int id)
        {
            var userId = HttpContext.Session.GetString("UserId");
            ViewBag.UserId = userId;
            ViewBag.MovieId = id;
            // var movie = await _context.Movies.FirstOrDefaultAsync(m => m.Id == id);
            var movie = await _context.Movies.Include(m => m.Genre).FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }
            return View(movie);
            // return View();
        }

        [HttpGet]
        public async Task<IActionResult> Stream(string videourl)
        {
            if (string.IsNullOrEmpty(videourl))
            {
                return BadRequest("Video URL is required.");
            }

            // Prepare paths
            string videoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", videourl.TrimStart('/'));
            string videoName = Path.GetFileNameWithoutExtension(videourl);
            string outputFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "videos", "hls", videoName);
            Directory.CreateDirectory(outputFolder);

            string m3u8FilePath = Path.Combine(outputFolder, "output.m3u8");

            // / Set response headers for HLS streaming
            Response.Headers["Content-Type"] = "application/x-mpegURL";

            // Set the response headers to ensure proper HLS streaming
            Response.Headers.Add("Cache-Control", "no-cache, no-store, must-revalidate"); // Prevent caching


            // If the HLS files already exist, skip generation
            if (!System.IO.File.Exists(m3u8FilePath))
            {
                var startInfo = new ProcessStartInfo
                {
                    FileName = "ffmpeg",
                    Arguments = $"-i \"{videoPath}\" -f hls -hls_time 60 -hls_list_size 0 -g 1 -c:v libx264 -c:a aac -preset veryfast \"{m3u8FilePath}\"",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (var ffmpegProcess = Process.Start(startInfo))
                {
                    if (ffmpegProcess == null)
                    {
                        return StatusCode(500, "Error starting FFmpeg process.");
                    }

                    await ffmpegProcess.WaitForExitAsync();

                    if (ffmpegProcess.ExitCode != 0)
                    {
                        string error = await ffmpegProcess.StandardError.ReadToEndAsync();
                        return StatusCode(500, $"FFmpeg error: {error}");
                    }
                }
            }

            // Redirect to PlayVideo after generating the HLS files
            return RedirectToAction("PlayVideo", new { videourl });
        }


        [HttpGet]
        public IActionResult PlayVideo(string videourl)
        {
            if (string.IsNullOrEmpty(videourl))
            {
                return BadRequest("Video URL is required.");
            }

            string videoName = Path.GetFileNameWithoutExtension(videourl);
            string m3u8Url = $"/videos/hls/{videoName}/output.m3u8";

            // Pass the .m3u8 URL to the view
            ViewData["VideoUrl"] = m3u8Url;

            return View();
        }









        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {

            return View("Error!");
        }
    }
}