using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HXQ.NWBC_Assignment05_v50.Data.Models
{
    public class RegisterVm
    {
        [Required(ErrorMessage = "Username là trường bắt buộc!")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Email là trường bắt buộc!")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password là trường bắt buộc!")]
        [MinLength(8, ErrorMessage = "Password tối thiểu 8 ký tự, bao gồm chữ cái!")]
        public string Password { get; set; }

        [JsonIgnore]
        public string Role { get; set; } = "User";
    }
}
