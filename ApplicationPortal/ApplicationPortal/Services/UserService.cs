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
        private readonly SignInManager<User> _signInManager;

        public UserService(ApplicationDbContext context, SignInManager<User> signInManager)
        {
            _context = context;
            _signInManager = signInManager;
        }

        public async Task<UserViewModelResponse> GetUserProfile(string userId)
        {
            //add id in responsemodel?
            var userProfileInfo = await _context.Users.Where(q => q.Id == userId)
                .Include(q => q.Company)
                .Select(q => new UserViewModelResponse
                {
                    FirstName = q.FirstName,
                    LastName = q.LastName,
                    UserEmail = q.Email,
                    Phone = q.PhoneNumber,
                    BirthDate = q.BirthDate,
                    //if company == null, company = null, else getting new companyviewmodel
                    Company = q.Company == null ? null :
                    new CompanyViewModel
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
            var user = await _context.Users.Include(q => q.Company)
                .FirstOrDefaultAsync(q => q.Id == userId);

            user.Email = requestModel.UserEmail;
            user.FirstName = requestModel.FirstName;
            user.LastName = requestModel.LastName;
            user.PhoneNumber = requestModel.Phone;
            user.BirthDate = requestModel.BirthDate;
            user.UserName = requestModel.UserEmail;
           
            if (user.Company == null)
            {
                user.Company = new Company()
                {
                    Address = requestModel.Company.Address,
                    BusinessId = requestModel.Company.BusinessId
                };
            }
            else
            {
                user.Company.Address = requestModel.Company.Address;
                user.Company.BusinessId = requestModel.Company.BusinessId;
            }

            await _context.SaveChangesAsync();
            await _signInManager.RefreshSignInAsync(user);

            return new UserViewModelResponse
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserEmail = user.Email,
                UserName = user.UserName,
                Phone = user.PhoneNumber,
                BirthDate = user.BirthDate,
                Company = new CompanyViewModel
                {
                    CompanyId = user.Company.CompanyId,
                    CompanyName = user.Company.CompanyName,
                    Address = user.Company.Address,
                    BusinessId = user.Company.BusinessId
                }
            };
        }
    }
}
