using Core.Entites.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Abstract
{
	public interface IGenericRepository<T> where T : class, IEntity
	{
		Task<T> GetAsync(Expression<Func<T, bool>> predicate);
		Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? predicate = null, params Expression<Func<T, object>>[] includes);
		Task<T?> GetByIdAsync(int id, CancellationToken ct = default);
		Task<T?> GetFirstAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);
		Task<bool> ExistsAsync(int id, CancellationToken ct = default);
		Task<T> AddAsync(T entity, CancellationToken ct = default);
		Task<List<T>> AddRangeAsync(List<T> entities, CancellationToken ct = default);
		Task<T> UpdateAsync(T entity, CancellationToken ct = default);
		Task<List<T>> UpdateRangeAsync(List<T> entites, CancellationToken ct = default);
		Task SoftDeleteAsync(int id, CancellationToken ct = default);
	}
}
