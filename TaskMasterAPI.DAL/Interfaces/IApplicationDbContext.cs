using Microsoft.EntityFrameworkCore;
using TaskMasterAPI.DAL.Enums;

namespace TaskMasterAPI.DAL.Interfaces;

public interface IApplicationDbContext
{
    DbSet<T> Set<T>() where T : class;
    Task SaveChangesAsync<T>(T entity, SaveChangeType type, CancellationToken cancellationToken = default);
    Task InvokeTransactionAsync(Func<Task> action, CancellationToken cancellationToken = default);
    Task<T> InvokeTransactionAsync<T>(Func<Task<T>> action, CancellationToken cancellationToken = default);
}