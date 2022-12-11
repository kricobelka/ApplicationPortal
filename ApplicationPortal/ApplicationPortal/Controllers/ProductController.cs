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
            return View(userAndCompany);
        }

        [HttpPost]
        public async Task<IActionResult> VerifyUserAndGoNext()
        {
            return RedirectToAction("GetGeneralProductInfo", "Product"); 
                //new {});
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
            return RedirectToAction("GetTechProductInfo", "Product", new {productId = result.Id});
        }

        [HttpGet]
        public async Task<IActionResult> GetTechProductInfo (int productId)
        {
            return View(new TechProductViewModel { ProductId = productId });
        }

        [HttpPost]
        public async Task<IActionResult> PostProductInfo(TechProductViewModel techProduct)
        {
            await _productService.PostTechnicalProductInfo(techProduct);
            return RedirectToAction("GetExtraInfo", "Product", new { id = techProduct.ProductId })
        }

    }
}