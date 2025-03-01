using HoangXuanQuy.OnlinePainting.Data.Context;
using HoangXuanQuy.OnlinePainting.Data.Repository;
using HoangXuanQuy.OnlinePainting.Data.Models;

namespace HoangXuanQuy.OnlinePainting.Data.UnitOfWork{
    public interface IUnitOfWork : IDisposable 
    {
        OnlinePaintingContext Context {get;}
        IGenericRepository<Artist> Artists {get;}
        IGenericRepository<Category> Categories {get;}
        IGenericRepository<Comment> Comments {get;}
        IGenericRepository<Customer> Customers {get;}
        IGenericRepository<Order> Orders {get;}
        IGenericRepository<Painting> Paintings {get;}
        IGenericRepository<TEntity> GenericRepository<TEntity>() where TEntity : class;
        int SaveChanges();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();

    }
}