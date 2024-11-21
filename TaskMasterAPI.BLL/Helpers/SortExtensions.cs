using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TaskMasterAPI.Models.Bases;
using TaskMasterAPI.Models.Bases.Enums;

namespace TaskMasterAPI.BLL.Helpers;

public static class Sort
{
    public static async Task<IEnumerable<T>> PaginationAsync<T>(this IQueryable<T> source, int pageNumber, int take)
    {
        return await source.Skip(take * pageNumber - take).Take(take).ToListAsync();
    }

    public static IQueryable<TEntity> OrderByPropertyName<TEntity>(this IQueryable<TEntity> source, SortType sortType, bool desc) where TEntity : class
    {

        var command = desc ? "OrderByDescending" : "OrderBy";
        var type = typeof(TEntity);
        var property = type.GetProperty(sortType.ToString());
        var parameter = Expression.Parameter(type, "p");
        var propertyAccess = Expression.MakeMemberAccess(parameter, property);
        var orderByExpression = Expression.Lambda(propertyAccess, parameter);
        var resultExpression = Expression.Call(typeof(Queryable), command, new Type[] { type, property.PropertyType },
            source.Expression, Expression.Quote(orderByExpression));

        return source.Provider.CreateQuery<TEntity>(resultExpression);
    }
}