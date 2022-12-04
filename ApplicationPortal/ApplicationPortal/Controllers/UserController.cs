using ApplicationPortal.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApplicationPortal.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;
        RoleManager<IdentityRole> _roleManager;
        UserManager<IdentityUser> _userManager;

        public UserController(UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        //добавить для юзера hijikatanya right of admin

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


