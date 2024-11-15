using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using netflix_clone.Models;
using Microsoft.AspNetCore.Identity;
using netflix_clone.data;
using netflix_clone.DTO;
using Microsoft.EntityFrameworkCore;


namespace netflix_clone.Controllers
{
    // [ApiController]
    // [Route("api/[controller]")]
    public class UserController : Controller
    {

        

        private readonly ILogger<UserController> _logger;
        private readonly ApplicationDbContext _context; 

        public UserController(ILogger<UserController> logger,ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        
        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }

        
       
    }
}