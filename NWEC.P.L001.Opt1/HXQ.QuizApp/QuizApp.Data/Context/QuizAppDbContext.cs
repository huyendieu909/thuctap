using HXQ.QuizApp.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HXQ.QuizApp.Data.Context
{
    public class QuizAppDbContext : IdentityDbContext<User, Role, Guid>
    {
        public QuizAppDbContext(DbContextOptions<QuizAppDbContext> options) : base(options) { }
        public DbSet<Quiz> Quizzes { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<QuizQuestion> QuizQuestions { get; set; }
        public DbSet<UserQuiz> UserQuiz { get; set; }
        public DbSet<UserAnswer> UserAnswers { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //thêm quan hệ giữa các bảng

            //user-role
            modelBuilder.Entity<UserRole>(ur =>
            {
                ur.HasOne(y => y.User).WithMany(u => u.UserRoles).HasForeignKey(u => u.UserId).IsRequired();
                ur.HasOne(x => x.Role).WithMany(x => x.UserRoles).HasForeignKey(x => x.RoleId).IsRequired();
            });

            //quiz-question
            modelBuilder.Entity<QuizQuestion>(qq =>
            {
                qq.HasKey(x => new { x.QuizId, x.QuestionId });

                qq.HasOne(x => x.Quiz).WithMany(x => x.QuizQuestions).HasForeignKey(x => x.QuizId);
                qq.HasOne(x => x.Question).WithMany(x => x.QuizQuestions).HasForeignKey(x => x.QuestionId);
            });

            //user-quiz 
            /*Note: ở đây không có composite key như các bảng ghép trên. Lý do là 1 user có thể làm 1 quiz nhiều lần, nếu tạo composite key thì cặp giá trị userid quizid sẽ duy nhất và vì vậy user chỉ làm đc quiz 1 lần.*/
            modelBuilder.Entity<UserQuiz>(uq =>
            {
                uq.HasOne(x => x.User).WithMany(x => x.UserQuizzes).HasForeignKey(x => x.UserId);
                uq.HasOne(x => x.Quiz).WithMany(x => x.UserQuizzes).HasForeignKey(x => x.QuizId);
            });

            //user-answer 
            modelBuilder.Entity<UserAnswer>(ua =>
            {
                ua.HasOne(x => x.UserQuiz).WithMany(x => x.UserAnswers).HasForeignKey(x => x.UserQuizId).OnDelete(DeleteBehavior.NoAction);
                ua.HasOne(x => x.Question).WithMany().HasForeignKey(x => x.QuestionId).OnDelete(DeleteBehavior.NoAction);
                ua.HasOne(x => x.Answer).WithMany().HasForeignKey(x => x.AnswerId).OnDelete(DeleteBehavior.NoAction);
            });
        }
    }
}
