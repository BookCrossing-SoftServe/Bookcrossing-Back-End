using Domain.NoSQL;
using Domain.NoSQL.Entities;

namespace Infrastructure.NoSQL.Abstract
{
    class ABaseRepository<TEntity> : IRepository<TEntity> where TEntity : class, IEntityBase
    {
    }
}
