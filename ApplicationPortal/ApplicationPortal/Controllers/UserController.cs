using ApplicationPortal.Data;
using ApplicationPortal.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApplicationPortal.Constants;

namespace ApplicationPortal.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;
        readonly RoleManager<IdentityRole> _roleManager;
        readonly UserManager<User> _userManager;

        public UserController(UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        public async Task<IActionResult> AddAdminRoleToUser()
        {
            var user = await _userManager.FindByNameAsync("hijikataanya@gmail.com");
            await _userManager.AddToRoleAsync(user, Constants.AdminUserRoles._admin);
            return RedirectToAction("GetRoles", "Role");
        }

        //public async Task<IActionResult> UsersAndTheirRoles()
        //{

        //}

        #region  методом выбоора между юзером и его ролью(userid, roleid)
        //где происходит выбор между ролями? после логина?

        //[HttpGet]
        ////indicate it or string? in userroles table id = nvarchar 
        //public async Task<IActionResult> SelectRole(int id)
        //{
        //    var selectedUser = await _context.Users.FindAsync(id);
        //    var selectedRole = await _context.Roles.ToListAsync();
        //    var userAndRoles = await _context.UserRoles.Select(q => new
        //    {
        //        User = selectedUser,
        //        Role = selectedRole
        //    }).ToListAsync();

        //    return View(userAndRoles);
        //}

        ////UserRoles: набор объектов IdentityUserRole, 
        ////    соответствует таблице, которая сопоставляет 
        ////    пользователей и их роли

        //[HttpPost]
        //public async Task<IActionResult> SelectRole(IdentityUser userId, IdentityRole roleId)
        //{
        //    await _context.Users.FindAsync(userId);
        //    if (roleId == await _roleManager.FindByNameAsync("Admin"))
        //    {
        //        var selectedRole = await _roleManager.UpdateAsync(roleId);
        //        return View(selectedRole);
        //        //_roleManager.SetRoleNameAsync
        //    }
        //    // нужно ко всем остальным юзерам кроме админа повесить роль "юзер" или она сама вешается
        //    else if (roleId == await _roleManager.FindByNameAsync("User"))
        //    {
        //        //var selectedRole = await _roleManager.UpdateAsync(roleId);   
        //        return View(await _roleManager.UpdateAsync(roleId));
        //    }
        //    return RedirectToAction();
        //}
        #endregion
    }
}


