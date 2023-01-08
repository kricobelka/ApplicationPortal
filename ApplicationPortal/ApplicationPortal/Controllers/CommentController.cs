using ApplicationPortal.Data.Entities;
using ApplicationPortal.Models;
using ApplicationPortal.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationPortal.Controllers
{
    public class CommentController : Controller
    {
        private readonly CommentService _commentService;
        
        private readonly UserManager<User> _userManager;

        public CommentController(CommentService commentService, UserManager<User> userManager)
        {
            _commentService = commentService;
            _userManager = userManager;
        }

        //public async Task<IActionResult> GetComments(int productId)
        //{
        //    var comments = await _commentService.GetCommentsPerProductId(productId);
        //    return View(comments);
        //}

        [HttpGet]
        public async Task<IActionResult> CreateAComment(int productId, bool isAdmin)
        {
            ViewBag.IsAdmin = isAdmin;

            //bool value засунуть во вь.бэг, на вюьхе достват значение вьюбэга )isadmin) if isadmin == true redirect to adminproduct info, false - userproductinfo
            //create modelresponse with productId and return to View
            var userId = _userManager.GetUserId(User);

            var user = await _userManager.FindByIdAsync(userId);

            return View(new CommentViewModelResponse
            {
                ProductId = productId,
                UserEmail = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                CreatedDate = DateTime.Now,
            });
        }

        [HttpPost]
        public async Task<IActionResult> CreateAComment(CommentViewModelRequest modelRequest, bool isAdmin)
        {
            if (!ModelState.IsValid)
            {
                var userId = _userManager.GetUserId(User);

                //bool == true, когда админ оставляет комменты на админ-
                //false, когда юзер будет оставлять комменты на своей части
                await _commentService.CreateComment(modelRequest, userId);

                if (isAdmin == true)
                {
                    return RedirectToAction("ViewSubmittedProduct", "Admin", new { ProductId = modelRequest.ProductId });
                }
                return RedirectToAction("ViewApplicationNotSubmitted", "Product", new { ProductId = modelRequest.ProductId });
            }

            return View(modelRequest);
        }
    }
}
