using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace HXQ.NWBC_Assignment05_v50.Data.Models
{
    public class ErrorVm
    {
        public int ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
        public string Path { get; set; }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
