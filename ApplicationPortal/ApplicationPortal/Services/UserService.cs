using ApplicationPortal.Data;
using ApplicationPortal.Data.Entities;
using ApplicationPortal.Enums;
using ApplicationPortal.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApplicationPortal.Services
{
    public class UserService
    {
        private readonly ApplicationDbContext _context;
        //private readonly UserManager<User> _userManager;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UserViewModelResponse> GetUserProfile(string userId)
        {
            var userProfileInfo = await _context.Users.Where(q => q.Id == userId)
                .Select(q => new UserViewModelResponse
                {
                    FirstName  = q.FirstName,
                    LastName = q.LastName,
                    UserEmail = q.Email,
                    Phone = q.PhoneNumber,
                    BirthDate = q.BirthDate,
                    Company = new CompanyViewModel
                    {
                        CompanyId = q.Company.CompanyId,
                        CompanyName = q.Company.CompanyName,
                        Address = q.Company.Address,
                        BusinessId = q.Company.BusinessId
                    }

                }).SingleOrDefaultAsync();

            return userProfileInfo;
        }

        
        public async Task<UserViewModelResponse> EditUserProfile(string userId, UserViewModelRequest requestModel)
        {
            var user = await _context.Users.FindAsync(userId);

            user.Email = requestModel.UserEmail;
            user.FirstName = requestModel.FirstName;
            user.LastName = requestModel.LastName;
            user.PhoneNumber = requestModel.Phone;
            user.BirthDate = requestModel.BirthDate;

            await _context.SaveChangesAsync();

            return new UserViewModelResponse
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserEmail = user.Email,
                Phone = user.PhoneNumber,
                BirthDate = user.BirthDate,
                //Company = new CompanyViewModel
                //{
                //    CompanyId = user.Company.CompanyId,
                //    CompanyName = user.Company.CompanyName,
                //    Address = user.Company.Address,
                ////    BusinessId = user.Company.BusinessId
                //}
            };
        }
    }
}
