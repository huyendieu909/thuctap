using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HXQ.NWBC_Assignment05_v50.Data.Models
{
    public class LoginVm
    {
        [Required(ErrorMessage = "Username không bỏ trống!")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password không bỏ trống!")]
        [MinLength(8, ErrorMessage = "Password tối thiểu 8 ký tự, bao gồm chữ cái!")]
        public string Password { get; set; }
    }
}
