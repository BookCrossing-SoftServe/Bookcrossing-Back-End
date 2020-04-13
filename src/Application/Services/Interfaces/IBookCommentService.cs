using Application.Dto.Comment;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services.Interfaces
{
    public interface IBookCommentService
    {
        /// <summary>
        /// Retrieve root comment by ID
        /// </summary>
        /// <param name="commentId">Comment's ID</param>
        /// <returns>returns book root comment DTO</returns>
        Task<BookRootCommentDto> GetById(string rootCommentId);

        /// <summary>
        /// Retrieve comment by book's ID
        /// </summary>
        /// <param name="bookId">Book's ID</param>
        /// <returns>returns Book DTO</returns>
        Task<IEnumerable<BookRootCommentDto>> GetByBookId(int bookId);

        /// <summary>
        /// Retrieve all comments
        /// </summary>
        /// <returns>returns list of comment DTOs</returns>
        Task<IEnumerable<BookRootCommentDto>> GetAll();

        /// <summary>
        /// Update specified comment
        /// </summary>
        /// <param name="updateDto">Book commet update DTO instance</param>
        /// <param name="ids">Book commet path of ids</param>
        /// <returns>Number of updated comments</returns>
        Task<int> Update(BookCommentUpdateDto updateDto, params string[] ids);

        /// <summary>
        /// Remove commnet from database
        /// </summary>
        /// <param name="ids">Book commet path of ids</param>
        /// <returns>Number of removed comments</returns>
        Task<int> Remove(params string[] ids);

        /// <summary>
        /// Create new comment and add it into Database
        /// </summary>
        /// <param name="insertDto">Commnet DTO instance</param>
        /// <param name="ids">Book commet path of id</param>
        /// <returns>Number of inserted commnets</returns>
        Task<int> Add(BookCommentInsertDto insertDto, params string[] ids);
    }
}
