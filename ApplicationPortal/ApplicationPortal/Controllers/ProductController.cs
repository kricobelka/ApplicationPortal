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
            return RedirectToAction("GeneralProductInfo", "Product");
        }

        [HttpGet]
        public async Task<IActionResult> GeneralProductInfo()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GeneralProductInfo(GenProductViewModel product)
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
            return RedirectToAction("VerifyProductInfo", "Product", new { productId = result.Id });
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
        public async Task<IActionResult> PostVerifiedProduct(int productId, string action)
        {
            //не совсем верно: нужно просто вернуть айдишник
            //var product = await _productService.GetProductTotalInfo(productId);

            if (action == "getSubmittedProducts")
            {
                return await SubmitProduct(productId);
            }

            //else if (action == "editProduct")
            //{
            //    return await EditProduct(productId);
            //}

            else if (action == "cancelProduct")
            {
                return await CancelProduct(productId);
            }

            else if (action == "saveProduct")
            {
                return await SaveDraftAndRedirectToProducts();
            }
            //is productviewmodel necessary as a parameter and can I return View()?
            return View();
        }

        private async Task<IActionResult> SubmitProduct(int productId)
        {
            await _productService.SubmitProduct(productId);
            return RedirectToAction("GetSubmittedProducts", "Product");
        }

        #region buttons
        [HttpGet]
        public async Task<IActionResult> GetSubmittedProducts()
        {
            var allProducts = await _productService.GetProducts();
            return View(allProducts);
        }

        //public async Task<IActionResult> EditProduct(int productId)
        //{
        //    var product = await _productService.EditProduct(productId);
        //    return View(product);
        //}
        private async Task<IActionResult> SaveDraftAndRedirectToProducts()
        {
            return RedirectToAction("GetSubmittedProducts", "Product");
        }

        private async Task<IActionResult> CancelProduct(int productId)
        {
            await _productService.CancelProduct(productId);
            //ViewBag send to view. which view?
            return RedirectToAction("GetSubmittedProducts", "Product");
            //return View()?
        }



        //Product product = new Product();
        //var result = await _productService.GetProductTotalInfo(productId);

        //if (product.Id == productId && product.Status == "TotallySubmitted")
        //{
        //    //создавать дяля этой цели отдельный контроллер?
        //    return RedirectToAction("SubmittedProducts", "Product");
        //}

        //else if (product.Id == productId && product.Status == "Cancelled")
        //{
        //    await _context.Products.RemoveAsync(result);
        //    await _context.SaveChangesAsync();
        //}

    }
}
#endregion