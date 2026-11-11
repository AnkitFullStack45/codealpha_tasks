using Circlely.Data;
using Circlely.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Circlely.Controllers
{
    [Authorize]
    public class CommentController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CommentController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(
            int postId,
            string content)
        {
            // Check comment content
            if (string.IsNullOrWhiteSpace(content))
            {
                TempData["CommentError"] = "Comment cannot be empty.";

                return RedirectToAction("Index", "Home");
            }

            if (content.Length > 500)
            {
                TempData["CommentError"] =
                    "Comment cannot exceed 500 characters.";

                return RedirectToAction("Index", "Home");
            }

            // Check whether post exists
            var post = await _context.Posts
                .FirstOrDefaultAsync(p => p.Id == postId);

            if (post == null)
            {
                return NotFound();
            }

            // Get currently logged-in user
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return Challenge();
            }

            // Create comment
            var comment = new Comment
            {
                Content = content.Trim(),

                PostId = postId,

                UserId = user.Id,

                CreatedAt = DateTime.UtcNow
            };

            // Save comment
            _context.Comments.Add(comment);

            await _context.SaveChangesAsync();

            // Redirect back to feed
            return RedirectToAction("Index", "Home");
        }
    }
}