using JobFinder.Data;
using JobFinder.Interface;
using JobFinder.Models;
using JobFinder.Repository;
using JobFinder.Service;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Net.payOS;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IDashboardRepository, DashboardRepository>();
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentity<AppUser, IdentityRole>()
.AddDefaultUI()
.AddDefaultTokenProviders()
.AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddRazorPages();
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ElevatedRights", policy =>
          policy.RequireRole("Administrator", "Users", "Recruiter"));
});


builder.Services.ConfigureApplicationCookie(options => {

    options.LoginPath = "/Login/" ;
    options.LogoutPath = "/Logout/"; // Customize logout path
    options.AccessDeniedPath = "/AccessDenied/"; // Customize access denied path
    options.SlidingExpiration = false; // Disable sliding expiration if not needed
});
builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
builder.Services.Configure<IdentityOptions>(options =>
{
    // Configure Password settings
    options.Password.RequireDigit = true; // No need for digits
    options.Password.RequireLowercase = false; // No need for lowercase
    options.Password.RequireNonAlphanumeric = true; // No special characters
    options.Password.RequireUppercase = true; // No uppercase required
    options.Password.RequiredLength = 6; // Minimum length 3 characters
    options.Password.RequiredUniqueChars = 1; // At least 6 unique character

    // Configure Lockout settings
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5); // Lock for 5 minutes
    options.Lockout.MaxFailedAccessAttempts = 5; // Lock after 5 failed attempts
    options.Lockout.AllowedForNewUsers = true;

    // Configure User settings
    options.User.AllowedUserNameCharacters =
        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = true; // Unique email required

    // Configure Sign-in settings
    options.SignIn.RequireConfirmedEmail = true; // Confirm email is required
    options.SignIn.RequireConfirmedPhoneNumber = false; // Phone confirmation not required
    options.SignIn.RequireConfirmedAccount = true; // Confirmed account required


});

/*builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
})
.AddCookie()
.AddGoogle(options =>
{
    var gconfig = builder.Configuration.GetSection("Authentication:Google");
    options.ClientId = gconfig["ClientId"];
    options.ClientSecret = gconfig["ClientSecret"];
     options.CallbackPath = "/ExternalLogin"; 
});*/
// Register the email sender service
builder.Services.AddSingleton<IEmailSender, SendMailService>();

// Config for PayOs
var configuration = builder.Configuration;
PayOS payOS = new PayOS(
    configuration["Environment:PAYOS_CLIENT_ID"] ?? throw new Exception("PAYOS_CLIENT_ID không được tìm thấy"),
    configuration["Environment:PAYOS_API_KEY"] ?? throw new Exception("PAYOS_API_KEY không được tìm thấy"),
    configuration["Environment:PAYOS_CHECKSUM_KEY"] ?? throw new Exception("PAYOS_CHECKSUM_KEY không được tìm thấy")
);

builder.Services.AddSingleton(payOS);

//Config Repository
builder.Services.AddScoped<IJobNatureRepository, JobNatureRepository>();
builder.Services.AddScoped<IJobPositionRepository, JobPositionRepository>();
builder.Services.AddScoped<IJobSeekerRepository, JobSeekerRepository>();
builder.Services.AddScoped<IRecruiterRepository, RecruiterRepository>();
builder.Services.AddScoped<IJobPostingRepository, JobPostingRepository>();
builder.Services.AddScoped<IAppUserRepository, AppUserRepository>();
builder.Services.AddScoped<IJobTypeRepository, JobTypeRepository>();
builder.Services.AddScoped<IApplicationRepository, ApplicationRepository>();
//Config Blob
builder.Services.AddSingleton<BlobStorageService>(provider =>
        new BlobStorageService(builder.Configuration.GetValue<string>("AzureBlobStorage:ConnectionString")));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
//
app.UseDeveloperExceptionPage(); 

app.Run();


