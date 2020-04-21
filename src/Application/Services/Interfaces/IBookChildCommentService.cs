using Application.Dto.Comment.Book;
using System.Threading.Tasks;

namespace Application.Services.Interfaces
{
    public interface IBookChildCommentService
    {      
        /// <summary>
        /// Update specified comment
        /// </summary>
        /// <param name="updateDto">Book commet update DTO instance</param>
        /// <param name="updateDto.Ids">
        /// If ids length greater than 1, child comment will be updated. 
        /// </param>
        /// <returns>Number of updated comments</returns>
        Task<int> Update(ChildUpdateDto updateDto);

        /// <summary>
        /// Remove commnet from database
        /// </summary>
        /// <param name="insertDto">Commnet DTO instance</param>
        /// <param name="deleteDto.Ids">
        /// If ids length greater than 1, child comment will be deleted. 
        /// </param>
        /// <returns>Number of removed comments</returns>
        Task<int> Remove(ChildDeleteDto deleteDto);

        /// <summary>
        /// Create new comment and add it into Database
        /// </summary>
        /// <param name="insertDto">Commnet DTO instance</param>
        /// <param name="insertDto.Ids">
        /// If ids length greater than 0, dto will  insert in childe array of comments. 
        /// </param>
        /// <returns>Number of inserted commnets</returns>
        Task<int> Add(ChildInsertDto insertDto);
    }
}
