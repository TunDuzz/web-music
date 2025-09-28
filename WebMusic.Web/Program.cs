using WebMusic.Infrastructure;
using WebMusic.Web.Services;
using FluentValidation;
using WebMusic.Application.Validators;
using WebMusic.Application.Services;
using WebMusic.Domain.Entities;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews()
    .AddRazorRuntimeCompilation();

// Add Infrastructure services
builder.Services.AddInfrastructure(builder.Configuration);

// Add Identity services
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
{
    // Password settings
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
    
    // Lockout settings
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;
    
    // User settings
    options.User.RequireUniqueEmail = true;
    options.SignIn.RequireConfirmedEmail = false;
})
.AddEntityFrameworkStores<WebMusic.Infrastructure.Data.WebMusicDbContext>()
.AddDefaultTokenProviders();

// Add Authentication (Identity already configures this)
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Auth/Login";
    options.LogoutPath = "/Auth/Logout";
    options.AccessDeniedPath = "/Auth/AccessDenied";
    options.ExpireTimeSpan = TimeSpan.FromDays(7);
    options.SlidingExpiration = true;
});

// Add Authorization
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdmin", policy => policy.RequireRole("Admin"));
    options.AddPolicy("RequireModerator", policy => policy.RequireRole("Admin", "Moderator"));
    options.AddPolicy("RequireUser", policy => policy.RequireRole("Admin", "Moderator", "User"));
});

// Add Web Services
builder.Services.AddScoped<ISongWebService, SongWebService>();
builder.Services.AddScoped<IAlbumWebService, AlbumWebService>();
builder.Services.AddScoped<IPlaylistWebService, PlaylistWebService>();
builder.Services.AddScoped<IFileUploadService, FileUploadService>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();

// Add FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<CreateSongCommandValidator>();

var app = builder.Build();

// Configure the HTTP request pipeline.
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
