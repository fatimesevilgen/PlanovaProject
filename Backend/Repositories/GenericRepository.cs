using Core.Entites.Abstract;
using Microsoft.EntityFrameworkCore;
using Repositories.Abstract;
using System.Linq.Expressions;

namespace Repositories
{
	public class GenericRepository<T> : IGenericRepository<T> where T : class, IEntity
	{
		protected readonly DbContext _context;
		protected readonly DbSet<T> _dbSet;

		public GenericRepository(DbContext context)
		{
			_context = context;
			_dbSet = context.Set<T>();
		}


		public async Task<T> AddAsync(T entity, CancellationToken ct = default)
		{
			await _context.Set<T>().AddAsync(entity, ct);
			await _context.SaveChangesAsync(ct);
			return entity;
		}


		public async Task<List<T>> AddRangeAsync(List<T> entities, CancellationToken ct = default)
		{
			await _context.Set<T>().AddRangeAsync(entities, ct);
			await _context.SaveChangesAsync(ct);
			return entities;
		}


		public async Task<bool> ExistsAsync(int id, CancellationToken ct = default)
		{
			return await _context.Set<T>().AnyAsync(x => x.Id == id, ct);
		}

		public async Task<T> GetAsync(Expression<Func<T, bool>> predicate)
		{
			return await _context.Set<T>().FirstOrDefaultAsync(predicate);
		}

		public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? predicate = null, params Expression<Func<T, object>>[] includes)
        {

            //  Sorguyu oluştur
            IQueryable<T> query = _context.Set<T>().AsNoTracking();

           // Filtre varsa (örn: UserId == 5) uygula
          if (predicate != null)
          {
            query = query.Where(predicate);
          }

    
           // İstenen yan tabloları (Category, Progress vb.) sorguya ekle
          if (includes != null)
          {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
          }

           //  Veritabanına git ve dolu listeyi getir
           return await query.ToListAsync();  
        }

		public virtual async Task<T?> GetFirstAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
		{
			IQueryable<T> query = _dbSet;

			foreach (var include in includes)
			{
				query = query.Include(include);
			}

			return await query.FirstOrDefaultAsync(predicate);
		}


		public async Task<T?> GetByIdAsync(int id, CancellationToken ct = default)
		{
			return await _context.Set<T>().FindAsync(new object[] { id }, ct);
		}


		public async Task SoftDeleteAsync(int id, CancellationToken ct = default)
		{
			var entity = await _context.Set<T>().FindAsync(new object[] { id }, ct);
			if (entity != null)
			{
				entity.IsDeleted = true;
				entity.UpdatedDate = DateTime.UtcNow;

				_context.Set<T>().Update(entity);
				await _context.SaveChangesAsync(ct);
			}
		}


		public async Task<T> UpdateAsync(T entity, CancellationToken ct = default)
		{
			_context.Set<T>().Update(entity);
			await _context.SaveChangesAsync(ct);
			return entity;
		}

		public async Task<List<T>> UpdateRangeAsync(List<T> entites, CancellationToken ct = default)
		{
			_context.ChangeTracker.Clear();
			_context.Set<T>().UpdateRange(entites);
			await _context.SaveChangesAsync(ct);
			return entites;
		}
	}
}
