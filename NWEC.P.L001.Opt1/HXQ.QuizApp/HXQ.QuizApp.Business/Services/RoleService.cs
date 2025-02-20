using HXQ.QuizApp.Data.Models;
using HXQ.QuizApp.Data.Repositories;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HXQ.QuizApp.Business.Services
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository roleRepository;
        private readonly RoleManager<Role> roleManager;

        public RoleService(IRoleRepository roleRepository, RoleManager<Role> roleManager)
        {
            this.roleRepository = roleRepository;
            this.roleManager = roleManager;
        }   

        public async Task<bool> CreateRoleAsync(Role role)
        {
            var result = await roleManager.CreateAsync(role);
            return result.Succeeded;
        }

        public Task<Role?> GetByNameAsync(string roleName)
        {
            return roleRepository.GetByNameAsync(roleName);
        }
    }
}
