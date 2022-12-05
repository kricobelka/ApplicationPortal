using ApplicationPortal.Data;
using ApplicationPortal.Data.Entities;
using ApplicationPortal.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApplicationPortal.Services
{
    public class ProductService
    {
        private readonly ApplicationDbContext _context;

        public ProductService(ApplicationDbContext context)
        {
            _context = context;
        }

        ////выводим информацию о залогиненном юзере (мэйл  и название компаний)
        ////для проверки юзером и выбора (компании выбираем, мэйл проверяем)

        //public async Task<List<UserCompaniesViewModel>> GetUserAndCompanyInfo(int userId)
        //{
        //    //как правильнее выыводить информацию? через анонимный объект?
        //    var selectedUser = await _context.Users.FindAsync(userId);

        //    //check
        //    //var selecteddUser = await _context.Users.Where(q => q.Id == userId);
        //    //var data = await _context.Users.Select(q => q.Company.CompanyName)
        //    //    .ToListAsync();

        //    var userAndCompanyInfo = await _context.Users.Select(q => new UserCompaniesViewModel
        //    {
        //        UserEmail = selectedUser.UserName,
        //        CompanyName = selectedUser.Company.CompanyName
        //    }).ToListAsync();

        //    await _context.SaveChangesAsync();

        //    //можно ли как-то вернуть без создания отдельной вьюмодели?
        //    return userAndCompanyInfo;
        //}

        //public async Task UsersAndCompanyPost(int userId, UserCompaniesViewModel viewModel)
        //{
        //    await GetUserAndCompanyInfo(userId);
        //    await _context.AddAsync(viewModel);
        //    await _context.SaveChangesAsync();  
        //}

        //public async Task<List<Product>> GetGeneralProductInfo()
        //{
        //    var genProdInfo = await _context.
        //    await _context.SaveChangesAsync();
        //}
    }
}
