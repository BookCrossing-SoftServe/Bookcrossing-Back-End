using Domain.NoSQL.Entities;

namespace Domain.NoSQL
{
    public interface IRepository<TEntity> where TEntity : IEntityBase
    {
    }
}
