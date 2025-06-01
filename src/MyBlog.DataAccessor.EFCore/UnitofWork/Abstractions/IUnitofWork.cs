using Microsoft.EntityFrameworkCore;

namespace MyBlog.DataAccessor.EFCore.UnitofWork.Abstractions
{
    public interface IUnitOfWork<TContext> where TContext : DbContext
    {
        void BeginTransaction();
        void ClearChangeTracker();
        Task<int> CommitAsync();
        Task CommitTransactionAsync();
        void Dispose();
        Task<int> ExecuteSqlCommandAsync(string sql, params object[] parameters);
        IRepository<T> GetRepository<T>() where T : class;
        void Rollback();
        Task RollbackTransactionAsync();
    }
}