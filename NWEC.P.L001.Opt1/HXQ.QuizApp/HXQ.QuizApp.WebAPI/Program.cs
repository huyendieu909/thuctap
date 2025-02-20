using HXQ.QuizApp.Business.Services;
using HXQ.QuizApp.Data.Context;
using HXQ.QuizApp.Data.Models;
using HXQ.QuizApp.Data.Repositories;
using HXQ.QuizApp.Data.Seed;
using HXQ.QuizApp.Data.UnitOfWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;


namespace HXQ.QuizApp.WebAPI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            #region builder
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            //đăng ký db context
            builder.Services.AddDbContext<QuizAppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("HXQ.QuizApp.Data")));
            builder.Services.AddIdentity<User, Role>().AddEntityFrameworkStores<QuizAppDbContext>().AddDefaultTokenProviders();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            //đk repo
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IRoleRepository, RoleRepository>();
            builder.Services.AddScoped<IQuizRepository, QuizRepository>();
            builder.Services.AddScoped<IQuestionRepository, QuestionRepository>();
            builder.Services.AddScoped<IAnswerRepository, AnswerRepository>();

            //đk service 
            builder.Services.AddScoped<IQuizService, QuizService>();
            builder.Services.AddScoped<IQuestionService, QuestionService>();
            builder.Services.AddScoped<IAnswerService, AnswerService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IRoleService, RoleService>();

            //đk serilog
            Log.Logger = new LoggerConfiguration()
                        .ReadFrom.Configuration(builder.Configuration)
                        .CreateLogger();
            builder.Host.UseSerilog();
            #endregion

            #region app
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            using (var scope = app.Services.CreateScope())
            {
                await SeedData.InitializeAsync(scope.ServiceProvider);
            }

            app.Run(); 
            #endregion
        }
    }
}
