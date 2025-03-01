using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCareAppointment.Business.Services
{
    public interface IBaseService<T>
    {
        //Đặt lịch hẹn thì add, bỏ hẹn thì delete
        Task<int> AddAync(T entity);
        Task<bool> UpdateAsync(T entity);
        bool Delete(Guid id);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> DeleteAsync(T entity);
        Task<T?> GetByIdAsync(Guid id);
        Task<IEnumerable<T>> GetAllAsync();
    }
}
