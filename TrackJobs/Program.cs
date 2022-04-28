using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TrackJobs.Data;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var connectionString = Environment.GetEnvironmentVariable("TRACKJOBS_CONNECTION_STRING", EnvironmentVariableTarget.Process);
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
var secureKey = Environment.GetEnvironmentVariable("TRACKJOBS_API_SECURE_KEY", EnvironmentVariableTarget.Process);
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
                              // Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

builder.Services.AddRazorPages().AddRazorRuntimeCompilation();

//builder.Services.AddAuthentication(options =>
//{
//    options.DefaultAuthenticateScheme = "JwtBearer";
//    options.DefaultChallengeScheme = "JwtBearer";
//}).AddJwtBearer("JwtBearer", jwtBearerOptions =>
//{
//    jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters
//    {
//        ValidateIssuerSigningKey = true,
//        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secureKey)),
//        ValidateIssuer = false,
//        ValidIssuer = "https://trackjobs.azurewebsites.net",
//        ValidateAudience = false,
//        ValidateLifetime = true,
//        ClockSkew = TimeSpan.FromMinutes(5)
//    };
//});

//builder.Services.AddCors();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    SeedRoles.Initialize(services);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

//app.UseCors(options => options
//            .WithOrigins(new[] {"http://localhost:3000", "http://localhost:7125", "http://trackjobs.azurewebsites.net" })
//            .AllowAnyHeader()
//            .AllowAnyMethod()
//            .AllowCredentials()
//);

app.MapControllerRoute(
    name: "MyArea",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
