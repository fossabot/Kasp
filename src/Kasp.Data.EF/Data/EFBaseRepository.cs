using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Kasp.Data.EF.Extensions;
using Kasp.Data.EF.Helpers;
using Kasp.Data.Models.Helpers;
using Kasp.ObjectMapper.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Kasp.Data.EF.Data {
	public abstract class EFBaseRepository<TDbContext, TModel, TKey> : IEFBaseRepository<TModel, TKey>
		where TDbContext : DbContext
		where TModel : class, IModel<TKey>
		where TKey :  IEquatable<TKey> {
		protected EFBaseRepository(TDbContext db) {
			Db = db;
			Set = db.Set<TModel>();
		}

		public DbSet<TModel> Set { get; }
		public virtual IQueryable<TModel> BaseQuery => Set.AsQueryable();

		public virtual async ValueTask<bool> HasAsync(TKey id, CancellationToken cancellationToken = default) {
			return await BaseQuery.AnyAsync(x => x.Id.Equals(id), cancellationToken);
		}

		public virtual async ValueTask<bool> HasAsync(Expression<Func<TModel, bool>> filter, CancellationToken cancellationToken = default) {
			return await BaseQuery.AnyAsync(filter, cancellationToken);
		}


		public virtual async ValueTask<TModel> GetAsync(TKey id, CancellationToken cancellationToken = default) {
			return await BaseQuery.FirstOrDefaultAsync(x => x.Id.Equals(id), cancellationToken);
		}

		public virtual async ValueTask<TProject> GetAsync<TProject>(TKey id, CancellationToken cancellationToken = default) where TProject : IModel<TKey> {
			return await BaseQuery.MapTo<TProject>().FirstOrDefaultAsync(x => x.Id.Equals(id), cancellationToken);
		}

		public virtual async ValueTask<TModel> GetAsync(Expression<Func<TModel, bool>> filter, CancellationToken cancellationToken = default) {
			return await BaseQuery.FirstOrDefaultAsync(filter, cancellationToken);
		}

		public virtual async ValueTask<TProject> GetAsync<TProject>(Expression<Func<TModel, bool>> filter, CancellationToken cancellationToken = default) where TProject : IModel<TKey> {
			return await BaseQuery.Where(filter).MapTo<TProject>().FirstOrDefaultAsync(cancellationToken);
		}

		public virtual async ValueTask<IEnumerable<TModel>> ListAsync(CancellationToken cancellationToken = default) {
			return await BaseQuery.ToArrayAsync(cancellationToken);
		}

		public virtual async ValueTask<IEnumerable<TProject>> ListAsync<TProject>(CancellationToken cancellationToken = default) where TProject : IModel<TKey> {
			return await BaseQuery.MapTo<TProject>().ToArrayAsync(cancellationToken);
		}

		public virtual async ValueTask<IEnumerable<TModel>> ListAsync(Expression<Func<TModel, bool>> expression, CancellationToken cancellationToken = default) {
			return await BaseQuery.Where(expression).ToArrayAsync(cancellationToken);
		}

		public virtual async ValueTask<IEnumerable<TProject>> ListAsync<TProject>(Expression<Func<TModel, bool>> expression, CancellationToken cancellationToken = default)
			where TProject : IModel<TKey> {
			return await BaseQuery.Where(expression).MapTo<TProject>().ToArrayAsync(cancellationToken);
		}

		public virtual async ValueTask<IPagedList<TModel>> PagedListAsync(Expression<Func<TModel, bool>> expression, int page = 1, int count = 20, CancellationToken cancellationToken = default) {
			return await BaseQuery.Where(expression).ToPagedListAsync(count, page, cancellationToken);
		}

		public virtual async ValueTask<IPagedList<TProject>> PagedListAsync<TProject>(Expression<Func<TModel, bool>> expression, int page = 1, int count = 20,
			CancellationToken cancellationToken = default)
			where TProject : IModel<TKey> {
			return await BaseQuery.Where(expression).MapTo<TProject>().ToPagedListAsync(count, page, cancellationToken);
		}

		public virtual async ValueTask<IPagedList<TModel>> PagedListAsync(int page = 1, int count = 20, CancellationToken cancellationToken = default) {
			return await BaseQuery.ToPagedListAsync(count, page, cancellationToken);
		}


		public virtual async ValueTask<IPagedList<TProject>> PagedListAsync<TProject>(int page = 1, int count = 20, CancellationToken cancellationToken = default) where TProject : IModel<TKey> {
			return await BaseQuery.MapTo<TProject>().ToPagedListAsync(count, page, cancellationToken);
		}


		public virtual async ValueTask AddAsync(TModel model, CancellationToken cancellationToken = default) {
			Set.Add(model);
		}

		public virtual async ValueTask AddAsync(IEnumerable<TModel> model, CancellationToken cancellationToken = default) {
			await Set.AddRangeAsync(model, cancellationToken);
		}

		public virtual void Update(TModel model) {
			Set.Update(model);
		}

		public virtual async ValueTask RemoveAsync(TKey id, CancellationToken cancellationToken = default) {
			var model = await GetAsync(id, cancellationToken);
			Set.Remove(model);
		}

		public virtual void Remove(TModel model) {
			Set.Remove(model);
		}

		public async ValueTask<int> SaveAsync(CancellationToken cancellationToken = default) {
			return await Db.SaveChangesAsync(cancellationToken);
		}


		public TDbContext Db { get; }
	}

	public abstract class EFBaseRepository<TDbContext, TModel> : EFBaseRepository<TDbContext, TModel, int>, IEFBaseRepository<TModel> where TModel : class, IModel where TDbContext : DbContext {
		protected EFBaseRepository(TDbContext db) : base(db) {
		}
	}
}