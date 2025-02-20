using HXQ.QuizApp.Business.ViewModels;
using HXQ.QuizApp.Data.Models;
using HXQ.QuizApp.Data.UnitOfWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HXQ.QuizApp.Business.Services
{
    public class QuizService : BaseService<Quiz>, IQuizService
    {
        private readonly UserManager<User> userManager;
        private readonly ILogger<QuizService> logger;

        public QuizService(IUnitOfWork unitOfWork) : base(unitOfWork){
            this.logger = logger;
        }

        public async Task<IEnumerable<Quiz>> GetQuizzesByDurationAsync(int duration)
        {
            logger.LogInformation("Đang lấy danh sách quiz...");
            var quizzes = await unitOfWork.QuizRepository.GetQuery(q => q.Duration == duration).ToListAsync();
            logger.LogInformation($"Đã lấy {quizzes.Count()} quiz");
            return quizzes;
        }

        public async Task<PaginatedResult<Quiz>> GetQuizzesPageAsync(int pageIndex, int pageSize)
        {
            var query = unitOfWork.QuizRepository.GetQuery();
            return await PaginatedResult<Quiz>.CreateAsync(query, pageIndex, pageSize);
        }

        public async Task<QuizPrepareInfoViewModel?> PrepareQuizForUserAsync(PrepareQuizViewModel prepareQuizViewModel)
        {
            // Tìm bản ghi UserQuiz dựa trên QuizId, UserId và QuizCode (QuizCode nằm trong UserQuiz)
            var userQuiz = await unitOfWork.UserQuizRepository.GetQuery()
                .Where(uq => uq.QuizId == prepareQuizViewModel.QuizId
                          && uq.UserId == prepareQuizViewModel.UserId
                          && uq.QuizCode == prepareQuizViewModel.QuizCode)
                .Include(uq => uq.Quiz)   // Include Quiz để lấy thông tin Quiz
                .FirstOrDefaultAsync();

            if (userQuiz == null) return null;

            // Lấy thông tin User từ repository
            var user = await unitOfWork.UserRepository.GetByIdAsync(prepareQuizViewModel.UserId);
            if (user == null) return null;
            

            // Lấy danh sách vai trò của User thông qua UserManager
            var roles = await userManager.GetRolesAsync(user);

            // Xây dựng UserViewModel
            var userViewModel = new UserViewModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                DisplayName = $"{user.FirstName} {user.LastName}",
                Email = user.Email,
                UserName = user.UserName,
                PhoneNumber = user.PhoneNumber,
                DateOfBirth = user.DateOfBirth,
                Avatar = user.Avatar,
                IsActive = user.IsActive,
                Roles = roles.ToList()
            };

            // Xây dựng QuizPrepareInfoViewModel, lấy QuizCode từ UserQuiz
            var quiz = userQuiz.Quiz;
            var quizPrepareInfo = new QuizPrepareInfoViewModel
            {
                Id = quiz.Id,
                Title = quiz.Title,
                Description = quiz.Description,
                Duration = quiz.Duration,
                ThumbnailUrl = quiz.ThumbnailUrl,
                // Lấy QuizCode từ UserQuiz
                QuizCode = userQuiz.QuizCode,  
                User = userViewModel
            };

            return quizPrepareInfo;
        }

        public async Task<QuizResultViewModel> GetQuizResultAsync(GetQuizResultViewModel model)
        {
            var totalQuestions = await unitOfWork.UserQuizRepository.GetQuery()
            .Where(q => q.QuizId == model.QuizId)
            .CountAsync();

            var correctAnswers = await unitOfWork.UserAnswerRepository.GetQuery()
                .Where(ua => ua.UserQuiz.UserId == model.UserId && ua.Question.Id == model.QuizId && ua.Answer.IsCorrect)
                .CountAsync();

            return new QuizResultViewModel
            {
                QuizId = model.QuizId,
                UserId = model.UserId,
                CorrectAnswers = correctAnswers,
                TotalQuestions = totalQuestions,
                Score = totalQuestions > 0 ? (double)correctAnswers / totalQuestions * 100 : 0
            };
        }

        public async Task<bool> SubmitQuizAsync(QuizSubmissionViewModel model)
        {
            // Lấy đối tượng UserQuiz dựa trên QuizId và UserId
            var userQuiz = await unitOfWork.UserQuizRepository.GetQuery()
                                .FirstOrDefaultAsync(uq => uq.QuizId == model.QuizId && uq.UserId == model.UserId);

            // Nếu không tìm thấy, tạo mới đối tượng UserQuiz
            if (userQuiz == null)
            {
                // Tạo mới UserQuiz nếu không có
                userQuiz = new UserQuiz
                {
                    Id = Guid.NewGuid(),
                    QuizId = model.QuizId,
                    UserId = model.UserId,
                    QuizCode = Guid.NewGuid().ToString(), // Bạn có thể sinh ra mã phù hợp
                    StartedAt = DateTime.UtcNow
                    // FinishedAt có thể được set sau khi quiz hoàn thành.
                };
                await unitOfWork.UserQuizRepository.AddAsync(userQuiz);
                await unitOfWork.SaveChangesAsync();
            }

            // Duyệt qua từng đáp án do user gửi lên
            foreach (var submission in model.UserAnswers)
            {
                // Tạo đối tượng UserAnswer với UserQuizId từ userQuiz vừa tìm/thêm
                var userAnswer = new UserAnswer
                {
                    Id = Guid.NewGuid(),
                    UserQuizId = userQuiz.Id,
                    QuestionId = submission.QuestionId,
                    AnswerId = submission.AnswerId
                };

                // Tính toán thuộc tính IsCorrect dựa trên dữ liệu trong Answer
                var answerEntity = await unitOfWork.AnswerRepository.GetByIdAsync(submission.AnswerId);
                userAnswer.IsCorrect = (answerEntity != null && answerEntity.IsCorrect);

                await unitOfWork.UserAnswerRepository.AddAsync(userAnswer);
            }

            return await unitOfWork.SaveChangesAsync() > 0;
        }

        public async Task<QuizForTestViewModel?> TakeQuizAsync(TakeQuizViewModel model)
        {
            // Tìm UserQuiz (chứa QuizCode) dựa trên UserId & QuizId
            var userQuiz = await unitOfWork.UserQuizRepository
                .GetQuery(uq => uq.UserId == model.UserId && uq.QuizId == model.QuizId)
                .Include(uq => uq.Quiz)
                    .ThenInclude(q => q.QuizQuestions)
                        .ThenInclude(qq => qq.Question)
                            .ThenInclude(q => q.Answers)
                .FirstOrDefaultAsync();

            // Nếu không tìm thấy, có thể trả về null hoặc throw exception tùy theo nghiệp vụ
            if (userQuiz == null)
                return null;

            // Lấy Quiz từ UserQuiz
            var quizEntity = userQuiz.Quiz;
            if (quizEntity == null)
                return null;

            // Lấy danh sách Question qua bảng trung gian QuizQuestions
            // Sắp xếp theo thuộc tính Order (nếu có)
            var questions = quizEntity.QuizQuestions
                .OrderBy(qq => qq.Order)
                .Select(qq => qq.Question)
                .ToList();

            // Tạo đối tượng ViewModel trả về
            var quizForTest = new QuizForTestViewModel
            {
                Id = quizEntity.Id,
                Title = quizEntity.Title,
                Description = quizEntity.Description,
                // Lấy QuizCode từ UserQuiz
                QuizCode = userQuiz.QuizCode, 
                // Thời điểm bắt đầu
                StartTime = DateTime.UtcNow,   
                Duration = quizEntity.Duration,

                Questions = questions.Select(q => new QuestionForTestViewModel
                {
                    Id = q.Id,
                    Content = q.Content,
                    // Vì QuestionType là enum, ta dùng .ToString() để chuyển sang chuỗi
                    QuestionType = q.QuestionType.ToString(),
                    Answers = q.Answers.Select(a => new AnswerForTestViewModel
                    {
                        Id = a.Id,
                        Content = a.Content
                    }).ToList()
                }).ToList()
            };

            return quizForTest;
        }


    }
}
