using HXQ.QuizApp.Business.Services;
using HXQ.QuizApp.Data.Models;
using HXQ.QuizApp.Data.Repositories;
using HXQ.QuizApp.Data.UnitOfWork;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HXQ.QuizApp.Tests
{

    [TestFixture]
    public class QuizServiceTests
    {
        private Mock<IUnitOfWork> _unitOfWorkMock;
        private Mock<IQuizRepository> _quizRepositoryMock;
        private Mock<ILogger<QuizService>> _loggerMock;
        private QuizService _quizService;

        [SetUp]
        public void Setup()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _quizRepositoryMock = new Mock<IQuizRepository>();
            _loggerMock = new Mock<ILogger<QuizService>>();
            _loggerMock.Setup(x => x.Log(
                It.IsAny<LogLevel>(),
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                (Func<It.IsAnyType, Exception, string>)It.IsAny<object>())
            );


            _unitOfWorkMock.Setup(u => u.GenericRepository<Quiz>()).Returns(_quizRepositoryMock.Object);


            _quizService = new QuizService(_unitOfWorkMock.Object);
        }

        [Test]
        public async Task GetAllQuizzesAsync_ShouldReturnAllQuizzes()
        {
            // Arrange
            var mockQuizzes = new List<Quiz>
            {
                new Quiz { Id = Guid.NewGuid(), Title = "C# Basics" },
                new Quiz { Id = Guid.NewGuid(), Title = "ASP.NET Core" }
            };
            _quizRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(mockQuizzes);

            // Act
            var result = await _quizService.GetAllAsync();

            // Assert

            /* Note: GPT dùng lệnh Assert.IsNotNull(result); báo lỗi. Lệnh này đã bị lỗi thời, nếu vẫn muốn dùng thì thêm Classic vào trước như các cmt bên dưới, doc nói nên sử dụng lệnh Assert kiểu mới */

            //ClassicAssert.IsNotNull(result);
            Assert.That(result, Is.Not.Null);

            //ClassicAssert.AreEqual(2, result.Count());
            Assert.That(result.Count(), Is.EqualTo(2));
            _quizRepositoryMock.Verify(r => r.GetAllAsync(), Times.Once);
        }

        [Test]
        public async Task GetQuizByIdAsync_QuizNotFound_ShouldReturnNull()
        {
            // Arrange
            _quizRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                               .Returns(Task.FromResult<Quiz>(null)); // Đảm bảo trả về null đúng cách

            // Act
            var result = await _quizService.GetByIdAsync(Guid.NewGuid());

            // Assert
            Assert.That(result, Is.Null);
        }
    }
}
