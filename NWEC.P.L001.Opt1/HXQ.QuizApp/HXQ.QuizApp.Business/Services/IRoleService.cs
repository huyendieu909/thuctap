using HXQ.QuizApp.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HXQ.QuizApp.Business.Services
{
    /* Note: không kế thừa IBaseService, lý do là vì user và role không có các thao tác CRUD bằng DbContext như bảng thông thường (Có thể thấy CreateAsync() thay vì AddAsync() chẩng hạn).   */
    public interface IRoleService
    {
        Task<Role?> GetByNameAsync(string roleName);
        Task<bool> CreateRoleAsync(Role role);
    }
}
