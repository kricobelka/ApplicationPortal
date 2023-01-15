using ApplicationPortal.Data;
using ApplicationPortal.Data.Entities;
using ApplicationPortal.Enums;
using ApplicationPortal.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace ApplicationPortal.Services
{
    public class CommentService
    {
        private readonly ApplicationDbContext _context;

        public CommentService (ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<CommentViewModelResponse>> GetCommentsPerProductId(int productId)
        {
            //var product = await _context.Products.Where(q => q.Id == productId);

            var comments = await _context.Comments.Where(q => q.ProductId == productId)
                .Select(q => new CommentViewModelResponse
                {
                    FirstName = q.User.FirstName,
                    LastName = q.User.LastName,
                    UserEmail = q.User.Email,
                    Text = q.Text,
                    CreatedDate = q.CreatedDate,
                }).ToListAsync();

            return comments;
        }

        public async Task<CommentViewModelResponse> CreateComment(CommentViewModelRequest modelRequest, string userId)
        {
            var user = await _context.Users.FindAsync(userId);

            Comment comment = new Comment
            {
                ProductId = modelRequest.ProductId,
                Text = modelRequest.Text,
                CreatedDate = DateTime.Now,
                UserId = user.Id,
            };

            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();

            return new CommentViewModelResponse()
            {
                ProductId = comment.ProductId,
                Text = comment.Text,
                CreatedDate = comment.CreatedDate,
                FirstName = comment.User.FirstName,
                LastName = comment.User.LastName,
                UserEmail = comment.User.Email
            };
        }
    }
}
