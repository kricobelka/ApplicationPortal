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
                Manufacturer = product.Manufacturer,
                Status = "GeneralInfoSubmitted"
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
            var technicalproduct = await GetProductPerId(productId);

            //обязательно ли след действие? нельзя сразу врнуть ProductViewModel?
            //нельзя потому что мы возвращали в контроллере techproductviewmodel для связ экшенов айдишниками?
            technicalproduct.OutputPower = product.OutputPower;
            technicalproduct.Status = "TechnicalInfoSubmitted";
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
            var selectedProduct = await GetProductPerId(productId);

            selectedProduct.OtherInformation = product.OtherInformation;
            selectedProduct.PathToFile = product.PathToFile;
            selectedProduct.Status = "ExtraInfoSubmitted";

            await _context.SaveChangesAsync();

            return FullMap(selectedProduct);
        }
        #endregion

        #region full information about submitted product
        public async Task<ProductViewModel> GetProductTotalInfo(int productId)
        {
            var product = await GetProductPerId(productId);
            return FullMap(product);
        }

        #region buttons
        public async Task<List<ProductViewModel>> GetProducts()
        {
            var products = await _context.Products.Select(q => FullMap(q))
                .ToListAsync();

            return products;
        }

        #endregion
        public async Task<ProductViewModel> SubmitProduct(int productId)
        {
          
            var endProduct = await GetProductPerId(productId);

            endProduct.Status = "FinallySubmitted";
            await _context.SaveChangesAsync();
            return FullMap(endProduct);
        }

        //public async Task<ProductViewModel> EditProduct(int productId)
        //{
        //    //будет содержать вызов 4 методов с условиями (в зависимости от секции эдита)
        //    var productForEdit = await GetProductPerId(productId);
        //    productForEdit.Id = productModel.
        //    productForEdit.Name = 
        //}

        public async Task CancelProduct (int productId)
        {
            var endProduct = await GetProductPerId(productId);
            _context.Products.Remove(endProduct);
            await _context.SaveChangesAsync();
        }

        #endregion
        //unnecessary, action is enough
        private Task<Product> GetProductPerId(int productId)
        {
            return _context.Products.FindAsync(productId).AsTask();
        }

        #region mapping of product to productVieMmodel
        private static ProductViewModel FullMap(Product product)
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
                Status = product.Status,
                //add antenna gain frequency etc.
            };
        }
        #endregion
    }
}
