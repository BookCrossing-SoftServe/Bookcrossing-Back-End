using Application.Dto.Comment.Book;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services.Interfaces
{
    public interface IBookRootCommentService
    {
        /// <summary>
        /// Retrieve comment by ID
        /// </summary>
        /// <param name="id">Comment's ID</param>
        /// <returns>returns book root comment DTO</returns>
        Task<RootDto> GetById(string id);

        /// <summary>
        /// Retrieve comment by book's ID
        /// </summary>
        /// <param name="bookId">Book's ID</param>
        /// <returns>returns enumerable of comment DTOs</returns>
        Task<IEnumerable<RootDto>> GetByBookId(int bookId);

        /// <summary>
        /// Retrieve all comments
        /// </summary>
        /// <returns>returns enumerable of comment DTOs</returns>
        Task<IEnumerable<RootDto>> GetAll();

        /// <summary>
        /// Update specified comment
        /// </summary>
        /// <param name="updateDto">Book commet update DTO instance</param>
        /// <returns>Number of updated comments</returns>
        Task<int> Update(RootUpdateDto updateDto);

        /// <summary>
        /// Remove commnet from database
        /// </summary>
        /// <param name="id">Comment's ID</param>
        /// <returns>Number of removed comments</returns>
        Task<int> Remove(string id);

        /// <summary>
        /// Create new comment and add it into Database
        /// </summary>
        /// <param name="insertDto">Commnet DTO instance</param>
        /// <returns>Number of inserted commnets</returns>
        Task<int> Add(RootInsertDto insertDto);
    }
}
