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
            //можно ли было вернуть новый продукт вместо продактмодели? и ющат айдишник того продукта
            var newProduct = new Product()
            {
                Name = product.Name,
                Model = product.Model,
                Brand = product.Brand,
                Manufacturer = product.Manufacturer
            };

            await _context.AddAsync(newProduct);

            await _context.SaveChangesAsync();

            return FullMap(newProduct);
            //return new ProductViewModel()
            //{
            //    Id = newProduct.Id,
            //    Name = newProduct.Name,
            //    Brand = newProduct.Brand,
            //    Model = newProduct.Model,
            //    Manufacturer = newProduct.Manufacturer,
            //};
        }
        #endregion

        #region technical product info

        //return ProdViewModel to tarnsfer id to the next method:
        public async Task<ProductViewModel> UpdateProductWithTechnicalInfo(int productId, TechProductViewModel product)
        {
            //можно ил так найти подукт?
            //var technicalproductt = await _context.Products.Where(q => q.Id == productId && q.Id == product.ProductId).FirstOrDefaultAsync();
            //var technicalproductt = await _context.Products.Where(q => q.Equals(productId) && q.Equals(product)).FirstOrDefaultAsync();
            var technicalproduct = await _context.Products.FindAsync(productId);

            //обязательно ли след действие? нельзя сразу врнуть ProductViewModel?
            //нельзя потому что мы возвращали в контроллере techproductviewmodel для связ экшенов айдишниками?
            technicalproduct.OutputPower = product.OutputPower;
            //AntennaGainAmount = technicalproduct.AntennaGain.Amount,
            //AntennaGainUnit = technicalproduct.AntennaGain.AntennaGainUnit.ToString()

            await _context.SaveChangesAsync();

            return FullMap(technicalproduct);

            //return new ProductViewModel()
            //{
            //    OutputPower = technicalproduct.OutputPower,
            //    //AntennaGainAmount = techproductinfo.AntennaGainAmount,
            //    //AntennaGainUnit = techproductinfo.AntennaGainUnit
            //};
        }
        #endregion

        #region other information + path to file
        public async Task<ProductViewModel> UpdateProductWithExtraInfo(int productId, ExtraProductViewModel product)
        {
            var selectedProduct = await _context.Products.FindAsync(productId);

            selectedProduct.OtherInformation = product.OtherInformation;
            selectedProduct.PathToFile = product.PathToFile;

            await _context.SaveChangesAsync();

            return FullMap(selectedProduct);
            //{
            //    OtherInformation = selectedProduct.OtherInformation,
            //    PathToFile = selectedProduct.PathToFile
            //};
        }
        #endregion

        #region full information about submitted product
        public async Task<ProductViewModel> GetProductTotalInfo(int productId)
        {
            var product = await _context.Products.FindAsync(productId);

            return FullMap(product);
        }
        #endregion

        #region mapping of product to productVieMmodel
        public ProductViewModel FullMap(Product product)
        {
            return new ProductViewModel()
            {
                Id = product.Id,
                Name = product.Name,
                Model = product.Model,
                Brand = product.Brand,
                Manufacturer = product.Manufacturer,
                OutputPower = product.OutputPower,
                OtherInformation = product.OtherInformation,
                PathToFile = product.PathToFile,
            };
        }
        #endregion
    }
}
