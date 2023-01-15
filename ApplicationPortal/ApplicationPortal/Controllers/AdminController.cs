using ApplicationPortal.Data;
using ApplicationPortal.Models;
using ApplicationPortal.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationPortal.Controllers
{
    public class AdminController : Controller
    {
        private readonly ProductService _productService;
        private readonly CommentService _commentService;

        public AdminController(ProductService productService, CommentService commentService)
        {
            _productService = productService;
            _commentService = commentService;
        }

        public IActionResult Index()
        {
            return View();
        }

        #region adminpage

        public async Task<IActionResult> GetSubmittedProductsAndUsers()
        {
            var allProducts = await _productService.GetProductsAndUser();
            return View(allProducts);
        }

        public async Task<IActionResult> ViewSubmittedProduct(int productId)
        {
            var product = await _productService.GetProductById(productId);
            var comments = await _commentService.GetCommentsPerProductId(productId);
            ViewBag.Comments = comments;
            return View(product);
        }

        //можно так или лучше контекст добавлять и через него выполнять?
        public async Task<IActionResult> DeleteProduct(int productId)
        {
            //var product = await _productService.GetProductById(productId);
            //можно ли вызывать cancelrpdocut в этом случае? (он ничего не возвращает)
            await _productService.CancelProduct(productId);
            
            return RedirectToAction("GetSubmittedProductsAndUsers", "Admin");
        }

        // можно ил сделать как-нить без добавления appcontext DI?
        public async Task<IActionResult> AcceptProduct(int productId)
        {
            await _productService.AcceptProduct(productId);
            return RedirectToAction("GetSubmittedProductsAndUsers", "Admin");
        }

        //вместо нео можно добавить комменты, юзер будет сам исправлять, админ нет
        //если нет, в один метод добавлять все модели и вызыывать методы?
        [HttpGet]
        public async Task<IActionResult> EditProduct(int productId)
        {
            var product = await _productService.GetProductById(productId);
            if (product.Frequencies.Any() == false)
            {
                product.Frequencies.Add(new FrequencyViewModel());
            }
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> EditProduct(ProductViewModel product, string action)
        {
            if (action == "addNewFrequency")
            {
                if (product.Frequencies.Count > 10)
                {
                    ModelState.AddModelError("", "The number of frequency models must be no more than 10");
                    return View(product);
                }

                product.Frequencies.Add(new FrequencyViewModel());
                return View(product);
            }

            if (action == "removeFrequency")
            {
                if (product.Frequencies.Count < 1)
                {
                    ModelState.AddModelError("", "There must be at least one frequency");
                    return View(product);
                }
                var frequencyAddedLast = product.Frequencies.Last();
                product.Frequencies.Remove(frequencyAddedLast);

                return View(product);
            }

            await _productService.EditProductFullInfo(product);
            
            return RedirectToAction("ViewSubmittedProduct", new {productId = product.Id});
        } 
        #endregion
    }
}