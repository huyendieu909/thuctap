using HealthCareAppointment.Data.Repositories;
using HealthCareAppointment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCareAppointment.Data.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        HealthcareAppointmentContext Context { get; }
        IGenericRepository<Appointment> AppointmentRepository { get; } 
        IGenericRepository<Users> UserRepository { get; }
        IGenericRepository<TEntity> GenericRepository<TEntity>() where TEntity : class;

        int SaveChanges();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}
