using ApplicationPortal.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApplicationPortal.Services;
using ApplicationPortal.Models;
using ApplicationPortal.Data.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Security.Claims;

namespace ApplicationPortal.Controllers
{
    public class ProductController : Controller
    {
        private readonly ProductService _productService;
        private readonly UserManager<User> _userManager;

        public ProductController(ProductService productService, UserManager<User> userManager)
        {
            _productService = productService;
            _userManager = userManager; 
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
        public async Task<IActionResult> GeneralProductInfo(int productId)
        {
            if (productId == 0)
            {
                ViewBag.ShowApplyChanges = false;
                return View();
            }

            var product = await _productService.GetProductById(productId);
            ViewBag.ShowApplyChanges = true;
            return View(new GenProductViewModel()
            {
                //product.Id - что в этом случае бы было
                ProductId = productId,
                Name = product.Name,
                Brand = product.Brand,
                Manufacturer = product.Manufacturer,
                Model = product.Model
            });
        }

        [HttpPost]
        public async Task<IActionResult> GeneralProductInfo(GenProductViewModel product, string action)
        {
            if (ModelState.IsValid)
            {
                if (product.ProductId == null)
                {
                    var userId = _userManager.GetUserId(User);
                    //показывает мэйл:
                    // userId = _userManager.GetUserName(User);
                    var result = await _productService.CreateProductGeneralInfo(userId, product);
                    return RedirectToAction("TechnicalProductInfo", "Product", new { productId = result.Id });
                }
                var productForEdit = await _productService.EditProductGenInfo(product.ProductId.Value, product);

                if (action == "submitEditedInfo")
                {
                    return RedirectToAction("VerifyProductInfo", "Product", new
                    {
                        productId = productForEdit.Id
                    });
                }

                return RedirectToAction("TechnicalProductInfo", "Product", new { productId = productForEdit.Id });
            }
            //hasvalue определяет налл ли свойство, value его получает
            return View(product);
        }

        [HttpGet]
        public async Task<IActionResult> TechnicalProductInfo(int productId)
        {

            var product = await _productService.GetProductById(productId);
            //if (product.Status == Enums.ProductStatus.GeneralInfoSubmitted)
            //{
            //    ViewBag.ShowApplyChanges = false;
            //}
            ViewBag.ShowApplyChanges = product.Status >= Enums.ProductStatus.ExtraInfoSubmitted;

            var viewModelPerId = new TechProductViewModel { ProductId = productId };
            viewModelPerId.Frequencies.Add(new FrequencyViewModel());
            return View(viewModelPerId);
        }

        [HttpPost]
        //почему мы называем вьюшку только именем метода гет? почему не берем названия метода пост?
        public async Task<IActionResult> TechnicalProductInfo(TechProductViewModel techProduct, string action)
        {
            if (ModelState.IsValid)
            {
                if (action == "goToNextPage")
                {
                    var productForSubmit = await _productService.UpdateProductWithTechnicalInfo(techProduct.ProductId, techProduct);
                    return RedirectToAction("ExtraProductInfo", "Product", new { productId = productForSubmit.Id });
                }

                if (action == "addNewFrequency")
                {
                    if (techProduct.Frequencies.Count > 10)
                    {
                        ModelState.AddModelError("", "The number of frequency models must be no more than 10");
                        return View(techProduct);
                    }
                    techProduct.Frequencies.Add(new FrequencyViewModel());
                    return View(techProduct);
                }

                var productForEdit = await _productService.UpdateProductWithTechnicalInfo(techProduct.ProductId, techProduct);
                if (action == "submitEditedTechnicalInfo")
                {
                    return RedirectToAction("VerifyProductInfo", "Product", new { productId = productForEdit.Id });
                }

                //todo: change parameter
                return RedirectToAction("ExtraProductInfo", "Product", new { productId = productForEdit.Id });
            }
            return View(techProduct);
        }

        //не передаем еще и айдишникв параметр экшена, т.к во вью можно передать одну модель?
        [HttpGet]
        public async Task<IActionResult> ExtraProductInfo(int productId)
        {
            var product = await _productService.GetProductById(productId);
            ViewBag.ShowApplyChanges = product.Status >= Enums.ProductStatus.ExtraInfoSubmitted;
            return View(new ExtraProductViewModel { ProductId = productId });
        }

        [HttpPost]
        public async Task<IActionResult> ExtraProductInfo(ExtraProductViewModel product, string action)
        {
            if (ModelState.IsValid)
            {
                var productForEditOrSubmit = await _productService.UpdateProductWithExtraInfo(product.ProductId, product);
                if (action == "submitEditedExtraInfo")
                {
                    return RedirectToAction("VerifyProductInfo", "Product", new { productId = productForEditOrSubmit.Id });
                }
                return RedirectToAction("VerifyProductInfo", "Product", new { productId = productForEditOrSubmit.Id });
                //return to VerificationOfInsertedDatePage
            }
            return View(product);
        }

        [HttpGet]
        public async Task<IActionResult> VerifyProductInfo(int productId)
        {
            var result = await _productService.GetProductTotalInfo(productId);
            return View(result);
        }

        //сделать 4  баттона (save as a draft, submit (submittedproducts), cancel, edit)
        [HttpPost]
        public async Task<IActionResult> VerifyProductInfo(int productId, string action)
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
        //id to parameter
        public async Task<IActionResult> GetSubmittedProducts(string userId)
        {
            var allProducts = await _productService.GetProducts(userId);
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
    }
}
#endregion