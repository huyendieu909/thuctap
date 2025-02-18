using Microsoft.EntityFrameworkCore;
using HXQ.NWBC_Assignment05_v50.Data;
using HXQ.NWBC_Assignment05_v50.Data.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using HXQ.NWBC_Assignment05_v50.Data.Exceptions;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using HXQ.NWBC_Assignment05_v50.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;

namespace HXQ.NWBC_Assignment05_v50.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            #region builder
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            //Cấu hình swagger,  Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                var provider = builder.Services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    c.SwaggerDoc(description.GroupName, new Microsoft.OpenApi.Models.OpenApiInfo { Title = "My API", Version = description.ApiVersion.ToString() });
                }
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using the Bearer scheme.  
                                  Enter 'Bearer' [space] and then your token in the text input below.
                                  Example: 'Bearer 12345abcdef'",
                    Name = "Authorization",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {                    {
                          new OpenApiSecurityScheme
                          {
                              Reference = new OpenApiReference
                              {
                                  Type = ReferenceType.SecurityScheme,
                                  Id = "Bearer"
                              }
                          },
                         new string[] {}
                    }
                });
            });

            //Cấu hình database
            builder.Services.AddDbContext<SchoolContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("HXQ.NWBC_Assignment05_v50.API")));

            //Cấu hình api version
            builder.Services.AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ApiVersionReader = new UrlSegmentApiVersionReader();
            }).AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            }
                );

            //Cấu hình identity
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                //Tùy chỉnh điều kiện cho identity
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
            }).AddEntityFrameworkStores<SchoolContext>().AddDefaultTokenProviders();

            //Cấu hình JWT
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
                    ValidateIssuer = false,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidateAudience = false,
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    //Chỉnh chênh lệch thời gian so token về 0, thực tế việc xác thực token giữa các server có thể có delay,  
                    ClockSkew = TimeSpan.Zero
                };
            });
            #endregion

            #region app
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
                    foreach (var description in provider.ApiVersionDescriptions)
                    {
                        c.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                    }
                });
            }

            app.UseHttpsRedirection();

            //đk authen và autho
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            //implement global exception handler
            app.ConfigureBuildInExceptionHandler();
            //dùng exception middleware custom
            app.UseMiddleware<CustomExceptionMiddleware>();

            //Thêm role, thêm 1 acc admin
            using (var scope = app.Services.CreateScope())
            {
                await SeedRoles(scope.ServiceProvider);
                await CreateAdmin(scope.ServiceProvider);
            }
            async Task SeedRoles(IServiceProvider serviceProvider)
            {
                var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                string[] roleNames = { "Admin", "User" };

                foreach (var roleName in roleNames)
                {
                    var roleExists = await roleManager.RoleExistsAsync(roleName);
                    if (!roleExists)
                    {
                        await roleManager.CreateAsync(new IdentityRole(roleName));
                        Console.WriteLine($"Role '{roleName}' đã được thêm vào DB!");
                    }
                }
            }
            async Task CreateAdmin(IServiceProvider serviceProvider)
            {
                var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

                string adminEmail = "admin@lord.com";
                //username admin
                string adminUsername = "admin";
                //pass admin
                string adminPassword = "Admin@123"; 

                var adminUser = await userManager.FindByEmailAsync(adminEmail);
                if (adminUser == null)
                {
                    var newAdmin = new ApplicationUser
                    {
                        UserName = adminUsername,
                        Email = adminEmail,
                        Name = "Chúa nhẫn"
                    };

                    var createAdmin = await userManager.CreateAsync(newAdmin, adminPassword);
                    if (createAdmin.Succeeded)
                    {
                        await userManager.AddToRoleAsync(newAdmin, "Admin");
                    }
                }
            }

            await app.RunAsync(); 
            #endregion 
            
        }
        
    }
}
