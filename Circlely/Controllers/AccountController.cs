using Circlely.Models;
using Circlely.Services;
using Circlely.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;

namespace Circlely.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IEmailService emailService;

        public AccountController(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager, IEmailService emailService)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.emailService = emailService;
        }

        // =========================================================
        // LOGIN - GET
        // =========================================================

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            // If already logged in, don't show login page again
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Index", "Home");
            }

            ViewData["ReturnUrl"] = returnUrl;

            return View();
        }


        // =========================================================
        // LOGIN - POST
        // =========================================================

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(
            LoginViewModel model,
            string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (!ModelState.IsValid)
            {
                return View(model);
            }


            // Find user using email
            var user = await userManager.FindByEmailAsync(model.Email);


            if (user == null)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Invalid email or password.");

                return View(model);
            }


            // Attempt login
            var result = await signInManager.PasswordSignInAsync(
                user.UserName!,
                model.Password,
                model.RememberMe,
                lockoutOnFailure: true);


            // =====================================================
            // LOGIN SUCCESS
            // =====================================================

            if (result.Succeeded)
            {
                if (!string.IsNullOrEmpty(returnUrl) &&
                    Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }

                return RedirectToAction("Index", "Home");

            }


            // =====================================================
            // EMAIL NOT CONFIRMED
            // =====================================================

            if (result.IsNotAllowed)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Please confirm your email address before logging in.");

                return View(model);
            }


            // =====================================================
            // LOCKED OUT
            // =====================================================

            if (result.IsLockedOut)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Your account is temporarily locked. Please try again later.");

                return View(model);
            }


            // =====================================================
            // INVALID LOGIN
            // =====================================================

            ModelState.AddModelError(
                string.Empty,
                "Invalid email or password.");

            return View(model);
        }


        // =========================================================
        // LOGOUT
        // =========================================================

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();

            return RedirectToAction("Login", "Account");
        }


        // =========================================================
        // ACCESS DENIED
        // =========================================================

        [AllowAnonymous]
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
            [HttpGet]
            public IActionResult Register()
            {
                return View();
            }
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email
            };

            var result = await userManager.CreateAsync(
                user,
                model.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(
                        string.Empty,
                        error.Description);
                }

                return View(model);
            }

            // Generate email confirmation token
            var token =
                await userManager.GenerateEmailConfirmationTokenAsync(user);

            // Encode token so it is safe inside the URL
            var encodedToken =
                WebEncoders.Base64UrlEncode(
                    Encoding.UTF8.GetBytes(token));

            // Create confirmation link
            var confirmationLink = Url.Action(
                "ConfirmEmail",
                "Account",
                new
                {
                    userId = user.Id,
                    token = encodedToken
                },
                protocol: HttpContext.Request.Scheme);

            // Send confirmation email
            bool emailSent = emailService.SendMail(
                user.Email!,
                "Confirm your Circlely account",
                $"""
        <h2>Welcome to Circlely!</h2>

        <p>
            Thank you for creating your Circlely account.
        </p>

        <p>
            Please confirm your email address by clicking
            the button below:
        </p>

        <p>
            <a href="{confirmationLink}"
               style="
                   display:inline-block;
                   padding:12px 20px;
                   background:#6c5ce7;
                   color:white;
                   text-decoration:none;
                   border-radius:8px;
               ">
                Confirm Email
            </a>
        </p>

        <p>
            If you did not create this account, you can
            safely ignore this email.
        </p>
        """
            );

            if (!emailSent)
            {
                // Remove the newly created account because
                // the user cannot verify it without the email.
                await userManager.DeleteAsync(user);

                ModelState.AddModelError(
                    string.Empty,
                    "We could not send the confirmation email. Please try again.");

                return View(model);
            }

            // Email was successfully sent
            return RedirectToAction(
                "RegisterConfirmation",
                new
                {
                    email = user.Email
                });
        }
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> ConfirmEmail( string userId,string token)
        {
            if (string.IsNullOrEmpty(userId) ||
                string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login");
            }

            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            string decodedToken;

            try
            {
                decodedToken = Encoding.UTF8.GetString(
                    WebEncoders.Base64UrlDecode(token));
            }
            catch
            {
                return View("EmailConfirmationFailed");
            }

            var result = await userManager.ConfirmEmailAsync(
                user,
                decodedToken);

            if (result.Succeeded)
            {
                return View("EmailConfirmed");
            }

            return View("EmailConfirmationFailed");
        }
        [AllowAnonymous]
        [HttpGet]
        public IActionResult RegisterConfirmation(string email)
        {
            ViewBag.Email = email;

            return View();
        }
        // =====================================================
        // FORGOT PASSWORD - GET
        // =====================================================

        [AllowAnonymous]
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }


        // =====================================================
        // FORGOT PASSWORD - POST
        // =====================================================

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(
            ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await userManager.FindByEmailAsync(model.Email);

            // Don't reveal whether an account exists
            if (user == null)
            {
                return RedirectToAction("ForgotPasswordConfirmation");
            }

            // Generate password reset token
            var token =
                await userManager.GeneratePasswordResetTokenAsync(user);

            // Encode token for URL
            var encodedToken =
                WebEncoders.Base64UrlEncode(
                    Encoding.UTF8.GetBytes(token));

            // Create reset password link
            var resetLink = Url.Action(
                "ResetPassword",
                "Account",
                new
                {
                    email = user.Email,
                    token = encodedToken
                },
                protocol: HttpContext.Request.Scheme);

            // Send email
            bool emailSent = emailService.SendMail(
                user.Email!,
                "Reset your Circlely password",
                $"""
        <h2>Reset your Circlely password</h2>

        <p>
            We received a request to reset your password.
        </p>

        <p>
            Click the button below to create a new password:
        </p>

        <p>
            <a href="{resetLink}"
               style="
                   display:inline-block;
                   padding:12px 20px;
                   background:#6c5ce7;
                   color:white;
                   text-decoration:none;
                   border-radius:8px;
               ">
                Reset Password
            </a>
        </p>

        <p>
            If you did not request a password reset,
            you can safely ignore this email.
        </p>
        """);

            return RedirectToAction(
                "ForgotPasswordConfirmation");
        }


        // =====================================================
        // FORGOT PASSWORD CONFIRMATION
        // =====================================================

        [AllowAnonymous]
        [HttpGet]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }
        // =====================================================
        // RESET PASSWORD - GET
        // =====================================================

        [AllowAnonymous]
        [HttpGet]
        public IActionResult ResetPassword(
            string? email,
            string? token)
        {
            if (string.IsNullOrEmpty(email) ||
                string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login");
            }

            var model = new ResetPasswordViewModel
            {
                Email = email,
                Token = token
            };

            return View(model);
        }


        // =====================================================
        // RESET PASSWORD - POST
        // =====================================================

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(
            ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                return RedirectToAction("ResetPasswordConfirmation");
            }

            string decodedToken;

            try
            {
                decodedToken = Encoding.UTF8.GetString(
                    WebEncoders.Base64UrlDecode(model.Token)
                );
            }
            catch
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Invalid or expired password reset link.");

                return View(model);
            }

            var result = await userManager.ResetPasswordAsync(
                user,
                decodedToken,
                model.Password);

            if (result.Succeeded)
            {
                return RedirectToAction(
                    "ResetPasswordConfirmation");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(
                    string.Empty,
                    error.Description);
            }

            return View(model);
        }


        // =====================================================
        // RESET PASSWORD CONFIRMATION
        // =====================================================

        [AllowAnonymous]
        [HttpGet]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Dashboard()
        {
            return View();
        }
    }
    }
