using Bookify.Core.Entities;
using Bookify.Core.Interfaces;
using Bookify.Core.Seed;
using Bookify.Data.Context;
using Bookify.Data.Repositories;
using Bookify.Data.UnitOfWork;
using Bookify.Services.Implementations;
using Bookify.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


builder.Services.AddDbContext<BookifyDbContext>(op => op.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
builder.Services.AddScoped<IRoomRepository, RoomRepository>();
builder.Services.AddScoped<IWishlistRepository, WishlistRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRoomTypeRepository, RoomTypeRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<BookifyDbContext>()
    .AddDefaultTokenProviders();
builder.Services.AddScoped<IAdminService, AdminService>();
var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

    await IdentitySeedData.SeedRolesAsync(roleManager);
    await IdentitySeedData.SeedAdminAsync(userManager);
}
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
//to migrate anything
//Add-Migration m5 -Project Bookify.Data -StartupProject Bookify.Web

//Update-Database -Project Bookify.Data -StartupProject Bookify.Web
