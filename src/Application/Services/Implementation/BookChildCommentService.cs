using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Dto.Comment.Book;
using Application.Services.Interfaces;
using AutoMapper;
using Domain.NoSQL;
using Domain.NoSQL.Entities;

namespace Application.Services.Implementation
{
    public class BookChildCommentService : IBookChildCommentService
    {
        private readonly IChildRepository<BookRootComment, BookChildComment> _childCommentRepository;
        private readonly IBookRootCommentService _bookRootCommentService;
        private readonly IMapper _mapper;

        public BookChildCommentService(IChildRepository<BookRootComment, BookChildComment> childCommentRepository,
            IBookRootCommentService bookRootCommentService,
            IMapper mapper)
        {
            _childCommentRepository = childCommentRepository;
            _bookRootCommentService = bookRootCommentService;
            _mapper = mapper;
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
                    OwnerId = insertDto.OwnerId,
                    Date = DateTime.Now.ToUniversalTime().ToString()
                },
                path,
                "Comments");

            return Convert.ToInt32(updateResult.ModifiedCount);
        }

        public async Task<int> Remove(IEnumerable<string> ids)
        {
            string rootId = ids.First();
            string childId = ids.Last();
            var rootComment = await _bookRootCommentService.GetById(rootId);
            var childComment = await FindChild(rootComment.Comments, 
                childId);
            MongoDB.Driver.UpdateResult updateResult;
            if (childComment != null && childComment.Comments.Any())
            {
                var children = childComment.Comments.Select(c => _mapper.Map<ChildDto, BookChildComment>(c));

                List <(string nestedArrayName, string itemId)> path = ids.Skip(1).Select(x => ("Comments", x)).ToList();
                updateResult = await _childCommentRepository.SetAsync(
                    rootId,
                    new BookChildComment() {IsDeleted = true, Text = childComment.Text, Comments = children},
                    path
                );
            }
            else
            {
                List<(string nestedArrayName, string itemId)> path = ids.Skip(1).SkipLast(1).Select(x => ("Comments", x)).ToList();
                updateResult = await _childCommentRepository.PullAsync(
                    rootId,
                    childId,
                    path,
                    "Comments");
                rootComment = await _bookRootCommentService.GetById(rootId);
                if (!HasNotDeletedComments(rootComment.Comments))
                {
                    await _bookRootCommentService.Remove(rootId);
                }
                else
                {
                    await ClearCommentTree(rootComment, ids.Skip(1).SkipLast(1), 0);
                }
            }

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

        private async Task<ChildDto> FindChild(IEnumerable<ChildDto> children, string childId)
        {
            var searchedChild = children.FirstOrDefault(c => c.Id == childId);
            if (searchedChild != null)
            {
                return searchedChild;
            }

            foreach (var child in children)
            {
                if (child.Comments.Any())
                {
                    var result = await FindChild(child.Comments, childId);
                    if (result != null)
                    {
                        return result;
                    }
                }
            }

            return null;
        }

        public async Task ClearCommentTree(RootDto root, IEnumerable<string> ids, int skip)
        {
            List<(string nestedArrayName, string itemId)> path = ids.Skip(skip).Select(x => ("Comments", x)).ToList();
            var child = await FindChild(root.Comments, path[0].itemId);
            if (!HasNotDeletedComments(child.Comments))
            {
                var rootId = root.Id;
                var childId = child.Id;
                var x = ids.SkipLast(ids.Count() - skip).Select(x => ("Comments", x)).ToList();
                await _childCommentRepository.PullAsync(
                    rootId,
                    childId,
                    x,
                    "Comments");

                return;
            }

            await ClearCommentTree(root, ids, skip + 1);
        }

        private bool HasNotDeletedComments(IEnumerable<ChildDto> children)
        {
            if (!children.Any())
            {
                return false;
            }

            if (children.Any(c => c.IsDeleted == false))
            {
                return true;
            }

            foreach (var child in children)
            {
                if (HasNotDeletedComments(child.Comments))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
