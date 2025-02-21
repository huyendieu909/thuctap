using System.ComponentModel.DataAnnotations;

namespace HoangXuanQuy.OnlinePainting.MVC.Models
{
    public class LoginRegisterViewModel
    {
        public LoginViewModel Login { get; set; } = new LoginViewModel();
        public RegisterViewModel Register { get; set; } = new RegisterViewModel();
    }
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Không để trống!"), EmailAddress]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Không để trống!"), DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {

        [Required(ErrorMessage = "Không để trống!"), EmailAddress]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Không để trống!"), Phone]
        public string PhoneNumber { get; set; } = null!;

        [Required(ErrorMessage = "Không để trống!")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Không để trống!")]
        public string Address { get; set; } = null!;

        [Required(ErrorMessage = "Không để trống!"), DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        [Required(ErrorMessage = "Không để trống!"), DataType(DataType.Password), Compare("Password")]
        public string ConfirmPassword { get; set; } = null!;
    }
}
