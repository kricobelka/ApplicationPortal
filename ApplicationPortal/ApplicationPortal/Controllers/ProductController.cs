using ApplicationPortal.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApplicationPortal.Services;
using ApplicationPortal.Models;

namespace ApplicationPortal.Controllers
{
    public class ProductController : Controller
    {
        private readonly ProductService _productService;

        public ProductController(ProductService productService)
        {
            _productService = productService;
        }

        //выводим информацию о залогиненном юзере (мэйл, имя  и название компаний)
        //для проверки юзером и выбора (компании выбираем, мэйл  и имя проверяем)

        //[HttpGet]
        //public async Task<IActionResult> GetUserAndCompanyInfo(int userId)
        //{
        //    var userCompanies = await _productService.GetUserAndCompanyInfo(userId);
        //    return View(userCompanies);
        //}
        
        //[HttpPost]
        //public async Task<IActionResult> GetUserAndCompanyInfo(int userId, UserCompaniesViewModel viewModel)
        //{
        //    await _productService.UsersAndCompanyPost(userId, viewModel);
        //    return RedirectToAction("GetGeneralProductInfo", "Product");
        //}

        //[HttpGet]
        //public async Task<IActionResult> GetGeneralProductInfo()
        //{
        //    var generalInfo = await _productService.GetGeneralProductInfo();
        //    return View(generalInfo);
        //}
    }
}