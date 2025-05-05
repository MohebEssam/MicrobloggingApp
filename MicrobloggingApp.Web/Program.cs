using MicrobloggingApp.Web.Services;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Register MVC services
builder.Services.AddControllersWithViews();

// Register HTTP clients
builder.Services.AddHttpClient(); // for generic use (e.g. AuthController)
builder.Services.AddHttpClient<PostApiService>();

// Accessor for retrieving user claims (like JWT from cookie)
builder.Services.AddHttpContextAccessor();

// Register authentication using cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Login";             // Corrected controller name (was /Account/Login)
        options.AccessDeniedPath = "/Auth/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromHours(1); // Optional: Session timeout
        options.SlidingExpiration = true;
        options.Cookie.Name = ".MicrobloggingApp.Auth"; // Ensure consistency with your token name
    });

// Add any other scoped services if needed (e.g., other API clients or helpers)
// builder.Services.AddScoped<IExampleService, ExampleService>();

var app = builder.Build();

// Configure middleware pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
