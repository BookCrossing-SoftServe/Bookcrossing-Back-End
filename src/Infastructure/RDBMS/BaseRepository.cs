using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Domain.RDBMS;
using Domain.RDBMS.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Infrastructure.RDBMS
{
    public class BaseRepository<TEntity> : IDisposable, IRepository<TEntity> where TEntity : class, IEntityBase
    {
        protected readonly BookCrossingContext Context;
        protected DbSet<TEntity> Entities;

        public BaseRepository(BookCrossingContext context)
        {
            Context = context;
            Entities = context.Set<TEntity>();
        }

        public virtual IQueryable<TEntity> GetAll()
        {
            return Entities.AsQueryable();
        }

        public virtual async Task<TEntity> FindByIdAsync(params object[] keys)
        {
            return await Entities.FindAsync(keys);
        }

        public virtual async Task<TEntity> FindByCondition(Expression<Func<TEntity, bool>> predicate)
        {
            return await Entities.FirstOrDefaultAsync(predicate);
        }

        public virtual void Add(TEntity entity)
        {
            Entities.Add(entity);
        }

        public virtual void AddRange(IEnumerable<TEntity> entity)
        {
            Entities.AddRange(entity);
        }

        public virtual void Remove(TEntity entity)
        {
            Entities.Remove(entity);
        }

        public virtual void RemoveRange(IEnumerable<TEntity> entity)
        {
            Entities.RemoveRange(entity);
        }

        public virtual void Update(TEntity entity)
        {
            Entities.Update(entity);
        }

        public virtual async Task Update(TEntity entity, IEnumerable<string> fieldMasks)
        {
            var newEntry = Context.Entry(entity);
            var oldEntity = await Entities.FindAsync(newEntry.Property("Id").CurrentValue);
            var oldEntry = Context.Entry(oldEntity);

            var collectionFieldMasks = fieldMasks.Where(name => newEntry.Collections.Any(a => a.Metadata.Name == name));
            var propertyFieldMasks = fieldMasks.Where(name => newEntry.Properties.Any(a => a.Metadata.Name == name));

            foreach (var name in collectionFieldMasks)
            {
                var oldCollection = oldEntry.Collection(name);
                await oldCollection.LoadAsync();
                foreach (var item in oldCollection.CurrentValue)
                {
                    var oldItemEntry = Context.Entry(item);
                    oldItemEntry.State = EntityState.Deleted;
                }
                var newCollection = newEntry.Collection(name);
                foreach (var item in newCollection.CurrentValue)
                {
                    var newItemEntry = Context.Entry(item);
                    newItemEntry.State = EntityState.Added;
                }
            }

            foreach (var name in propertyFieldMasks)
            {
                oldEntry.Property(name).CurrentValue = newEntry.Property(name).CurrentValue;
                oldEntry.Property(name).IsModified = true;
            }
        }

        public async Task<int> SaveChangesAsync()
        {
            lock (Context)
            {
                return Context.SaveChangesAsync().Result;
            }
        }

        #region IDisposable Support
        private bool _disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    Entities = null;
                }
                Context?.Dispose();
                _disposedValue = true;
            }
        }
        ~BaseRepository()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
