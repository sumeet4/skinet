using Core.Entities;
using Core.Specification;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class SpecificationEvaluator<TEntity> where TEntity : BaseEntity
    {
        public static IQueryable<TEntity> GetQuery (IQueryable<TEntity> inputQuery, ISpecification<TEntity> spec)
        {
            var query = inputQuery;

            if (spec.Criteria != null)
            {
                query = query.Where(spec.Criteria); //ex: - p => p.ProductType = 1
            }
            if (spec.OrderBy != null)
            {
                query = query.OrderBy(spec.OrderBy); //ex: - p => p.ProductType = 1
            }
            if (spec.OrderByDescending != null)
            {
                query = query.OrderByDescending(spec.OrderByDescending); //ex: - p => p.ProductType = 1
            }
            if(spec.IsPaginEnabled)
            {
                query = query.Skip(spec.Skip).Take(spec.Take);

            }
            query = spec.Includes.Aggregate(query,(current, include) => current.Include(include));

            return query;
            
        }
    }
}