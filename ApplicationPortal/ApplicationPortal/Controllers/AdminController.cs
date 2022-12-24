﻿using ApplicationPortal.Data;
using ApplicationPortal.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationPortal.Controllers
{
    public class AdminController : Controller
    {
        private readonly ProductService _productService;

        public AdminController(ProductService productService)
        {
            _productService = productService;
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
            return View(product);
        }

        //можно так или лучше контекст добавлять и через него выполнять?
        public async Task<IActionResult> DeleteProduct(int productId)
        {
            var product = await _productService.GetProductById(productId);
            //можно ли вызывать cancelrpdocut в этом случае? (он ничего не возвращает)
            await _productService.CancelProduct(product.Id);
            return RedirectToAction("GetSubmittedProductsAndUsers", "Admin");
        }

        // можно ил сделать как-нить без добавления appcontext DI?
        public async Task<IActionResult> AcceptProduct(int productId)
        {
            var product = await _productService.GetProductById(productId);
            if (product.Status == Enums.ProductStatus.FinallySubmitted)
            {
                product.Status = Enums.ProductStatus.ApprovedByAdmin;
            }
            await _productService.SaveChanges();
            return RedirectToAction("GetSubmittedProductsAndUsers", "Admin");
        }

        //вместо нео можно добавить комменты, юзер будет сам исправлять, админ нет
        //если нет, в один метод добавлять все модели и вызыывать методы?
        //public async Task<IActionResult> EditProduct(int productId)
        //{
        //    var product = await _productService.GetProductById(productId);
        //    await _productService.EditProductGenInfo()
        //}
        #endregion

    }
}