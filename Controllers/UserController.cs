using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using netflix_clone.Models;
using Microsoft.AspNetCore.Identity;
using netflix_clone.data;
using netflix_clone.DTO;
using Microsoft.EntityFrameworkCore;
using netflix_clone.ViewModels;



namespace netflix_clone.Controllers
{
    // [ApiController]
    // [Route("api/[controller]")]
    public class UserController : Controller
    {



        private readonly ILogger<UserController> _logger;
        private readonly ApplicationDbContext _context;

        public UserController(ILogger<UserController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet("users")]
        public IActionResult Index()
        {
            return View();
        }



        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            var user = _context.Users.SingleOrDefault(u => u.Email == email);
            if (user != null && user.Password == password)
            {
                // Set session and cookie if authentication is successful
                HttpContext.Session.SetString("Email", email);
                HttpContext.Session.SetString("UserId", user.Id.ToString());

                var cookieValue = $"{email}|{user.Id}";
                Response.Cookies.Append("UserInfo", cookieValue, new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddMinutes(30),
                    HttpOnly = true,
                    Secure = true
                });

                return RedirectToAction("Index", "Movie");
            }

            ViewBag.ErrorMessage = "Invalid email or password.";
            return View();
        }


        [HttpPost]
        public IActionResult Logout()
        {
            // Clear the session
            HttpContext.Session.Remove("Email");

            // Remove the authentication cookie (if used)
            Response.Cookies.Delete("Email");

            // Redirect to the home page or login page
            return RedirectToAction("Login", "User");  // Or RedirectToAction("Index", "Home");
        }



        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }


        [HttpPost]
        // [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
                    if (existingUser != null)
                    {
                        ModelState.AddModelError("Email", "Email is already registered.");
                        return View(model);
                    }

                    // Create the new user (no password hashing)
                    var user = new User
                    {

                        Email = model.Email,
                        DateJoined = DateTime.UtcNow,
                        Password = model.Password
                    };

                    _context.Users.Add(user);
                    await _context.SaveChangesAsync();

                    // Create the payment method (save card number)
                    var paymentMethod = new PaymentMethod
                    {
                        CardNumber = model.CardNumber,
                        UserId = user.Id
                    };

                    _context.PaymentMethods.Add(paymentMethod);



                    // Create the subscription


                    // Determine the subscription period and calculate start and end dates
                    var subscriptionPeriod = model.SubscriptionPeriod;
                    DateTime startDate = DateTime.UtcNow;
                    DateTime endDate = DateTime.UtcNow;
                    if (subscriptionPeriod == "1month")
                    {
                        endDate = startDate.AddMonths(1);
                    }
                    else if (subscriptionPeriod == "3months")
                    {
                        endDate = startDate.AddMonths(3);
                    }
                    else if (subscriptionPeriod == "6months")
                    {
                        endDate = startDate.AddMonths(6);
                    }
                    var subscription = new Subscription
                    {
                        SubscriptionType = model.SubscriptionType,  // Subscription plan (Basic/Standard/Premium)
                        StartDate = startDate,
                        EndDate = endDate,
                        UserId = user.Id
                    };

                    _context.Subscriptions.Add(subscription);

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync(); // Commit the transaction if all operations succeed

                    // Set session for the user
                    HttpContext.Session.SetString("Email", user.Email);
                    HttpContext.Session.SetInt32("UserId", user.Id);

                    // Optionally set a cookie (for example, for persistent login)
                    var cookieOptions = new CookieOptions
                    {
                        HttpOnly = true,
                        Expires = DateTime.UtcNow.AddDays(7) // Cookie valid for 7 days
                    };
                    Response.Cookies.Append("UserEmail", user.Email, cookieOptions);



                    // Redirect to login or home page after successful registration
                    return RedirectToAction("Index", "Movie");
                }
                catch (Exception ex)
                {

                    // Roll back the transaction if any error occurs
                    await transaction.RollbackAsync();
                    Console.WriteLine($"Error during registration: {ex.Message}");

                    // Log or display the error to the user
                    ModelState.AddModelError("", "An unexpected error occurred. Please try again.");
                    return View(model);
                }


            }
            else
            {
                // If we reach here, something went wrong, return the view with validation errors
                return View(model);
            }
        }






        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }



    }
}