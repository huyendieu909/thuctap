using HoangXuanQuy.OnlinePainting.Data.Context;
using HoangXuanQuy.OnlinePainting.Data.Models;
using HoangXuanQuy.OnlinePainting.Data.UnitOfWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HoangXuanQuy.OnlinePainting.MVC
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            //Đăng ký dbcontext kèm lazy loading 
            builder.Services.AddDbContext<OnlinePaintingContext>(options =>
    options.UseLazyLoadingProxies().UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Cấu hình Identity
            builder.Services.AddIdentity<Customer, IdentityRole>()
                .AddEntityFrameworkStores<OnlinePaintingContext>()
                .AddDefaultTokenProviders();

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                options.SlidingExpiration = true;

                options.LoginPath = "/Login/Login";
                options.LogoutPath = "/Login/Logout";
                options.AccessDeniedPath = "/Login/AccessDenied";
            });

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

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

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            // Tạo role, user admin 
            using (var scope = app.Services.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                await EnsureRolesCreated(roleManager);

                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<Customer>>();

                var user = new Customer
                {
                    Name = "Quy siu vip",
                    PhoneNumber = "032403403",
                    Address = "23 Hanoi",
                    Email = "quyquy909@gmail.com",
                    UserName = "adminVIP"
                };
                var result = await userManager.CreateAsync(user, "Admin@123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Admin");
                }
            }

            app.Run();


            // Hàm đảm bảo role "User" tồn tại
            async Task EnsureRolesCreated(RoleManager<IdentityRole> roleManager)
            {
                string[] roleNames = { "User", "Admin" }; // Bạn có thể thêm các role khác vào đây
                foreach (var roleName in roleNames)
                {
                    if (!await roleManager.RoleExistsAsync(roleName))
                    {
                        await roleManager.CreateAsync(new IdentityRole(roleName));
                    }
                }
            }
        }
    }
}
