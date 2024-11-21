using System.Reflection;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TaskMasterAPI.DAL.Enums;
using TaskMasterAPI.DAL.Interfaces;
using TaskMasterAPI.Models.Clients;

namespace TaskMasterAPI.DAL.Context;

public class AppDbContext(DbContextOptions<AppDbContext> options)
    : IdentityDbContext<Client>(options), IApplicationDbContext
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }

    async Task IApplicationDbContext.SaveChangesAsync<T>(T entity,
        SaveChangeType type,
        CancellationToken cancellationToken = default)
    {
        switch (type)
        {
            case SaveChangeType.Add:
                await AddAsync(entity!, cancellationToken);
                break;
            case SaveChangeType.Update:
                Update(entity!);
                break;
            case SaveChangeType.Delete:
                Remove(entity!);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }

        await SaveChangesAsync(cancellationToken);
    }

    public async Task InvokeTransactionAsync(Func<Task> action, CancellationToken cancellationToken = default)
    {
        if (this.Database.CurrentTransaction != null)
        {
            await action();
            return;
        }

        using var transaction = await this.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            await action();
            await transaction.CommitAsync(cancellationToken);
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    public async Task<T> InvokeTransactionAsync<T>(Func<Task<T>> action, CancellationToken cancellationToken = default)
    {
        if (this.Database.CurrentTransaction != null)
        {
            return await action();
        }
        else
        {
            using var transaction = await this.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                var result = await action();
                await transaction.CommitAsync(cancellationToken);
                return result;
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }
    }
}