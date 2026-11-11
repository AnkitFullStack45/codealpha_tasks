using Circlely.Data;
using Circlely.Models;
using Circlely.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Circlely.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IWebHostEnvironment environment;

        public HomeController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            IWebHostEnvironment environment)
        {
            this.context = context;
            this.userManager = userManager;
            this.environment = environment;
        }

        // ==========================================
        // HOME / FEED
        // ==========================================

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var currentUser = await userManager.GetUserAsync(User);

        if (currentUser == null)
            {
                return Challenge();
            }

            // ==========================================
            // LOAD POSTS, LIKES AND COMMENTS
            // ==========================================

            var posts = await context.Posts
                .AsNoTracking()
                .Include(p => p.User)
                .Include(p => p.Likes)
                .Include(p => p.Comments)
                    .ThenInclude(c => c.User)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();


            // ==========================================
            // CONVERT POSTS TO VIEW MODELS
            // ==========================================

            var postViewModels = posts.Select(post => new PostViewModel
            {
                Id = post.Id,

                Content = post.caption ?? string.Empty,

                ImageUrl = post.ImageUrl,

                UserId = post.UserId,

                UserName = post.User?.UserName ?? "User",

                ProfileImage = post.User?.ProfileImage,

                CreatedAt = post.CreatedAt,

                LikeCount = post.Likes.Count,

                CommentCount = post.Comments.Count,

                IsLikedByCurrentUser =
                    post.Likes.Any(like =>
                        like.UserId == currentUser.Id),

                // ==========================================
                // LOAD COMMENTS INTO VIEW MODEL
                // ==========================================

                Comments = post.Comments
                    .OrderBy(c => c.CreatedAt)
                    .Select(c => new CommentViewModel
                    {
                        Id = c.Id,

                        PostId = c.PostId,

                        UserId = c.UserId,

                        UserName = c.User?.UserName ?? "User",

                        Content = c.Content,

                        CreatedAt = c.CreatedAt

                    }).ToList()

            }).ToList();


            // ==========================================
            // SEND CURRENT USER TO VIEW
            // ==========================================

            ViewBag.CurrentUser = currentUser;


            return View(postViewModels);

}


        // ==========================================
        // CREATE POST - GET
        // ==========================================

        [Authorize]
        [HttpGet]
        public IActionResult CreatePost()
        {
            return View();
        }


        // ==========================================
        // CREATE POST - POST
        // ==========================================

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePost(
            CreatePostViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var currentUser = await userManager.GetUserAsync(User);

            if (currentUser == null)
            {
                return Challenge();
            }

            string? imageUrl = null;

            // ==========================================
            // IMAGE UPLOAD
            // ==========================================

            if (model.Image != null && model.Image.Length > 0)
            {
                var allowedExtensions = new[]
                {
                    ".jpg",
                    ".jpeg",
                    ".png",
                    ".gif",
                    ".webp"
                };

                var extension =
                    Path.GetExtension(model.Image.FileName)
                        .ToLowerInvariant();

                if (!allowedExtensions.Contains(extension))
                {
                    ModelState.AddModelError(
                        "Image",
                        "Only JPG, JPEG, PNG, GIF and WEBP images are allowed."
                    );

                    return View(model);
                }

                const long maxFileSize = 10 * 1024 * 1024;

                if (model.Image.Length > maxFileSize)
                {
                    ModelState.AddModelError(
                        "Image",
                        "Image size cannot exceed 10 MB."
                    );

                    return View(model);
                }

                var uploadsFolder = Path.Combine(
                    environment.WebRootPath,
                    "uploads",
                    "posts"
                );

                Directory.CreateDirectory(uploadsFolder);

                var fileName =
                    $"{Guid.NewGuid():N}{extension}";

                var filePath =
                    Path.Combine(uploadsFolder, fileName);

                await using var stream =
                    new FileStream(
                        filePath,
                        FileMode.Create
                    );

                await model.Image.CopyToAsync(stream);

                imageUrl =
                    $"/uploads/posts/{fileName}";
            }

            // ==========================================
            // CREATE POST
            // ==========================================

            var post = new Post
            {
                UserId = currentUser.Id,

                caption = string.IsNullOrWhiteSpace(model.Caption)
                    ? null
                    : model.Caption.Trim(),

                ImageUrl = imageUrl,

                CreatedAt = DateTime.UtcNow
            };

            context.Posts.Add(post);

            await context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        // ==========================================
        // LIKE / UNLIKE
        // ==========================================

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleLike(int postId)
        {
            var currentUser =
                await userManager.GetUserAsync(User);

            if (currentUser == null)
            {
                return Unauthorized();
            }

            var postExists =
                await context.Posts
                    .AnyAsync(p => p.Id == postId);

            if (!postExists)
            {
                return NotFound(new
                {
                    success = false,
                    message = "Post not found."
                });
            }

            var existingLike =
                await context.Likes
                    .FirstOrDefaultAsync(l =>
                        l.PostId == postId &&
                        l.UserId == currentUser.Id);

            bool isLiked;

            if (existingLike != null)
            {
                context.Likes.Remove(existingLike);

                isLiked = false;
            }
            else
            {
                var newLike = new Like
                {
                    PostId = postId,

                    UserId = currentUser.Id,

                    CreatedAt = DateTime.UtcNow
                };

                context.Likes.Add(newLike);

                isLiked = true;
            }
            await context.SaveChangesAsync();
            var likeCount =
                await context.Likes
                    .CountAsync(l =>
                        l.PostId == postId);

            return Json(new
            {
                success = true,

                isLiked = isLiked,

                likeCount = likeCount
            });
        }
        // ==========================================
        // ADD COMMENT
        // ==========================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddComment(
            int postId,
            string content)
        {
            if (string.IsNullOrWhiteSpace(content))
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Comment cannot be empty."
                });
            }

            content = content.Trim();

            if (content.Length > 500)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Comment cannot exceed 500 characters."
                });
            }

            // Get logged-in user

            var currentUser = await userManager.GetUserAsync(User);

            if (currentUser == null)
            {
                return Unauthorized();
            }

            // Check post

            var postExists = await context.Posts
                .AnyAsync(p => p.Id == postId);

            if (!postExists)
            {
                return NotFound(new
                {
                    success = false,
                    message = "Post not found."
                });
            }

            // Create comment

            var comment = new Comment
            {
                PostId = postId,

                UserId = currentUser.Id,

                Content = content,

                CreatedAt = DateTime.UtcNow
            };

            context.Comments.Add(comment);

            await context.SaveChangesAsync();

            // Return comment information to frontend

            return Json(new
            {
                success = true,

                comment = new
                {
                    id = comment.Id,

                    postId = comment.PostId,

                    userName = currentUser.UserName,

                    content = comment.Content,

                    createdAt = comment.CreatedAt
                },

                commentCount = await context.Comments
                    .CountAsync(c => c.PostId == postId)
            });
        }

    }
}