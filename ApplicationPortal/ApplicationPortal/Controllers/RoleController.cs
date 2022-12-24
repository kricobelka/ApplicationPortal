using ApplicationPortal.Data;
using ApplicationPortal.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApplicationPortal.Controllers
{
    public class RoleController : Controller
    {
        RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;
        UserManager<User> _userManager;

        public RoleController(RoleManager<IdentityRole> roleManager,
            ApplicationDbContext context, UserManager<User> userManager)
        {
            _roleManager = roleManager;
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> CreateAdminRole()
        {
            var adminRole = await _roleManager.CreateAsync(
                new IdentityRole
                {
                    Name = "Admin",
                    NormalizedName = "Admin"
                });

            var userRole = await _roleManager.CreateAsync(
                new IdentityRole

                {
                    Name = "User",
                    NormalizedName = "User"
                });

            await _context.SaveChangesAsync();

            return RedirectToAction("GetRoles", "Role");
        }

        //создать отдельный контроллер UserRoleController с методом выбоора
        // между юзером и его ролью(userid, roleid)
        //нужно ли для этого создавать отдельный класс юзер и юзать UserManager?

        public async Task<IActionResult> GetRoles()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            await _context.SaveChangesAsync();
            return View(roles);
        }
    }
}
