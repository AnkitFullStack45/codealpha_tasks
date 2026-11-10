using Circlely.Data;
using Circlely.Models;
using Circlely.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddTransient<IEmailService, EmailService>();
builder.Services.Configure<EmailSettings>(
    builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("constr")));
builder.Services
    .AddIdentity<ApplicationUser, IdentityRole>(options =>
    {
        options.Password.RequiredLength = 6;
        options.Password.RequireDigit = true;
        options.Password.RequireUppercase = true;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireLowercase = true;
        options.Password.RequiredUniqueChars = 1;
        options.SignIn.RequireConfirmedEmail = true;
        options.SignIn.RequireConfirmedPhoneNumber = false;
        options.User.RequireUniqueEmail = true;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();
// COOKIE SETTINGS
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";

    options.AccessDeniedPath = "/Account/AccessDenied";

    options.ExpireTimeSpan = TimeSpan.FromDays(14);

    options.SlidingExpiration = true;
});
var app = builder.Build();
// CREATE ADMIN ROLE + ADMIN ACCOUNT
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var roleManager =
        services.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager =
        services.GetRequiredService<UserManager<ApplicationUser>>();
    var configuration =
        services.GetRequiredService<IConfiguration>();
    // -----------------------------------------------------
    // Create Admin Role
    // -----------------------------------------------------
    if (!await roleManager.RoleExistsAsync("Admin"))
    {
        await roleManager.CreateAsync(
            new IdentityRole("Admin"));
    }
    // -----------------------------------------------------
    // Get Admin Credentials
    // -----------------------------------------------------
    var adminEmail =
        configuration["AdminAccount:Email"];
    var adminPassword =
        configuration["AdminAccount:Password"];
    // -----------------------------------------------------
    // Check Admin User
    // -----------------------------------------------------

    if (!string.IsNullOrWhiteSpace(adminEmail) &&
        !string.IsNullOrWhiteSpace(adminPassword))
    {
        var adminUser =
            await userManager.FindByEmailAsync(adminEmail);
        // Create Admin User
        if (adminUser == null)
        {
            adminUser = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,

                // Admin doesn't need email confirmation
                EmailConfirmed = true
            };
            var result =
                await userManager.CreateAsync(
                    adminUser,
                    adminPassword);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(
                    adminUser,
                    "Admin");
            }
        }
        // Make Existing User Admin
        else
        {
            if (!await userManager.IsInRoleAsync(adminUser,"Admin"))
            {
                await userManager.AddToRoleAsync(adminUser,"Admin");
            }
        }
    }
}
// ERROR HANDLING
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
// HTTP PIPELINE
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
// Authentication must come before Authorization.
app.UseAuthentication();
app.UseAuthorization();
// DEFAULT MVC ROUTE
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}")
    .WithStaticAssets();
// RUN APPLICATION
app.Run();
