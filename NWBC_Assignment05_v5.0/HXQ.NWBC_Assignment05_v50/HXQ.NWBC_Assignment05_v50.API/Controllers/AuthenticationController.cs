using HXQ.NWBC_Assignment05_v50.Data.Context;
using HXQ.NWBC_Assignment05_v50.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HXQ.NWBC_Assignment05_v50.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        #region khai báo, khởi tạo
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SchoolContext db;
        private readonly IConfiguration _configuration;

        public AuthenticationController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, SchoolContext db, IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            this.db = db;
            _configuration = configuration;
        } 
        #endregion

        #region đăng ký
        //Đăng ký
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterVm registerVm)
        {
            //dòng này kiểm tra điều kiện của model, chính là mấy cái annotation [Required], [MinLength] [EmailAddress]
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userEmailExists = await _userManager.FindByEmailAsync(registerVm.Email);
            if (userEmailExists != null)
            {
                return BadRequest($"Người dùng có email {registerVm.Email} đã tồn tại!");
            }
            var usernameExists = await _userManager.FindByNameAsync(registerVm.Username);
            if (usernameExists != null)
            {
                return BadRequest($"Người dùng có username {registerVm.Username} đã tồn tại!");
            }

            var newUser = new ApplicationUser { UserName = registerVm.Username, Email = registerVm.Email, Name = registerVm.Username };
            var result = await _userManager.CreateAsync(newUser, registerVm.Password);
            
            //báo lỗi
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return BadRequest($"Không thể tạo người dùng! {errors}");
            }

            /*Note: nếu viết thế này thì ai cũng có thể thêm tài khoản admin, như vậy thì rất lmao=)) nên mặc định thêm sẽ là user, cần tạo acc admin thì uncomment và bỏ [JsonIgnore] trong RegisterVM.cs đi. acc admin cũng có thể tạo thủ công, xem program.cs*/
            //Gán role
            //if (!string.IsNullOrEmpty(registerVm.Role))
            //{
            //    bool roleExists = await _roleManager.RoleExistsAsync(registerVm.Role);
            //    if (roleExists)
            //    {
            //        await _userManager.AddToRoleAsync(newUser, registerVm.Role);
            //    }
            //    else
            //    {
            //        return BadRequest("Role không hợp lệ!");
            //    }
            //}

            //mặc định user
            await _userManager.AddToRoleAsync(newUser, "User");


            return Created(nameof(Register), $"Đã thêm người dùng {registerVm.Email}!");
        }
        #endregion

        #region đăng nhập
        //Đăng nhập, dùng username
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginVm loginVm)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var user = await _userManager.FindByNameAsync(loginVm.Username);
                if (user != null && await _userManager.CheckPasswordAsync(user, loginVm.Password))
                {
                    // Tạo JWT token
                    var token = await GenerateJwtToken(user);
                    return Ok(new { Token = token });
                }
                return Unauthorized("Username hoặc password không hợp lệ!");
            }
            catch (Exception e)
            {
                return BadRequest($"Lỗi server khi đăng nhập: {e.Message}");
            }
        }
        #endregion

        #region Lấy thông tin tài khoản
        [HttpGet("all-accounts")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllAccounts()
        {
            var users = await _userManager.Users.ToListAsync();
            var userList = users.Select(user => new
            {
                user.Id,
                user.UserName,
                user.Email,
                Roles = _userManager.GetRolesAsync(user).Result
            });

            return Ok(userList);
        } 
        #endregion

        #region hàm gen token jwt
        private async Task<AuthResultVm> GenerateJwtToken(ApplicationUser user)
        {
            var authClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            //Lấy danh sách Role của user
            var userRoles = await _userManager.GetRolesAsync(user);
            foreach (var role in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                expires: DateTime.UtcNow.AddMinutes(10), // 5 - 10mins
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );
            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
            var refreshToken = new RefreshToken()
            {
                JwtId = token.Id,
                IsRevoked = false,
                UserId = user.Id,
                DateAdded = DateTime.UtcNow,
                //6 tháng token hết hạn
                DateExpire = DateTime.UtcNow.AddMonths(6),
                Token = Guid.NewGuid().ToString() + "-" + Guid.NewGuid().ToString()
            };
            await db.RefreshTokens.AddAsync(refreshToken);
            await db.SaveChangesAsync();
            var response = new AuthResultVm()
            {
                Token = jwtToken,
                RefreshToken = refreshToken.Token,
                ExpiresAt = token.ValidTo
            };
            return response;
        } 
        #endregion
    }
}
