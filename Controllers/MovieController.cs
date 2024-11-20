using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using netflix_clone.data;
using Microsoft.EntityFrameworkCore;

namespace netflix_clone.Controllers
{
    // [Route("[controller]")]
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

            return View(movies);
        }

        public async Task<IActionResult> Details(int id)
        {
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




        // This action generates and serves the HLS playlist (.m3u8 file)
        [HttpGet]
        public async Task<IActionResult> Stream(string videourl)
        {
            if (string.IsNullOrEmpty(videourl))
            {
                return BadRequest("Video URL is required.");
            }

            string videoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", videourl.TrimStart('/'));

            // Extract the video name from the video URL
            string videoNameWithExtension = Path.GetFileName(videourl); // e.g., "your_video.mp4"
            string videoName = Path.GetFileNameWithoutExtension(videoNameWithExtension);
            string outputFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "videos", "hls", videoName);

            // Ensure the output folder exists
            Directory.CreateDirectory(outputFolder);

            string m3u8FilePath = Path.Combine(outputFolder, "output.m3u8");

            // FFmpeg arguments to generate the HLS playlist (.m3u8 file)
            var startInfo = new ProcessStartInfo
            {
                FileName = ffmpegPath,
                Arguments = $"-i \"{videoPath}\" -f hls -hls_time 1 -hls_list_size 0 -c:v libx264 -c:a aac -preset veryfast \"{m3u8FilePath}\"",
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

                // Wait for the ffmpeg process to finish
                await ffmpegProcess.WaitForExitAsync();
                if (ffmpegProcess.ExitCode != 0)
                {
                    string error = await ffmpegProcess.StandardError.ReadToEndAsync();
                    return StatusCode(500, $"FFmpeg error: {error}");
                }
            }

            // Return the .m3u8 file path in the response
            // string videoUrl = $"/videos/hls/{videoName}/output.m3u8";

            // Set the correct content type for HLS streaming
            Response.ContentType = "application/vnd.apple.mpegurl";
            Response.Headers.Append("Access-Control-Allow-Origin", "*");  // Allow all origins for CORS

            // return File(System.IO.File.OpenRead(m3u8FilePath), "application/vnd.apple.mpegurl");
            // Return the .m3u8 file as a stream
            var fileStream = new FileStream(m3u8FilePath, FileMode.Open, FileAccess.Read);
            return File(fileStream, "application/vnd.apple.mpegurl", "output.m3u8");
        }

        // PlayVideo action
        public async Task<IActionResult> PlayVideo(string videourl)
        {
            if (string.IsNullOrEmpty(videourl))
            {
                return BadRequest("Video URL is required.");
            }

            // Call the Stream action to generate and retrieve the .m3u8 file URL
            var result = await Stream(videourl);

            if (result is FileContentResult fileResult)
            {
                // Retrieve the video URL from the stream result (the .m3u8 URL)
                string m3u8Url = $"/videos/hls/{Path.GetFileNameWithoutExtension(videourl)}/output.m3u8";
                ViewData["VideoUrl"] = m3u8Url; // Set the video URL to pass to the view
            }
            else
            {
                return StatusCode(500, "Error processing video.");
            }

            return View();
        }


        // public async Task<IActionResult> PlayVideo(string videourl)
        // {
        //     if (string.IsNullOrEmpty(videourl))
        //     {
        //         return BadRequest("Video URL is required.");
        //     }

        //     // Call the Stream action to generate the .m3u8 file
        //     var result = await Stream(videourl);

        //     if (result is OkObjectResult okResult)
        //     {
        //         // Get the video URL from the stream result (the .m3u8 URL)
        //         var m3u8Url = okResult.Value as string;
        //         ViewData["VideoUrl"] = m3u8Url; // Set the video URL to pass it to the view
        //     }
        //     else
        //     {
        //         return StatusCode(500, "Error processing video.");
        //     }

        //     return View();
        // }

        // public IActionResult PlayVideo(string videourl)
        // {
        //     // Pass the videoUrl to the view
        //     ViewData["videourl"] = videourl;
        //     return View(); // Return to the PlayVideo.cshtml view
        // }





        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}