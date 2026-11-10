using ECommerceStore.Models;
using ECommerceStore.Services;
using ECommerceStore.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Net.Sockets;
using System.Text.Encodings.Web;

namespace ECommerceStore.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IWebHostEnvironment _environment;
        private readonly IEmailService _emailService;
        private readonly ILogger<AccountController> _logger;
        private readonly RecaptchaService recaptchaService;
        private readonly IConfiguration configuration;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IWebHostEnvironment environment,
            IEmailService emailService,ILogger<AccountController> logger,RecaptchaService recaptchaService,IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _environment = environment;
            _emailService = emailService;
            _logger = logger;
            this.recaptchaService = recaptchaService;
            this.configuration = configuration;
        }

        // PROFILE - GET
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("Login");
            }

            var model = new ProfileViewModel
            {
                FullName = user.FullName,
                Email = user.Email,
                UserName = user.UserName,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address,
                ProfileImageUrl = user.ProfileImageUrl
            };

            return View(model);
        }

        // PROFILE - POST
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(ProfileViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("Login");
            }

            if (!ModelState.IsValid)
            {
                model.Email = user.Email;
                model.UserName = user.UserName;
                model.ProfileImageUrl = user.ProfileImageUrl;

                return View(model);
            }

            user.FullName = model.FullName;
            user.PhoneNumber = model.PhoneNumber;
            user.Address = model.Address;

            // PROFILE IMAGE
            if (model.ProfileImage != null &&
                model.ProfileImage.Length > 0)
            {
                string uploadsFolder = Path.Combine(
                    _environment.WebRootPath,
                    "uploads",
                    "profiles"
                );

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                // Delete old image
                if (!string.IsNullOrEmpty(user.ProfileImageUrl))
                {
                    string oldImagePath = Path.Combine(
                        _environment.WebRootPath,
                        user.ProfileImageUrl
                            .TrimStart('/')
                            .Replace(
                                "/",
                                Path.DirectorySeparatorChar.ToString()
                            )
                    );

                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                // Generate unique filename
                string extension =
                    Path.GetExtension(model.ProfileImage.FileName);

                string fileName =
                    Guid.NewGuid().ToString() + extension;

                string filePath =
                    Path.Combine(uploadsFolder, fileName);

                // Save image
                using (var stream = new FileStream(
                    filePath,
                    FileMode.Create))
                {
                    await model.ProfileImage.CopyToAsync(stream);
                }

                // Save image URL
                user.ProfileImageUrl =
                    "/uploads/profiles/" + fileName;
            }

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                TempData["Success"] =
                    "Your profile has been updated successfully.";

                return RedirectToAction("Profile");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(
                    "",
                    error.Description
                );
            }

            return View(model);
        }

        // LOGIN - GET
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Login(LoginViewModel model,string? returnUrl = null)
        {
            // =====================================================
            // 1. MODEL VALIDATION
            // =====================================================

            if (!ModelState.IsValid)
            {
                return View(model);
            }
            // =====================================================
            // 2. GET CAPTCHA TOKEN
            // =====================================================

            var captchaToken =
                Request.Form["g-recaptcha-response"].ToString();


            if (string.IsNullOrEmpty(captchaToken))
            {
                ModelState.AddModelError(
                    "",
                    "Please complete the CAPTCHA."
                );

                return View(model);
            }


            // =====================================================
            // 3. VERIFY CAPTCHA
            // =====================================================

            var captchaValid =
                await recaptchaService.VerifyAsync(
                    captchaToken
                );


            if (!captchaValid)
            {
                ModelState.AddModelError(
                    "",
                    "CAPTCHA verification failed. Please try again."
                );

                return View(model);
            }


            // =====================================================
            // 4. FIND USER
            // =====================================================

            var user =
                await _userManager.FindByEmailAsync(
                    model.Email
                );


            if (user == null)
            {
                ModelState.AddModelError(
                    "",
                    "Invalid email or password."
                );

                return View(model);
            }


            // =====================================================
            // 5. SIGN IN
            // =====================================================

            var result =
                await _signInManager.PasswordSignInAsync(
                    user,
                    model.Password,
                    model.RememberMe,
                    lockoutOnFailure: true
                );


            // =====================================================
            // 6. SUCCESS
            // =====================================================

            if (result.Succeeded)
            {
                if (!string.IsNullOrEmpty(returnUrl) &&
                    Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }

                return RedirectToAction(
                    "Index",
                    "Home"
                );
            }


            // =====================================================
            // 7. LOCKED OUT
            // =====================================================

            if (result.IsLockedOut)
            {
                ModelState.AddModelError(
                    "",
                    "Your account is locked. Please try again later."
                );

                return View(model);
            }


            // =====================================================
            // 8. EMAIL NOT CONFIRMED
            // =====================================================

            if (result.IsNotAllowed)
            {
                ModelState.AddModelError(
                    "",
                    "Please confirm your email before logging in."
                );

                return View(model);
            }


            // =====================================================
            // 9. INVALID PASSWORD
            // =====================================================

            ModelState.AddModelError(
                "",
                "Invalid email or password."
            );

            return View(model);
        }


        // REGISTER - GET
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
    //REGITER POST  
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var user = new ApplicationUser
        {
            UserName = model.Email,
            Email = model.Email,
            FullName = model.FullName
        };

        var result = await _userManager.CreateAsync(
            user,
            model.Password
        );

        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);

            return View(model);
        }

        var token =
            await _userManager.GenerateEmailConfirmationTokenAsync(user);

        var confirmLink = Url.Action("ConfirmEmail","Account",
            new
            {
                userId = user.Id,
                token = token
            },
            Request.Scheme
        );

            var emailBody = $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">

    <title>Confirm Your AnkitKart Account</title>
</head>

<body style=""
    margin:0;
    padding:0;
    background-color:#f5f7fb;
    font-family:Arial, Helvetica, sans-serif;
"">

    <table width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0""
           style=""background-color:#f5f7fb; padding:40px 15px;"">

        <tr>
            <td align=""center"">

                <!-- Main Card -->
                <table width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0""
                       style=""
                           max-width:600px;
                           background:#ffffff;
                           border-radius:18px;
                           overflow:hidden;
                           box-shadow:0 8px 30px rgba(0,0,0,0.08);
                       "">

                    <!-- Header -->
                    <tr>
                        <td align=""center""
                            style=""
                                background:#ff6600;
                                padding:28px 20px;
                            "">

                            <div style=""
                                font-size:30px;
                                font-weight:bold;
                                color:#ffffff;
                                letter-spacing:0.5px;
                            "">
                                AnkitKart
                            </div>

                            <div style=""
                                margin-top:6px;
                                font-size:13px;
                                color:#fff3eb;
                            "">
                                Your trusted shopping destination
                            </div>

                        </td>
                    </tr>

                    <!-- Content -->
                    <tr>
                        <td style=""padding:45px 40px 35px 40px;"">

                            <!-- Icon -->
                            <div style=""
                                width:70px;
                                height:70px;
                                line-height:70px;
                                margin:0 auto 25px auto;
                                background:#fff3eb;
                                border-radius:50%;
                                text-align:center;
                                font-size:32px;
                            "">
                                ✉
                            </div>

                            <h1 style=""
                                margin:0 0 15px 0;
                                text-align:center;
                                color:#172033;
                                font-size:26px;
                                font-weight:700;
                            "">
                                Welcome to AnkitKart!
                            </h1>

                            <p style=""
                                margin:0 0 15px 0;
                                text-align:center;
                                color:#687386;
                                font-size:15px;
                                line-height:1.7;
                            "">
                                Thank you for creating your account with us.
                            </p>

                            <p style=""
                                margin:0 0 28px 0;
                                text-align:center;
                                color:#687386;
                                font-size:15px;
                                line-height:1.7;
                            "">
                                Please confirm your email address to activate
                                your AnkitKart account.
                            </p>

                            <!-- Confirmation Button -->
                            <div style=""text-align:center; margin:30px 0;"">

                                <a href=""{HtmlEncoder.Default.Encode(confirmLink!)}""
                                   style=""
                                       display:inline-block;
                                       padding:14px 30px;
                                       background:#ff6600;
                                       color:#ffffff;
                                       text-decoration:none;
                                       border-radius:10px;
                                       font-size:15px;
                                       font-weight:bold;
                                   "">
                                    Confirm My Email
                                </a>

                            </div>

                            <!-- Security Info -->
                            <div style=""
                                margin-top:30px;
                                padding:18px;
                                background:#f8fafc;
                                border:1px solid #edf0f4;
                                border-radius:12px;
                            "">

                                <p style=""
                                    margin:0 0 8px 0;
                                    color:#202938;
                                    font-size:13px;
                                    font-weight:bold;
                                "">
                                    Why confirm your email?
                                </p>

                                <p style=""
                                    margin:0;
                                    color:#7b8494;
                                    font-size:13px;
                                    line-height:1.6;
                                "">
                                    Email verification helps us keep your account
                                    secure and ensures that your email address
                                    belongs to you.
                                </p>

                            </div>

                            <!-- Alternative Link -->
                            <p style=""
                                margin:28px 0 8px 0;
                                color:#7b8494;
                                font-size:12px;
                                line-height:1.6;
                                text-align:center;
                            "">
                                If the button above does not work, copy and paste
                                the following link into your browser:
                            </p>

                            <p style=""
                                margin:0;
                                word-break:break-all;
                                text-align:center;
                                font-size:12px;
                            "">
                                <a href=""{HtmlEncoder.Default.Encode(confirmLink!)}""
                                   style=""
                                       color:#ff6600;
                                       text-decoration:none;
                                   "">
                                    {HtmlEncoder.Default.Encode(confirmLink!)}
                                </a>
                            </p>

                        </td>
                    </tr>

                    <!-- Footer -->
                    <tr>
                        <td style=""
                            background:#f8fafc;
                            padding:22px 30px;
                            border-top:1px solid #edf0f4;
                            text-align:center;
                        "">

                            <p style=""
                                margin:0 0 6px 0;
                                color:#687386;
                                font-size:12px;
                            "">
                                This email was sent because an AnkitKart
                                account was created using this email address.
                            </p>

                            <p style=""
                                margin:0;
                                color:#9aa3b2;
                                font-size:11px;
                            "">
                                © 2026 AnkitKart. All rights reserved.
                            </p>

                        </td>
                    </tr>

                </table>

            </td>
        </tr>

    </table>

</body>
</html>";

            try
        {
            if (string.IsNullOrEmpty(user.Email))
            {
                ModelState.AddModelError(
                    "",
                    "Email address is missing."
                );

                return View(model);
            }

            var emailSent = _emailService.SendEmail(user.Email,"Confirm Your Email",emailBody);

            if (!emailSent)
            {
                ModelState.AddModelError(
                    "",
                    "Registration was successful, but we could not send the confirmation email. Please try again later."
                );

                return View(model);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Could not send confirmation email to {Email}",
                user.Email
            );

            ModelState.AddModelError(
                "",
                "Registration was successful, but we could not send the confirmation email. Please try again later."
            );

            return View(model);
        }

        return RedirectToAction("RegisterConfirmation");
    }
        // CONFIRM EMAIL
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userId,string token)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
            {
                ModelState.AddModelError(
                    "",
                    "Invalid email confirmation link."
                );

                return View("EmailConfirmed");
            }

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                ModelState.AddModelError(
                    "",
                    "User not found."
                );

                return View("EmailConfirmed");
            }

            var result = await _userManager.ConfirmEmailAsync( user,token);

            if (result.Succeeded)
            {
                return View("ConfirmEmail");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(
                    "",
                    error.Description
                );
            }

            return View("EmailConfirmed");
        }
        // REGISTER CONFIRMATION
        [HttpGet]
        public IActionResult RegisterConfirmation()
        {
            return View();
        }
        //Reset password
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View(); 
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user =await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return NotFound("User not found.");
            }
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var resetlink = Url.Action("ResetPassword", "Account",
                new
                {
                    email = user.Email,
                    token = token
                },Request.Scheme);
            string subject = "Reset Your AnkitKart Password";

            string message = $@"
<!DOCTYPE html>
<html lang='en'>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>Reset Your AnkitKart Password</title>
</head>

<body style='
    margin:0;
    padding:0;
    background-color:#f4f7fb;
    font-family:Arial,Helvetica,sans-serif;
    color:#1f2937;
'>

    <table width='100%' cellpadding='0' cellspacing='0' border='0'
           style='background-color:#f4f7fb; padding:40px 15px;'>

        <tr>
            <td align='center'>

                <!-- Main Container -->
                <table width='100%' cellpadding='0' cellspacing='0' border='0'
                       style='
                           max-width:600px;
                           background-color:#ffffff;
                           border-radius:12px;
                           overflow:hidden;
                           box-shadow:0 4px 20px rgba(0,0,0,0.08);
                       '>

                    <!-- Header -->
                    <tr>
                        <td style='
                            background-color:#0d6efd;
                            padding:28px 35px;
                            text-align:center;
                        '>

                            <h1 style='
                                margin:0;
                                color:#ffffff;
                                font-size:28px;
                                font-weight:700;
                            '>
                                AnkitKart
                            </h1>

                            <p style='
                                margin:6px 0 0;
                                color:#eaf2ff;
                                font-size:14px;
                            '>
                                Shop Smart. Shop Better.
                            </p>

                        </td>
                    </tr>


                    <!-- Content -->
                    <tr>
                        <td style='padding:40px 35px;'>

                            <h2 style='
                                margin:0 0 20px;
                                color:#111827;
                                font-size:24px;
                            '>
                                Reset Your Password
                            </h2>

                            <p style='
                                margin:0 0 15px;
                                font-size:16px;
                                line-height:1.7;
                                color:#4b5563;
                            '>
                                Hello,
                            </p>

                            <p style='
                                margin:0 0 15px;
                                font-size:16px;
                                line-height:1.7;
                                color:#4b5563;
                            '>
                                We received a request to reset the password
                                for your <strong>AnkitKart</strong> account.
                            </p>

                            <p style='
                                margin:0 0 28px;
                                font-size:16px;
                                line-height:1.7;
                                color:#4b5563;
                            '>
                                Click the button below to create a new password
                                and regain access to your account.
                            </p>


                            <!-- Button -->
                            <table cellpadding='0' cellspacing='0' border='0'
                                   width='100%'>

                                <tr>
                                    <td align='center'>

                                        <a href='{resetlink}'
                                           style='
                                               display:inline-block;
                                               background-color:#0d6efd;
                                               color:#ffffff;
                                               text-decoration:none;
                                               font-size:16px;
                                               font-weight:bold;
                                               padding:14px 32px;
                                               border-radius:7px;
                                           '>
                                            Reset My Password
                                        </a>

                                    </td>
                                </tr>

                            </table>


                            <!-- Security Notice -->
                            <table width='100%' cellpadding='0' cellspacing='0'
                                   border='0'
                                   style='
                                       margin-top:30px;
                                       background-color:#f8fafc;
                                       border-left:4px solid #0d6efd;
                                   '>

                                <tr>
                                    <td style='
                                        padding:15px 18px;
                                        font-size:14px;
                                        line-height:1.6;
                                        color:#64748b;
                                    '>

                                        <strong style='color:#334155;'>
                                            Security Notice
                                        </strong>
                                        <br>

                                        For your security, this password reset
                                        link is intended only for you.
                                        Please do not share it with anyone.

                                    </td>
                                </tr>

                            </table>


                            <p style='
                                margin:28px 0 0;
                                font-size:14px;
                                line-height:1.6;
                                color:#6b7280;
                            '>
                                If you did not request a password reset,
                                you can safely ignore this email. Your account
                                will remain secure and your password will not
                                be changed.
                            </p>

                        </td>
                    </tr>


                    <!-- Footer -->
                    <tr>
                        <td style='
                            background-color:#f8fafc;
                            padding:25px 35px;
                            text-align:center;
                            border-top:1px solid #e5e7eb;
                        '>

                            <p style='
                                margin:0 0 8px;
                                color:#374151;
                                font-size:14px;
                                font-weight:bold;
                            '>
                                AnkitKart
                            </p>

                            <p style='
                                margin:0;
                                color:#9ca3af;
                                font-size:12px;
                                line-height:1.6;
                            '>
                                This is an automated email. Please do not
                                reply to this message.
                            </p>

                            <p style='
                                margin:10px 0 0;
                                color:#9ca3af;
                                font-size:12px;
                            '>
                                &copy; {DateTime.Now.Year} AnkitKart.
                                All rights reserved.
                            </p>

                        </td>
                    </tr>

                </table>

            </td>
        </tr>

    </table>

</body>
</html>";
            bool EmailSent = _emailService.SendEmail(user.Email, subject, message);
            if (!EmailSent)
            {
                ModelState.AddModelError("", "Unable to Send Reset Password Link,Please Try Again Later");
                return View(model);
            }
            return RedirectToAction(nameof(ForgotPasswordConfirmation));

        }
        //Forgotpassword Confirmation
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }
        [HttpGet]
        public IActionResult ResetPassword(string email,string token)
        {
            if (string.IsNullOrEmpty(email) ||string.IsNullOrEmpty(token))
            {
                return RedirectToAction(nameof(ForgotPassword));
            }
            var model = new ResetPasswordViewModel
            {
                Email = email,
                Token = token
            };
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword( ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                return RedirectToAction(
                    nameof(ResetPasswordConfirmation));
            }

            var result = await _userManager.ResetPasswordAsync(
                user,
                model.Token,
                model.NewPassword
            );

            if (result.Succeeded)
            {
                return RedirectToAction(
                    nameof(ResetPasswordConfirmation));
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(
                    string.Empty,
                    error.Description
                );
            }

            return View(model);
        }
        [HttpGet]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }
    }
}