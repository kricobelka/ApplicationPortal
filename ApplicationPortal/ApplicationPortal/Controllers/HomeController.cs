using ApplicationPortal.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ApplicationPortal.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult AboutUs()
        {
            return View();
        }

        //private readonly ApplicationDbContext _context;

        // adding different contexts through DI to access the db
        //public HomeController(ApplicationDbContext context)
        //{
        //    _context = context;
        //}

        ////for ex to get users:
        //public IActionResult Index()
        //{
        //    var users = _context.Users.ToList();
        //    return View(users);
        //}
        //usage on razor page:
        //@model IEnumerable<Microsoft.AspNetCore.Identity.IdentityUser>


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}