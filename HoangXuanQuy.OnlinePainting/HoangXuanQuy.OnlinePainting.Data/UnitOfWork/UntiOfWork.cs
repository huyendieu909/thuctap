using System.Data;
using System.Diagnostics.Contracts;
using HoangXuanQuy.OnlinePainting.Data.Context;
using HoangXuanQuy.OnlinePainting.Data.Models;
using HoangXuanQuy.OnlinePainting.Data.Repository;
using Microsoft.EntityFrameworkCore.Storage;

namespace HoangXuanQuy.OnlinePainting.Data.UnitOfWork {
    public class UnitOfWork : IUnitOfWork
    {
        private readonly OnlinePaintingContext db;
        private IDbContextTransaction transaction;

        public UnitOfWork (OnlinePaintingContext context) {
            db = context;
            Artists = new GenericRepository<Artist>(db);
            Categories = new GenericRepository<Category>(db);
            Comments = new GenericRepository<Comment>(db);
            Customers = new GenericRepository<Customer>(db);
            Orders = new GenericRepository<Order>(db);
            Paintings = new GenericRepository<Painting>(db);
        }

        public OnlinePaintingContext Context => db;

        public IGenericRepository<Artist> Artists {get; private set;}

        public IGenericRepository<Category> Categories {get; private set;}

        public IGenericRepository<Comment> Comments {get; private set;}

        public IGenericRepository<Customer> Customers {get; private set;}

        public IGenericRepository<Order> Orders {get; private set;}

        public IGenericRepository<Painting> Paintings {get; private set;}

        public IGenericRepository<TEntity> GenericRepository<TEntity>() where TEntity : class
        {
            return new GenericRepository<TEntity>(db);
        }       

        public async Task BeginTransactionAsync()
        {
            if (transaction == null)
                transaction = await db.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            try {
                await db.SaveChangesAsync();
                if (transaction != null) await transaction.CommitAsync();
            }
            catch {
                await transaction.RollbackAsync();
                throw;
            }
            finally {
                if (transaction != null) {
                    await transaction.DisposeAsync();
                    transaction = null;
                }
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if (transaction != null){
                await transaction.RollbackAsync();
                await transaction.DisposeAsync();
                transaction = null;
            }
        }

        public int SaveChanges()
        {
            return db.SaveChanges();
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await db.SaveChangesAsync();
        }        
        
        public void Dispose()
        {
            db.Dispose();
        }
    }
}