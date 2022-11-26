using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using ApplicationPortal.Data;
using Microsoft.EntityFrameworkCore;

namespace ApplicationPortal.Controllers
{
    public class RoleController : Controller
    {
        RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;

        public RoleController(RoleManager<IdentityRole> roleManager,
            ApplicationDbContext context)
        {

            _roleManager = roleManager;
            _context = context;
        }

        public async Task<IActionResult> CreateAdminRole()
        {
            await _roleManager.CreateAsync(new IdentityRole()
            { Name = "Admin", NormalizedName = "Admin" });

            await _context.SaveChangesAsync();

            return RedirectToAction("GetRoles", "Admin");           
        }
//создать отдельный контроллер UserRoleController с методом выбоора
// между юзером и его ролью(userid, roleid)

        public async Task<IActionResult> GetRoles()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            return View(roles);
        }

        //вывести юзеров (включая админа) и их роли:
        //public async Task<IActionResult> GetUsersAndRoles()
        //{
        //    var users = _context.UserRoles.Select(q => new
        //    {

        //    });

        //    return View(users);
        //}
        
        
    }
}
