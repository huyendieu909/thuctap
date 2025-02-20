using HXQ.QuizApp.Data.Context;
using HXQ.QuizApp.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace HXQ.QuizApp.Data.Repositories
{
    public class RoleRepository : GenericRepository<Role>, IRoleRepository 
    {
        public readonly QuizAppDbContext db;
        public RoleRepository(QuizAppDbContext context) : base(context)
        {
            db = context;
        }

        public async Task<Role?> GetByNameAsync(string roleName)
        {
            return await db.Roles.FirstOrDefaultAsync(r => r.Name == roleName);
        }
    }
}
