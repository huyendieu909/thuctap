using HXQ.QuizApp.Data.Context;
using HXQ.QuizApp.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HXQ.QuizApp.Data.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        private readonly QuizAppDbContext db;
        public UserRepository(QuizAppDbContext context) : base(context)
        {
            db = context;
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await db.Users.FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task<IList<string>> GetUserRoleAsync(User user)
        {
            return await db.UserRoles
                .Where(ur => ur.UserId == user.Id)
                .Select(ur => db.Roles.Where(r => r.Id == ur.RoleId).Select(r => r.Name).FirstOrDefault()!)
                .ToListAsync();
        }
    }
}
