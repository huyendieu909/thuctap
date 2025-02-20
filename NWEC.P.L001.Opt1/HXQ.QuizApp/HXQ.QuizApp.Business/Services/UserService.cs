using HXQ.QuizApp.Data.Models;
using HXQ.QuizApp.Data.Repositories;
using HXQ.QuizApp.Data.UnitOfWork;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HXQ.QuizApp.Business.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;
        private readonly IRoleRepository roleRepository;
        private readonly UserManager<User> userManager;

        public UserService(IUserRepository userRepository, IRoleRepository roleRepository, UserManager<User> userManager)
        {
            this.userRepository = userRepository;
            this.roleRepository = roleRepository;
            this.userManager = userManager;
        }

        public async Task<bool> CreateUserAsync(User user, string password)
        {
            var result = await userManager.CreateAsync(user, password);
            return result.Succeeded;
        }

        public Task<User?> GetByEmailAsync(string email)
        {
            return userRepository.GetByEmailAsync(email);
        }

        public Task<User?> GetByIdAsync(Guid id)
        {
            return userRepository.GetByIdAsync(id);
        }

        public async Task<bool> AssignRoleToUserAsync(Guid userId, string roleName)
        {
            //Tìm user 
            var user = await userRepository.GetByIdAsync(userId);
            if (user == null) return false;

            //tìm role
            var role = await roleRepository.GetByNameAsync(roleName);
            if (role == null) return false;

            //sét role
            var result = await userManager.AddToRoleAsync(user, role.Name!);
            return result.Succeeded;
        }

        public async Task<IList<string>> GetUserRolesAsync(Guid userId)
        {
            var user = await userRepository.GetByIdAsync(userId);
            if (user == null) return new List<string>();
            return await userManager.GetRolesAsync(user);
        }
    }
}
