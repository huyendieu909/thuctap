using HealthCareAppointment.Data.Repositories;
using HealthCareAppointment.Models;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCareAppointment.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly HealthcareAppointmentContext db;
        private IDbContextTransaction transaction;
        public UnitOfWork(HealthcareAppointmentContext db)
        {
            this.db = db;
            AppointmentRepository = new GenericRepository<Appointment>(db);
            UserRepository = new GenericRepository<Users>(db);
            
        }

        public IGenericRepository<Appointment> AppointmentRepository { get; private set; }
        public IGenericRepository<Users> UserRepository { get; private set; }

        public HealthcareAppointmentContext Context => db;
        public IGenericRepository<TEntity> GenericRepository<TEntity>() where TEntity : class
        {
            return new GenericRepository<TEntity>(db);
        }

        public int SaveChanges()
        {
            return db.SaveChanges();
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await db.SaveChangesAsync(cancellationToken);
        }

        //Phương thức khởi tạo transaction
        public async Task BeginTransactionAsync()
        {
            if (transaction == null)
            {
                transaction = await db.Database.BeginTransactionAsync();
            }
        }

        //Phương thức để cam kết transaction
        public async Task CommitTransactionAsync()
        {
            try
            {
                await db.SaveChangesAsync();
                if (transaction != null) await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
            finally
            {
                if (transaction != null)
                {
                    await transaction.DisposeAsync();
                    transaction = null;
                }
            }
        }

        //Phương thức để rollback transaction
        public async Task RollbackTransactionAsync()
        {
            if (transaction != null)
            {
                await transaction.RollbackAsync();
                await transaction.DisposeAsync();
                transaction = null;
            }
        }

        //Giải phóng
        public void Dispose()
        {
            db.Dispose();
        }
    }
}
