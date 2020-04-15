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
        /// <param name="updateDto.Ids">
        /// If ids equals null or ids length equals 0, method will return 0 (0 elements was updated). 
        /// If ids length equals 1, root comment will be updated. 
        /// If ids length greater than 1, child comment will be updated. 
        /// </param>
        /// <returns>Number of updated comments</returns>
        Task<int> Update(BookCommentUpdateDto updateDto);

        /// <summary>
        /// Remove commnet from database
        /// </summary>
        /// <param name="insertDto">Commnet DTO instance</param>
        /// <param name="deleteDto.Ids">
        /// If ids equals null or ids length equals 0, method will return 0 (0 elements was deleted). 
        /// If ids length equals 1, root comment will be deleted. 
        /// If ids length greater than 1, child comment will be deleted. 
        /// </param>
        /// <returns>Number of removed comments</returns>
        Task<int> Remove(BookCommentDeleteDto deleteDto);

        /// <summary>
        /// Create new comment and add it into Database
        /// </summary>
        /// <param name="insertDto">Commnet DTO instance</param>
        /// <param name="insertDto.Ids">
        /// If ids equals null or ids length equals 0, dto will insert like root comment. 
        /// If ids length greater than 1, dto will  insert in childe array of comments. 
        /// </param>
        /// <returns>Number of inserted commnets</returns>
        Task<int> Add(BookCommentInsertDto insertDto);
    }
}
