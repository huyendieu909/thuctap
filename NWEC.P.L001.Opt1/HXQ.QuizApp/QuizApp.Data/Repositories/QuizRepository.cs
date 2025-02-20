using HXQ.QuizApp.Data.Context;
using HXQ.QuizApp.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HXQ.QuizApp.Data.Repositories
{
    public class QuizRepository : GenericRepository<Quiz>, IQuizRepository
    {
        //Generic muôn năm =)))
        public QuizRepository(QuizAppDbContext context) : base(context) { }
        
    }
}
