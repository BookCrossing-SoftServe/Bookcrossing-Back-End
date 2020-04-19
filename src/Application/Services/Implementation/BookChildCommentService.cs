using Application.Dto.Comment.Book;
using Application.Services.Interfaces;
using Domain.NoSQL;
using Domain.NoSQL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Services.Implementation
{
    public class BookChildCommentService : IBookChildCommentService
    {
        private readonly IChildRepository<BookRootComment, BookChildComment> _childCommentRepository;
        public BookChildCommentService(IChildRepository<BookRootComment, BookChildComment> childCommentRepository)
        {
            _childCommentRepository = childCommentRepository;
        }

        public async Task<int> Add(ChildInsertDto insertDto)
        {
            string rootId = insertDto.Ids.First();
            List<(string nestedArrayName, string itemId)> path = insertDto.Ids.Skip(1).Select(x => ("Comments", x)).ToList();

            var updateResult = await _childCommentRepository.PushAsync(
                rootId,
                new BookChildComment(true)
                {
                    Text = insertDto.Text,
                    OwnerId = insertDto.CommentOwnerId,
                    Date = DateTime.Now.ToUniversalTime().ToString()
                },
                path,
                "Comments");

            return Convert.ToInt32(updateResult.ModifiedCount);
        }

        public async Task<int> Remove(ChildDeleteDto deleteDto)
        {
            string rootId = deleteDto.Ids.First();
            List<(string nestedArrayName, string itemId)> path = deleteDto.Ids.Skip(1).SkipLast(1).Select(x => ("Comments", x)).ToList();
            string childId = deleteDto.Ids.Last();

            var updateResult = await _childCommentRepository.PullAsync(
                rootId,
                childId,
                path,
                "Comments");

            return Convert.ToInt32(updateResult.ModifiedCount);
        }

        public async Task<int> Update(ChildUpdateDto updateDto)
        {
            string rootId = updateDto.Ids.First();
            List<(string nestedArrayName, string itemId)> path = updateDto.Ids.Skip(1).Select(x => ("Comments", x)).ToList();

            var updateResult = await _childCommentRepository.SetAsync(
                rootId,
                new BookChildComment() { Text = updateDto.Text },
                path);

            return Convert.ToInt32(updateResult.ModifiedCount);
        }
    }
}
