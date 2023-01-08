﻿using ApplicationPortal.Data;
using ApplicationPortal.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApplicationPortal.Constants;
using ApplicationPortal.Services;
using ApplicationPortal.Models;

namespace ApplicationPortal.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;
        readonly RoleManager<IdentityRole> _roleManager;
        readonly UserManager<User> _userManager;
        readonly UserService _userService;

        public UserController(UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext context, UserService userService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
            _userService = userService;
        }

        public async Task<IActionResult> AddAdminRoleToUser()
        {
            var user = await _userManager.FindByNameAsync("hijikataanya@gmail.com");
            await _userManager.AddToRoleAsync(user, Constants.AdminUserRoles._admin);
            return RedirectToAction("GetRoles", "Role");
        }

        //#region userprofile

        public async Task<IActionResult> GetUserProfile()
        {
            string userId = GetUserId();
            var userProfile = await _userService.GetUserProfile(userId);
            //include Company in the db
            return View(userProfile);
        }

        [HttpGet]
        public async Task<IActionResult> EditUserProfile()
        {
            string userId = GetUserId();
            var userProfile = await _userService.GetUserProfile(userId);
            return View(userProfile);
        }

        [HttpPost]
        public async Task<IActionResult> EditUserProfile(UserViewModelRequest requestModel)
        {
            if (ModelState.IsValid)
            {
                string userId = GetUserId();
                await _userService.EditUserProfile(userId, requestModel);
                return RedirectToAction("GetUserProfile", "User");
            }

            return View(requestModel);
        }

        public string GetUserId()
        {
            var userId = _userManager.GetUserId(User);
            return userId;
        }
    }
}
