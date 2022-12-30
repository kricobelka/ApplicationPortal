using ApplicationPortal.Data;
using ApplicationPortal.Data.Entities;
using ApplicationPortal.Enums;
using ApplicationPortal.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApplicationPortal.Services
{
    public class ProductService
    {
        private readonly ApplicationDbContext _context;
        //private readonly UserManager<User> _userManager;

        public ProductService(ApplicationDbContext context/*бUserManager<User> userManageк*/)
        {
            _context = context;
            //_userManager = userManager;
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


        public async Task<ProductViewModel> CreateProductGeneralInfo(string userId, GenProductViewModel product)
        {
            var newProduct = new Product()
            {
                UserId = userId,
                Name = product.Name,
                Model = product.Model,
                Brand = product.Brand,
                Manufacturer = product.Manufacturer,
                Status = ProductStatus.GeneralInfoSubmitted,
                //why frequencies here?
                Frequencies = new List<Frequency>()
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
            //yes, only first value: var technicalproductt = await _context.Products.Where(q => q.Id == productId && q.Id == product.ProductId).FirstOrDefaultAsync();
            //no: //var technicalproductt = await _context.Products.Where(q => q.Id.Equals(productId) && q.Equals(product)).FirstOrDefaultAsync();
            var technicalproduct = await GetProductPerId(productId);

            //TO-DO;    does the following property change smth"?
            //technicalproduct.Id = productId;
            //обязательно ли след действие? нельзя сразу врнуть ProductViewModel?
            //нельзя потому что мы возвращали в контроллере techproductviewmodel для связ экшенов айдишниками?
            technicalproduct.OutputPower = product.OutputPower;

            //frequencies:
            var frequencyForUpdate = product.Frequencies.Where(q => q.FrequencyId != null).ToList();
            var frequencyForAdd = product.Frequencies.Where(q => q.FrequencyId == null).ToList();

            var frequencyForDelete = technicalproduct.Frequencies
                .Where(q => !frequencyForUpdate.Select(q => q.FrequencyId)
                .Contains(q.FrequencyId))
                .ToList();
            //product.Frequencies = frequences

            foreach (var frequency in frequencyForAdd)
            {
                technicalproduct.Frequencies.Add(new Frequency()
                {
                    StartRange = frequency.StartRange,
                    EndRange = frequency.EndRange,
                    FrequencyUnit = frequency.FrequencyUnit
                });
            }

            //technicalproduct.Frequencies = product.Frequencies.Select(q => new Frequency()
            //{
            //    StartRange = q.StartRange,
            //    EndRange = q.EndRange,
            //    FrequencyUnit = q.FrequencyUnit
            //}).ToList();

            foreach (var frequency in frequencyForDelete)
            {
                technicalproduct.Frequencies.Remove(frequency);
            }

            foreach (var frequency in frequencyForUpdate)
            {
                var frequencyPerId = technicalproduct.Frequencies.SingleOrDefault(q => q.FrequencyId == frequency.FrequencyId);

                frequency.StartRange = frequencyPerId.StartRange;
                frequency.EndRange = frequencyPerId.EndRange;
                frequency.FrequencyUnit = frequencyPerId.FrequencyUnit;
            }   
            //end fre
            if (technicalproduct.AntennaGain == null)
            {
                technicalproduct.AntennaGain = new AntennaGain()
                {
                    ProductId = productId,
                    Amount = product.AntennaGain.Amount,
                    AntennaGainUnit = product.AntennaGain.AntennaGainUnit
                };
            }
            else
            {
                technicalproduct.AntennaGain.Amount = product.AntennaGain.Amount;
                technicalproduct.AntennaGain.AntennaGainUnit = product.AntennaGain.AntennaGainUnit;
            }
  
            if (technicalproduct.Status == ProductStatus.GeneralInfoSubmitted)
            {
                technicalproduct.Status = ProductStatus.TechnicalInfoSubmitted;
            }

            await _context.SaveChangesAsync();

            return FullMap(technicalproduct);
        }
        #endregion

        #region other information + path to file
        public async Task<ProductViewModel> UpdateProductWithExtraInfo(int productId, ExtraProductViewModel product)
        {
            var selectedProduct = await GetProductPerId(productId);

            selectedProduct.OtherInformation = product.OtherInformation;
            selectedProduct.PathToFile = product.PathToFile;

            if (selectedProduct.Status == ProductStatus.TechnicalInfoSubmitted)
            {
                selectedProduct.Status = ProductStatus.ExtraInfoSubmitted;
            }

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

        //getproducts due to userid:
        public async Task<List<ProductViewModel>> GetProducts(string userId)
        {
            //var user = await _userManager.FindByIdAsync(userId);
            //var user = _context.Products.Where(q => q.UserId == userId).ToString();
            //adding string userName into parameter
            var products = await _context.Products.Include(q => q.User).Include(q => q.Frequencies).Include(q => q.AntennaGain)
                .Where(q => q.UserId == userId)
                .Select(q => FullMap(q))
                .ToListAsync();

            return products;
        }

        #region buttons
        public async Task<ProductViewModel> SubmitProduct(int productId)
        {

            var endProduct = await GetProductPerId(productId);

            endProduct.Status = ProductStatus.FinallySubmitted;
            await _context.SaveChangesAsync();
            return FullMap(endProduct);
        }

        public async Task<ProductViewModel> EditProductGenInfo(int productId, GenProductViewModel model)
        {
            //будет содержать вызов 4 методов с условиями (в зависимости от секции эдита)
            var productForEdit = await GetProductPerId(productId);
            productForEdit.Name = model.Name;
            productForEdit.Brand = model.Brand;
            productForEdit.Model = model.Model;
            productForEdit.Manufacturer = model.Manufacturer;

            await _context.SaveChangesAsync();

            return FullMap(productForEdit);
        }

        public async Task CancelProduct(int productId)
        {
            var endProduct = await GetProductPerId(productId);
            _context.Products.Remove(endProduct);
            await _context.SaveChangesAsync();
        }

        #endregion
        #endregion

        private Task<Product> GetProductPerId(int productId)
        {
            //to-do: check
            return _context.Products.Include(q => q.User).Include(q => q.Frequencies).Include(q => q.AntennaGain).SingleOrDefaultAsync(q => q.Id == productId);
        }

        #region mapping of product to productVieMmodel
        private static ProductViewModel FullMap(Product product)
        {
            return new ProductViewModel()
            {
                Id = product.Id,
                UserName = product.User?.UserName,
                Name = product.Name,
                Model = product.Model,
                Brand = product.Brand,
                Manufacturer = product.Manufacturer,
                Frequencies = product.Frequencies.Select(q => new FrequencyViewModel
                {
                    StartRange = q.StartRange,
                    EndRange = q.EndRange,
                    FrequencyUnit = q.FrequencyUnit
                }).ToList(),

                AntennaGain = new AntennaGainViewModel()
                {
                    Amount = product.AntennaGain?.Amount ?? default,
                    AntennaGainUnit = product.AntennaGain?.AntennaGainUnit ?? default
                },

                OutputPower = product.OutputPower,
                OtherInformation = product.OtherInformation,
                PathToFile = product.PathToFile,
                Status = product.Status,
            };
        }
        #endregion

        public async Task<ProductViewModel> GetProductById(int productId)
        {
            var product = FullMap(await GetProductPerId(productId));
            return product;
        }

        #region admin

        public async Task<List<ProductViewModel>> GetProductsAndUser()
        {
            var products = await _context.Products.Include(q => q.User).Include(q => q.Frequencies).Include(q => q.AntennaGain).Select(q => FullMap(q))
                .ToListAsync();
            return products;
        }

        public async Task AcceptProduct(int productId)
        {
            var product = await GetProductPerId(productId);

            //for admin
            if (product.Status == Enums.ProductStatus.FinallySubmitted)
            {
                product.Status = Enums.ProductStatus.ApprovedByAdmin;
            }
            // for user
            else if (product.Status == Enums.ProductStatus.ExtraInfoSubmitted)
            {
                product.Status = Enums.ProductStatus.FinallySubmitted;
            }
            await _context.SaveChangesAsync();
        }

        public async Task<ProductViewModel> EditProductFullInfo(ProductViewModel model)
        {
            var product = await GetProductPerId(model.Id);

            product.Name = model.Name;
            product.Brand = model.Brand;
            product.Model = model.Model;
            product.Manufacturer = model.Manufacturer;
            product.OutputPower = model.OutputPower;

            var frequencyForUpdate = model.Frequencies.Where(q => q.FrequencyId != null).ToList();
            var frequencyForAdd = model.Frequencies.Where(q => q.FrequencyId == null).ToList();

            var frequencyForDelete = product.Frequencies
                .Where(q => !frequencyForUpdate.Select(q => q.FrequencyId)
                .Contains(q.FrequencyId))
                .ToList();
            //product.Frequencies = frequences

            foreach (var frequency in frequencyForAdd)
            {
                product.Frequencies.Add(new Frequency()
                {
                    StartRange = frequency.StartRange,
                    EndRange = frequency.EndRange,
                    FrequencyUnit = frequency.FrequencyUnit
                });
            }

            foreach(var frequency in frequencyForDelete)
            {              
                product.Frequencies.Remove(frequency);
            }


            foreach (var frequency in frequencyForUpdate)
            {
                var frequencyPerId = product.Frequencies.Where(q => q.FrequencyId == frequency.FrequencyId)
                    .SingleOrDefault();
                
                frequency.StartRange = frequencyPerId.StartRange;
                frequency.EndRange = frequencyPerId.EndRange;
                frequency.FrequencyUnit = frequencyPerId.FrequencyUnit;
                
            }
            //id ne nado,тк обновляем сущность
            product.AntennaGain.Amount = model.AntennaGain.Amount;
            product.AntennaGain.AntennaGainUnit = model.AntennaGain.AntennaGainUnit;

            product.OtherInformation = model.OtherInformation;
            product.PathToFile = model.PathToFile;

            await _context.SaveChangesAsync();
            return FullMap(product);
        }
        #endregion

        #region buttons:edit (continue), cancel, submit by user


        #endregion
    }
}
