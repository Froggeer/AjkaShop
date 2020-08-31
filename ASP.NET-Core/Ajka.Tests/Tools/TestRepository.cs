using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ajka.DAL.Model.Interfaces;
using Arch.EntityFrameworkCore.UnitOfWork;

namespace Ajka.Tests.Tools
{
    public class TestRepository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity<int>
    {
        public List<TEntity> Repository { get; set; }

        public TestRepository()
        {
            Repository = new List<TEntity>();
        }

        public void ChangeTable(string table)
        {
            throw new NotImplementedException();
        }

        public int Count(System.Linq.Expressions.Expression<Func<TEntity, bool>> predicate = null)
        {
            throw new NotImplementedException();
        }

        public void Delete(object id)
        {
            var testEntity = Repository.First(i => i.Id == (int)id);
            if (testEntity != null)
            {
                Repository.Remove(testEntity);
            }
        }

        public void Delete(TEntity entity)
        {
            Repository.Remove(entity);
        }

        public void Delete(params TEntity[] entities)
        {
            foreach(var entity in entities)
            {
                Delete(entity.Id);
            }
        }

        public void Delete(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                Delete(entity.Id);
            }
        }

        public TEntity Find(params object[] keyValues)
        {
            return Repository.Where(x => keyValues.Contains(x.Id)).FirstOrDefault();
        }

        public ValueTask<TEntity> FindAsync(params object[] keyValues)
        {
            return new ValueTask<TEntity>(Repository.Where(x => keyValues.Contains(x.Id)).FirstOrDefault());
        }

        public ValueTask<TEntity> FindAsync(object[] keyValues, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public IQueryable<TEntity> FromSql(string sql, params object[] parameters)
        {
            throw new NotImplementedException();
        }

        public IQueryable<TEntity> GetAll()
        {
            throw new NotImplementedException();
        }

        public TEntity GetFirstOrDefault(System.Linq.Expressions.Expression<Func<TEntity, bool>> predicate = null, Func<System.Linq.IQueryable<TEntity>, System.Linq.IOrderedQueryable<TEntity>> orderBy = null, Func<System.Linq.IQueryable<TEntity>, Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<TEntity, object>> include = null, bool disableTracking = true, bool ignoreQueryFilters = false)
        {
            throw new NotImplementedException();
        }

        public TResult GetFirstOrDefault<TResult>(System.Linq.Expressions.Expression<Func<TEntity, TResult>> selector, System.Linq.Expressions.Expression<Func<TEntity, bool>> predicate = null, Func<System.Linq.IQueryable<TEntity>, System.Linq.IOrderedQueryable<TEntity>> orderBy = null, Func<System.Linq.IQueryable<TEntity>, Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<TEntity, object>> include = null, bool disableTracking = true, bool ignoreQueryFilters = false)
        {
            throw new NotImplementedException();
        }

        public Task<TResult> GetFirstOrDefaultAsync<TResult>(System.Linq.Expressions.Expression<Func<TEntity, TResult>> selector, System.Linq.Expressions.Expression<Func<TEntity, bool>> predicate = null, Func<System.Linq.IQueryable<TEntity>, System.Linq.IOrderedQueryable<TEntity>> orderBy = null, Func<System.Linq.IQueryable<TEntity>, Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<TEntity, object>> include = null, bool disableTracking = true, bool ignoreQueryFilters = false)
        {
            throw new NotImplementedException();
        }

        public Task<TEntity> GetFirstOrDefaultAsync(System.Linq.Expressions.Expression<Func<TEntity, bool>> predicate = null, Func<System.Linq.IQueryable<TEntity>, System.Linq.IOrderedQueryable<TEntity>> orderBy = null, Func<System.Linq.IQueryable<TEntity>, Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<TEntity, object>> include = null, bool disableTracking = true, bool ignoreQueryFilters = false)
        {
            throw new NotImplementedException();
        }

        public Arch.EntityFrameworkCore.UnitOfWork.Collections.IPagedList<TEntity> GetPagedList(System.Linq.Expressions.Expression<Func<TEntity, bool>> predicate = null, Func<System.Linq.IQueryable<TEntity>, System.Linq.IOrderedQueryable<TEntity>> orderBy = null, Func<System.Linq.IQueryable<TEntity>, Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<TEntity, object>> include = null, int pageIndex = 0, int pageSize = 20, bool disableTracking = true, bool ignoreQueryFilters = false)
        {
            throw new NotImplementedException();
        }

        public Arch.EntityFrameworkCore.UnitOfWork.Collections.IPagedList<TResult> GetPagedList<TResult>(System.Linq.Expressions.Expression<Func<TEntity, TResult>> selector, System.Linq.Expressions.Expression<Func<TEntity, bool>> predicate = null, Func<System.Linq.IQueryable<TEntity>, System.Linq.IOrderedQueryable<TEntity>> orderBy = null, Func<System.Linq.IQueryable<TEntity>, Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<TEntity, object>> include = null, int pageIndex = 0, int pageSize = 20, bool disableTracking = true, bool ignoreQueryFilters = false) where TResult : class
        {
            throw new NotImplementedException();
        }

        public Task<Arch.EntityFrameworkCore.UnitOfWork.Collections.IPagedList<TEntity>> GetPagedListAsync(System.Linq.Expressions.Expression<Func<TEntity, bool>> predicate = null, Func<System.Linq.IQueryable<TEntity>, System.Linq.IOrderedQueryable<TEntity>> orderBy = null, Func<System.Linq.IQueryable<TEntity>, Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<TEntity, object>> include = null, int pageIndex = 0, int pageSize = 20, bool disableTracking = true, CancellationToken cancellationToken = default, bool ignoreQueryFilters = false)
        {
            throw new NotImplementedException();
        }

        public Task<Arch.EntityFrameworkCore.UnitOfWork.Collections.IPagedList<TResult>> GetPagedListAsync<TResult>(System.Linq.Expressions.Expression<Func<TEntity, TResult>> selector, System.Linq.Expressions.Expression<Func<TEntity, bool>> predicate = null, Func<System.Linq.IQueryable<TEntity>, System.Linq.IOrderedQueryable<TEntity>> orderBy = null, Func<System.Linq.IQueryable<TEntity>, Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<TEntity, object>> include = null, int pageIndex = 0, int pageSize = 20, bool disableTracking = true, CancellationToken cancellationToken = default, bool ignoreQueryFilters = false) where TResult : class
        {
            throw new NotImplementedException();
        }

        public void Insert(TEntity entity)
        {
            entity.Id = GetRepositoryHighestId() + 1;
            Repository.Add(entity);
        }

        public void Insert(params TEntity[] entities)
        {
            foreach(var entity in entities)
            {
                Insert(entity);
            }
        }

        public void Insert(IEnumerable<TEntity> entities)
        {
            throw new NotImplementedException();
        }

        public ValueTask<Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<TEntity>> InsertAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            entity.Id = GetRepositoryHighestId() + 1;
            Repository.Add(entity);
            return new ValueTask<Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<TEntity>>();
        }

        public Task InsertAsync(params TEntity[] entities)
        {
            foreach (var entity in entities)
            {
                Insert(entity);
            }
            return Task.CompletedTask;
        }

        public Task InsertAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            foreach (var entity in entities)
            {
                Insert(entity);
            }
            return Task.CompletedTask;
        }

        public void Update(TEntity entity)
        {
            if (entity.Id.Equals(0))
            {
                throw new NullReferenceException("Hodnota ID nesmí být při operaci Update 0!");
            }
            var testEntity = Repository.First(i => i.Id.Equals(entity.Id));
            if (testEntity == null)
            {
                return;
            }
            var index = Repository.IndexOf(testEntity);

            if (index != -1)
            {
                Repository[index] = entity;
            }
        }

        public void Update(params TEntity[] entities)
        {
            foreach (var entity in entities)
            {
                Update(entity);
            }
        }

        public void Update(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                Update(entity);
            }
        }

        private int GetRepositoryHighestId()
        {
            return Repository.Any() ? Repository.Max(m => m.Id) : 0;
        }
    }
}
