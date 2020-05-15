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
            var entry = Context.Entry(entity);
            var collectionList = new List<CollectionEntry>();
            var oldEntity = await Entities.FindAsync(entry.Property("Id").CurrentValue);
            var oldEntry = Context.Entry(oldEntity);

            var collectionFieldMasks = fieldMasks.Where(name => entry.Collections.Any(a => a.Metadata.Name == name));
            var propertyFieldMasks = fieldMasks.Where(name => entry.Properties.Any(a => a.Metadata.Name == name));

            foreach (var name in collectionFieldMasks)
            {
                var oldCollection = Context.Entry(oldEntity).Collection(name);
                await oldCollection.LoadAsync();
                collectionList.Add(oldCollection);
            }

            if (oldEntry != null)
            {
                oldEntry.State = EntityState.Detached;
            }

            foreach (var collection in collectionList)
            {
                foreach (var item in collection.CurrentValue)
                {
                    var newEntry = Context.Entry(item);
                    newEntry.State = EntityState.Deleted;
                }
            }

            foreach (var name in collectionFieldMasks)
            {
                var newCollection = entry.Collections.Single(a => a.Metadata.Name == name);
                foreach (var item in newCollection.CurrentValue)
                {
                    var newEntry = Context.Entry(item);
                    newEntry.State = EntityState.Added;
                }
            }

            foreach (var name in propertyFieldMasks)
            {
                entry.Property(name).IsModified = true;
            }
        }

        public async Task<int> SaveChangesAsync()
        {
            return await Context.SaveChangesAsync();
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
