using HealthCareAppointment.Data.Repositories;
using HealthCareAppointment.Data.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCareAppointment.Business.Services
{
    public class BaseService<T> : IBaseService<T> where T : class
    {
        protected readonly IUnitOfWork unitOfWork;
        protected readonly IGenericRepository<T> repository;

        public BaseService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            this.repository = unitOfWork.GenericRepository<T>();
        }

        public async Task<int> AddAync(T entity)
        {
            await repository.AddAsync(entity);
            return await unitOfWork.SaveChangesAsync();
        }
        public async Task<bool> UpdateAsync(T entity)
        {
            repository.Update(entity);
            return await unitOfWork.SaveChangesAsync() > 0;
        }

        public bool Delete(Guid id)
        {
            repository.Delete(id);
            return unitOfWork.SaveChanges() > 0;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            await repository.DeleteAsync(id);
            return await unitOfWork.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(T entity)
        {
            await repository.DeleteAsync(entity);
            return await unitOfWork.SaveChangesAsync() > 0;
        }
        public async Task<T?> GetByIdAsync(Guid id)
        {
            return await repository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await repository.GetAllAsync();
        }

    }

}
