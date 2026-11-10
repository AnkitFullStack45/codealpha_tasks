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
    public class ProfileController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _environment;
        public ProfileController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager, IWebHostEnvironment environment, SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _environment = environment;
            _signInManager = signInManager;

        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Get user's posts
            var posts = await _context.Posts
                .Where(p => p.UserId == user.Id)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();

            // Count user's posts
            var postCount = posts.Count;

            // Count followers
            var followerCount = await _context.Follows
                .CountAsync(f => f.FollowingId == user.Id);

            // Count following
            var followingCount = await _context.Follows
                .CountAsync(f => f.FollowerId == user.Id);

            var model = new ProfileViewModel
            {
                Id = user.Id,
                Email = user.Email ?? string.Empty,
                Bio = user.Bio,
                ProfileImage = user.ProfileImage,
                CreatedAt = user.createdAt,
                IsPrivate = user.IsPrivate,

                PostCount = postCount,
                FollowerCount = followerCount,
                FollowingCount = followingCount,

                Posts = posts
            };

            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var model = new EditProfileViewModel
            {
                Email = user.Email ?? string.Empty,
                Bio = user.Bio,
                ProfileImage = user.ProfileImage
            };

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Update email
            user.Email = model.Email;
            user.UserName = model.Email;

            // Update bio
            user.Bio = model.Bio;

            // Upload profile image
            if (model.Image != null && model.Image.Length > 0)
            {
                string uploadsFolder = Path.Combine(
                    _environment.WebRootPath,
                    "uploads",
                    "profiles"
                );

                Directory.CreateDirectory(uploadsFolder);

                string extension = Path.GetExtension(model.Image.FileName);

                string fileName = Guid.NewGuid().ToString() + extension;

                string filePath = Path.Combine(
                    uploadsFolder,
                    fileName
                );

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.Image.CopyToAsync(stream);
                }

                user.ProfileImage = "/uploads/profiles/" + fileName;
            }

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Profile");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Login", "Account");
        }
    }
}