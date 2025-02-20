using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using HXQ.QuizApp.Data.Models;
using HXQ.QuizApp.Data.Context;


namespace HXQ.QuizApp.Data.Seed
{
    public static class SeedData
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            using var db = serviceProvider.GetRequiredService<QuizAppDbContext>();
            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<Role>>();

            // Đảm bảo database đã được tạo (nếu chưa có)
            await db.Database.MigrateAsync();

            // Seed dữ liệu cho Quiz
            if (!db.Quizzes.Any())
            {
                var quizzes = new List<Quiz>
                {
                    new Quiz { Title = "C# Basics", Duration = 30, Description = "Basic C# knowledge test." },
                    new Quiz { Title = "ASP.NET Core", Duration = 40, Description = "ASP.NET Core fundamentals." },
                    new Quiz { Title = "Entity Framework Core", Duration = 45, Description = "Learn about EF Core." },
                    new Quiz { Title = "LINQ Fundamentals", Duration = 35, Description = "Test your LINQ skills." },
                    new Quiz { Title = "Design Patterns", Duration = 50, Description = "Software design principles." }
                };

                db.Quizzes.AddRange(quizzes);
                await db.SaveChangesAsync();
            }

            // Seed dữ liệu cho Question
            if (!db.Questions.Any())
            {
                var questions = new List<Question>
                {
                    new Question { Content = "What is C#?", QuestionType = Question.QuestionTypeEnum.ShortAnswer },
                    new Question { Content = "What is ASP.NET Core?", QuestionType = Question.QuestionTypeEnum.ShortAnswer },
                    new Question { Content = "Explain Entity Framework Core.", QuestionType = Question.QuestionTypeEnum.ShortAnswer },
                    new Question { Content = "What is LINQ used for?", QuestionType = Question.QuestionTypeEnum.ShortAnswer },
                    new Question { Content = "What is Dependency Injection?", QuestionType = Question.QuestionTypeEnum.ShortAnswer }
                };

                db.Questions.AddRange(questions);
                await db.SaveChangesAsync();
            }

            // Seed dữ liệu cho Answer
            if (!db.Answers.Any())
            {
                // Lấy danh sách câu hỏi từ database
                var questions = db.Questions.ToList(); 
                var answers = new List<Answer>
                {
                    new Answer { Content = "C# is a modern object-oriented programming language.", IsCorrect = true, QuestionId = questions[0].Id },
                    new Answer { Content = "ASP.NET Core is a cross-platform framework for building web applications.", IsCorrect = true, QuestionId = questions[1].Id },
                    new Answer { Content = "EF Core is an ORM that allows working with databases using C#.", IsCorrect = true, QuestionId = questions[2].Id },
                    new Answer { Content = "LINQ is used to query collections and databases in C#.", IsCorrect = true, QuestionId = questions[3].Id },
                    new Answer { Content = "Dependency Injection is a technique for achieving Inversion of Control.", IsCorrect = true, QuestionId = questions[4].Id }
                };

                db.Answers.AddRange(answers);
                await db.SaveChangesAsync();
            }

            //Seed dữ liệu Role
            if (!db.Roles.Any())
            {
                var roles = new List<Role>
                {
                    new Role{Name = "Admin", Description = "Trùm cuối", NormalizedName = "ADMIN"},
                    new Role{Name = "User", Description = "Dân đen", NormalizedName = "USER"}
                };
                
                foreach (Role role in roles)
                {
                    await roleManager.CreateAsync(role);
                }
            }

            //Seed dữ liệu user
            if (!db.Users.Any())
            {
                var adminUser = new User
                {
                    FirstName = "Vua",
                    LastName = "Trò Chơi",
                    UserName = "admin",
                    DateOfBirth = new DateTime(2022, 11, 11),
                    Email = "admin@mail.com",
                    EmailConfirmed = true
                };

                var normalUser = new User
                {
                    FirstName = "Tôi",
                    LastName = "Làm Biếng",
                    UserName = "username",
                    Email = "username@mail.com",
                    DateOfBirth = new DateTime(2002,10,10),
                    EmailConfirmed = true
                };

                var addAdminResult = await userManager.CreateAsync(adminUser, "Admin@123");
                var addUserResult = await userManager.CreateAsync(normalUser, "User@123");

                if (addAdminResult.Succeeded) await userManager.AddToRoleAsync(adminUser, "Admin");
                if (addUserResult.Succeeded) await userManager.AddToRoleAsync(normalUser, "User"); 
            }

            //seed sql
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<QuizAppDbContext>();

            var filePath = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory())?.FullName, "QuizApp.Data", "Seed", "SeedUserQuiz.sql");

            if (File.Exists(filePath))
            {
                var sqlScript = await File.ReadAllTextAsync(filePath);

                Console.WriteLine($"Đang thực thi SQL:\n{sqlScript}");
                await context.Database.ExecuteSqlRawAsync(sqlScript);
                Console.WriteLine("Seed dữ liệu thành công!");
            }
            else
            {
                throw new FileNotFoundException($"SQL seed file not found: {filePath}");
            }
        }
    }
}
