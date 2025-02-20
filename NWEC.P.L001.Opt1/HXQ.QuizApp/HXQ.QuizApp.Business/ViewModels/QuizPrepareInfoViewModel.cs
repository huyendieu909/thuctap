using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HXQ.QuizApp.Business.ViewModels
{
    public class QuizPrepareInfoViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Duration { get; set; }
        public string ThumbnailUrl { get; set; }
        public string QuizCode { get; set; }
        public UserViewModel User { get; set; }
    }
}
