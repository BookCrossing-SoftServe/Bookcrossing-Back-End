using Domain.NoSQL.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application
{
    public interface ICommentOwnerMapper
    {
        Task<IEnumerable<Dto.Comment.Book.RootDto>> MapAsync(IEnumerable<BookRootComment> rootEntities);
        Task<Dto.Comment.Book.RootDto> MapAsync(BookRootComment rootEntity);
    }
}
