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
        private readonly CommentService _commentService;

        public ProductController(ProductService productService, UserManager<User> userManager, CommentService commentService)
        {
            _productService = productService;
            _userManager = userManager;
            _commentService = commentService;
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
                    return RedirectToAction("VerifyProductInfoByUser", "Product", new
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

            //ViewBag.ShowApplyChanges = product.Status >= Enums.ProductStatus.ExtraInfoSubmitted;

            if (product.Status >= Enums.ProductStatus.TechnicalInfoSubmitted)
            {
                ViewBag.ShowApplyChanges = true;

                return View(new TechProductViewModel
                {
                    ProductId = product.Id,
                    OutputPower = product.OutputPower,
                    Frequencies = product.Frequencies,
                    AntennaGain = product.AntennaGain,
                });
            }
            else
            {
                ViewBag.ShowApplyChanges = false;
                return View(new TechProductViewModel
                {
                    ProductId = productId,
                    //OutputPower = product.OutputPower,
                    Frequencies = new List<FrequencyViewModel>() { new FrequencyViewModel() }
                    //AntennaGain = product.AntennaGain
                });
            }
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

                if (action == "removeFrequency")
                {
                    if (techProduct.Frequencies.Count < 1)
                    {
                        ModelState.AddModelError("", "There must be at least one frequency");
                        return View(techProduct);
                    }
                    var frequencyAddedLast = techProduct.Frequencies.Last();
                    techProduct.Frequencies.Remove(frequencyAddedLast);

                    return View(techProduct);
                }

                var productForEdit = await _productService.UpdateProductWithTechnicalInfo(techProduct.ProductId, techProduct);

                if (action == "submitEditedTechnicalInfo")
                {
                    return RedirectToAction("VerifyProductInfoByUser", "Product", new { productId = productForEdit.Id });
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

            if (product.Status >= Enums.ProductStatus.ExtraInfoSubmitted)
            {
                ViewBag.ShowApplyChanges = true;

                return View(new ExtraProductViewModel
                {
                    ProductId = product.Id,
                    OtherInformation = product.OtherInformation,
                    PathToFile = product.PathToFile
                });
            }

            else
            {
                ViewBag.ShowApplyChanges = false;
                return View(new ExtraProductViewModel
                {
                    ProductId = productId
                });
                //ViewBag.ShowApplyChanges = product.Status >= Enums.ProductStatus.ExtraInfoSubmitted;
                //return View(new ExtraProductViewModel { ProductId = productId });
            }
        }

        [HttpPost]
        public async Task<IActionResult> ExtraProductInfo(ExtraProductViewModel product, string action)
        {
            if (ModelState.IsValid)
            {
                var productForEditOrSubmit = await _productService.UpdateProductWithExtraInfo(product.ProductId, product);
                if (action == "submitEditedExtraInfo")
                {
                    return RedirectToAction("VerifyProductInfoByUser", "Product", new { productId = productForEditOrSubmit.Id });
                }
                return RedirectToAction("VerifyProductInfoByUser", "Product", new { productId = productForEditOrSubmit.Id });
                //return to VerificationOfInsertedDatePage
            }
            return View(product);
        }

        [HttpGet]
        public async Task<IActionResult> VerifyProductInfoByUser(int productId)
        {
            var result = await _productService.GetProductTotalInfo(productId);            
            return View(result);
        }

        //сделать 4  баттона (save as a draft, submit (submittedproducts), cancel, t)
        [HttpPost]
        public async Task<IActionResult> VerifyProductInfoByUser(int productId, string action)
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
            var product = await _productService.GetProductById(productId);
            await _productService.SubmitProduct(productId);
            TempData["submitProduct"] = $"The product {product.Name}, {product.Model} has been submitted under Ref.No. {productId}";
            return RedirectToAction("GetSubmittedProducts", "Product");
        }

        #region buttons
        [HttpGet]
        //id to parameter
        public async Task<IActionResult> GetSubmittedProducts()
        {
            var userId = _userManager.GetUserId(User);
            var allProducts = await _productService.GetProducts(userId);
            return View(allProducts);
        }

        //public async Task<IActionResult> EditProduct(int productId)
        //{
        //    var product = await _productService.EditProduct(productId);
        //    return View(product);
        //
        private async Task<IActionResult> SaveDraftAndRedirectToProducts()
        {
            //how? transfer id? var productId = _productService.GetProductById()
            //TempData["saveProduct"] = $"The product with {productId} has been saved in the drafts";
            return RedirectToAction("GetSubmittedProducts", "Product");
        }

        private async Task<IActionResult> CancelProduct(int productId)
        {
            await _productService.CancelProduct(productId);

            TempData["deleteProduct"] = $"The product with {productId} has been deleted";

            return RedirectToAction("GetSubmittedProducts", "Product");
        }

        #region buttons by user(cancel, continue, view)
        public async Task<IActionResult> ViewApplicationNotSubmitted(int productId)
        {
            var application = await _productService.GetProductById(productId);
            var comments = await _commentService.GetCommentsPerProductId(productId);
            ViewBag.Comments = comments;
            return View(application);
        }

        public async Task<IActionResult> CancelNotSubmittedApplication(int productId)
        {
            await _productService.CancelProduct(productId);

            return RedirectToAction("GetSubmittedProducts", "Product");
        }

        public async Task<IActionResult> SubmitSavedProduct(int productId)
        {
            if (ModelState.IsValid)
            {
                await _productService.AcceptProduct(productId);
                return RedirectToAction("GetSubmittedProducts", "Product");
            }
            return View();
        }

        public async Task<IActionResult> EditNotSubmittedProduct(int productId)
        {
            if (ModelState.IsValid)
            {
                var product = await _productService.GetProductById(productId);

                if (product.Status == Enums.ProductStatus.GeneralInfoSubmitted)
                {
                    return RedirectToAction("TechnicalProductInfo", new { productId = product.Id });
                }

                else if (product.Status == Enums.ProductStatus.TechnicalInfoSubmitted)
                {
                    return RedirectToAction("ExtraProductInfo", new { productId = product.Id });
                }

                else if (product.Status > Enums.ProductStatus.ExtraInfoSubmitted)
                {
                    return RedirectToAction("VerifyProductInfoByUser", "Product", new { productId = product.Id });
                }

                else
                {
                    return RedirectToAction("ExtraProductInfo", new { productId = product.Id });
                }
            }
            return View();
            //    if (product.Frequencies.Any() == false)
            //    {
            //        product.Frequencies.Add(new FrequencyViewModel());
            //    }
            //    return View(product);
            //}

            //[HttpPost]
            //public async Task<IActionResult> EditNotSubmittedProduct(ProductViewModel product, string action)
            //{
            //    if (action == "addNewFrequency")
            //    {
            //        if (product.Frequencies.Count > 10)
            //        {
            //            ModelState.AddModelError("", "The number of frequency models must be no more than 10");
            //            return View(product);
            //        }

            //        product.Frequencies.Add(new FrequencyViewModel());
            //        return View(product);
            //    }

            //    if (action == "removeFrequency")
            //    {
            //        if (product.Frequencies.Count < 1)
            //        {
            //            ModelState.AddModelError("", "There must be at least one frequency");
            //            return View(product);
            //        }
            //        var frequencyAddedLast = product.Frequencies.Last();
            //        product.Frequencies.Remove(frequencyAddedLast);

            //        return View(product);
            //    }

            //    await _productService.EditProductFullInfo(product);

            //    return RedirectToAction("ViewSubmittedProduct", new { productId = product.Id });
            //}
            #endregion
        }
    }
}
#endregion