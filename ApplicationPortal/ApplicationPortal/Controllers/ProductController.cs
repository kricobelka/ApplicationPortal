using ApplicationPortal.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApplicationPortal.Services;
using ApplicationPortal.Models;
using ApplicationPortal.Data.Entities;

namespace ApplicationPortal.Controllers
{
    public class ProductController : Controller
    {
        private readonly ProductService _productService;

        public ProductController(ProductService productService)
        {
            _productService = productService;
        }

        //выводим информацию о залогиненном юзере (мэйл, название компаний)
        //для проверки юзером

        [HttpGet]
        public async Task<IActionResult> GetUserAndCompanyInfo()
        {
            var userAndCompany = await _productService.GetUserAndCompanyInfo(User.Identity.Name);
            // мы имеем доступ к юзеру через User.Identity., т.к есть созданное энтити юзер?
            //к ролям не могу так же обратиться
            return View(userAndCompany);
        }
        
        [HttpPost]
        public async Task<IActionResult> VerifyUserAndGoNext()
        {
            return RedirectToAction("GetGeneralProductInfo", "Product");
        }

        [HttpGet]
        public async Task<IActionResult> GetGeneralProductInfo()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateProductGeneralInfo(GenProductViewModel product)
        {
            
            var result = await _productService.CreateProductGeneralInfo(product);
            return RedirectToAction("GetTechProductInfo", "Product", new { productId = result.Id });
        }

        [HttpGet]
        public async Task<IActionResult> GetTechProductInfo (int productId)
        {
            return View(new TechProductViewModel { ProductId = productId });
        }

        [HttpPost]
        //почему мы называем вьюшку только именем метода гет? почему не берем названия метода пост?
        public async Task<IActionResult> PostTechnicalProductInfo(TechProductViewModel techProduct)
        {
            var result = await _productService.UpdateProductWithTechnicalInfo(techProduct.ProductId, techProduct);
            return RedirectToAction("GetExtraProductInfo", "Product", new { productId = result.Id });
        }

        //не передаем еще и айдишникв параметр экшена, т.к во вью можно передать одну модель?
        [HttpGet]
        public async Task<IActionResult> GetExtraProductInfo (int productId)
        {
            return View(new ExtraProductViewModel { ProductId = productId });
        }

        [HttpPost]
        public async Task<IActionResult> PostExtraProductInfo (ExtraProductViewModel product)
        {
            var result = await _productService.UpdateProductWithExtraInfo(product.ProductId, product);
            return RedirectToAction("GetSubmittedProductInfo", "Product", new { productId = result.Id });
            //return to VerificationOfInsertedDatePage
        }

        [HttpGet]
        public async Task<IActionResult> VerifyProductInfo(int productId)
        {
            var result = await _productService.GetProductTotalInfo(productId);
            return View(result);
        }

        //сделать 4  баттона (save as a draft, submit (submittedproducts), cancel, edit)
        [HttpPost]
        public async Task<IActionResult> PostVerifiedProduct(int productId)
        {
            Product product = new Product();
            var result = await _productService.GetProductTotalInfo(productId);

            if (product.Id == productId && product.Status == "TotallySubmitted")
            {
                //создавать дяля этой цели отдельный контроллер?
                return RedirectToAction("SubmittedProducts", "Product");
            }

            else if (product.Id == productId && product.Status == "Cancelled")
            {
                await _context.Products.RemoveAsync(result);
                await _context.SaveChangesAsync();
            }
            
            
        }
    }
}