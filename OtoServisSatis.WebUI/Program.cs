using OtoServisSatis.Data;
using OtoServisSatis.Service.Abstract;
using OtoServisSatis.Service.Concrete;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace OtoServisSatis.WebUI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddSession();

            builder.Services.AddDbContext<DataBaseContext>();

            builder.Services.AddTransient(typeof(IService<>), typeof(Service<>));
            builder.Services.AddTransient<ICarService,CarService>();
            builder.Services.AddTransient<IUserService,UserService>();

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(x =>
            {
                x.LoginPath = "/Account/Login";
                x.AccessDeniedPath = "/AccesDenied";
                x.LogoutPath = "/Account/Logout";
                x.Cookie.Name = "Admin";
                x.Cookie.MaxAge =TimeSpan.FromDays(7);
                x.Cookie.IsEssential = true;
            });
            builder.Services.AddAuthorization(x =>
            {
                x.AddPolicy("AdminPolicy", policy=>policy.RequireClaim(ClaimTypes.Role,"Admin"));
                x.AddPolicy("UserPolicy", policy => policy.RequireClaim(ClaimTypes.Role, "User", "Admin"));
                x.AddPolicy("CustomerPolicy", policy => policy.RequireClaim(ClaimTypes.Role, "Customer","User", "Admin"));
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline. 
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSession();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
            name: "admin",
            pattern: "{area:exists}/{controller=Main}/{action=Index}/{id?}");
            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
