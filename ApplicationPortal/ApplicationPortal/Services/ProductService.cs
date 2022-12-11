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
        ////для проверки юзером корректности мэйла и компании
        #region user and company info
        public async Task<UserCompaniesViewModel> GetUserAndCompanyInfo(string userName)
        {
            //second variant:
            //var selectedUser2 = await _context.Users.Where(q => q.Id == userId).SingleOrDefaultAsync();

            var userAndCompanyInfo = await _context.Users.Where(q => q.UserName == userName)
                .Select(q => new UserCompaniesViewModel
                {
                    UserEmail = q.Email,
                    CompanyName = q.Company.CompanyName

                }).SingleOrDefaultAsync();

            return userAndCompanyInfo;
        }

        #endregion

        #region general product info
        
        //в этом методе айдишник не нужен:
        public async Task<ProductViewModel> CreateProductGeneralInfo(GenProductViewModel product)
        {
            var newProduct = new Product()
            {
                Name = product.Name,
                Model = product.Model,
                Brand = product.Brand,
                Manufacturer = product.Manufacturer
            };

            await _context.AddAsync(newProduct);

            await _context.SaveChangesAsync();

            return new ProductViewModel()
            {
                Id = newProduct.Id,
                Name = newProduct.Name,
                Brand = newProduct.Brand,
                Model = newProduct.Model,
                Manufacturer = newProduct.Manufacturer,
            };
        }
        #endregion


        #region technical product info

        //return ProdViewModel to tarnsfer id to the next method:
        public async Task<ProductViewModel> PostTechnicalProductInfo(int productId, TechProductViewModel product)
        {
            //var technicalproduct = await _context.Products.Where(q => q.Id == productId).FirstOrDefaultAsync();
            //var technicalproduct = await _context.Products.FindAsync(productId);
            
            //technicalproduct.Frequencies = 
            //await _context.SaveChangesAsync();

            return null;
        }
        #endregion


        #region other information + path to file
        public async Task<List<ProductViewModel>> GetExtraProductInfo()
        {
            var extraInfo = await _context.Products.Select(q => new ProductViewModel
            {
                OtherInformation = q.OtherInformation,
                PathToFile = q.PathToFile
            }).ToListAsync();

            return extraInfo;
        }

        public async Task GetExtraProductInfo(ProductViewModel product)
        {
            await GetExtraProductInfo();
            await _context.AddAsync(product);
            await _context.SaveChangesAsync();
        }
        #endregion
    }
}
