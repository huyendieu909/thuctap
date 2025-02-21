using HoangXuanQuy.OnlinePainting.Data.Models;
using HoangXuanQuy.OnlinePainting.MVC.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HoangXuanQuy.OnlinePainting.MVC.Controllers
{
    public class LoginController : Controller
    {
        private readonly UserManager<Customer> _userManager;
        private readonly SignInManager<Customer> _signInManager;
        public LoginController(UserManager<Customer> userManager, SignInManager<Customer> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        //Trang đăng nhập đăng ký
        public IActionResult Index()
        {
            return View();
        }

        //Xử lý đăng nhập
        [HttpPost]
        public async Task<IActionResult> Login(LoginRegisterViewModel model)
        {
            ModelState.Clear();
            TryValidateModel(model.Login);
            if (!ModelState.IsValid)
            {
                Console.WriteLine("Model input is invalid");
                foreach (var modelState in ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        Console.WriteLine(error.ErrorMessage);
                    }
                }
                return View("Index", model);
            }

            var user = await _userManager.FindByEmailAsync(model.Login.Email);
            if (user == null)
            {
                ModelState.AddModelError("Login.Email", "Email không tồn tại");
                return View("Index", model);
            }

            var result = await _signInManager.PasswordSignInAsync(user, model.Login.Password, model.Login.RememberMe, false);
            if (result.Succeeded) return RedirectToAction("Index", "Home");

            ModelState.AddModelError("Login.Password", "Mật khẩu không đúng");
            return View("Index", model);
        }

        //Xử lý đăng ký
        [HttpPost]
        public async Task<IActionResult> Register(LoginRegisterViewModel model)
        {
            ModelState.Clear();
            TryValidateModel(model.Register);
            if (!ModelState.IsValid)
            {
                Console.WriteLine("Model input is invalid");
                foreach (var modelState in ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        Console.WriteLine(error.ErrorMessage);
                    }
                }
                return View("Index", model);
            }

            var checkExists = await _userManager.FindByEmailAsync(model.Register.Email);
            if (checkExists != null)
            {
                ModelState.AddModelError("Register.Email", "Email đã tồn tại");
                return View("Index", model);
            }

            var user = new Customer
            {
                Name = model.Register.Name,
                Email = model.Register.Email,
                UserName = model.Register.Email,
                Address = model.Register.Address,
                PhoneNumber = model.Register.PhoneNumber
            };

            var result = await _userManager.CreateAsync(user, model.Register.Password);
            if (result.Succeeded)
            {
                var roleResult = await _userManager.AddToRoleAsync(user, "User");
                //thêm role thất bại
                if (!roleResult.Succeeded)
                {
                    foreach (var error in roleResult.Errors)
                    {
                        ModelState.AddModelError("Register.Password", error.Description);
                    }
                }
                //thành công thì đăng nhập luôn
                var signInResult = await _signInManager.PasswordSignInAsync(user, model.Register.Password, false, false);
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("Register.Password", "Đăng ký thất bại");
            return View("Index", model);
        }

        //Xử lý đăng xuất
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        //Trang lỗi quyền truy cập
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
