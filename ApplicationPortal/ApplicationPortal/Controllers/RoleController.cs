using ApplicationPortal.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
            //уеж есть роль юзера? или добавлять надо?
            await _context.SaveChangesAsync();

            return RedirectToAction("GetRoles", "Role");
        }
        //создать отдельный контроллер UserRoleController с методом выбоора
        // между юзером и его ролью(userid, roleid)
        //нужно ли для этого создавать отдельный класс юзер и юзать UserManager?

        public async Task<IActionResult> GetRoles()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            return View(roles);
        }
    }
}
