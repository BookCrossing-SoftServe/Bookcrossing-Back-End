using Application.Dto.Comment;
using Application.Services.Interfaces;
using AutoMapper;
using Domain.NoSQL;
using Domain.NoSQL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Services.Implementation
{
    public class BookCommentService : IBookCommentService
    {
        private readonly IRootRepository<BookRootComment> _rootCommentRepository;
        private readonly IChildRepository<BookRootComment, BookChildComment> _childCommentRepository;
        private readonly IMapper _mapper;
        public BookCommentService(IRootRepository<BookRootComment> rootCommentRepository, IChildRepository<BookRootComment, BookChildComment> childCommentRepository, IMapper mapper)
        {
            _rootCommentRepository = rootCommentRepository;
            _childCommentRepository = childCommentRepository;
            _mapper = mapper;
        }

        public async Task<int> Add(BookCommentInsertDto insertDto, params string[] ids)
        {
            if (ids == null || ids.Length == 0)
            {
                return await _rootCommentRepository.InsertOneAsync(
                    new BookRootComment(true)
                    {
                        Text = insertDto.Text,
                        BookId = insertDto.BookId,
                        CommentOwnerId = insertDto.CommentOwnerId,
                        Date = DateTime.Now.ToUniversalTime().ToString()
                    });
            }
            else
            {
                string rootId = "";
                List<(string nestedArrayName, string itemId)> path = new List<(string nestedArrayName, string itemId)>();

                foreach (string id in ids)
                {
                    if (rootId == "")
                    {
                        rootId = id;
                    }
                    else
                    {
                        path.Add(("Comments", id));
                    }
                }

                var updateResult = await _childCommentRepository.PushAsync(
                    rootId,
                    new BookChildComment(true)
                    {
                        Text = insertDto.Text,
                        CommentOwnerId = insertDto.CommentOwnerId,
                        Date = DateTime.Now.ToUniversalTime().ToString()
                    },
                    path,
                    "Comments");
                return Convert.ToInt32(updateResult.ModifiedCount);
            }
        }

        public async Task<IEnumerable<BookRootCommentDto>> GetAll()
        {
            return _mapper.Map<IEnumerable<BookRootCommentDto>>(await _rootCommentRepository.GetAllAsync());
        }

        public async Task<IEnumerable<BookRootCommentDto>> GetByBookId(int bookId)
        {
            return _mapper.Map<IEnumerable<BookRootCommentDto>>(await _rootCommentRepository.FindManyAsync(root => root.BookId == bookId));
        }

        public async Task<BookRootCommentDto> GetById(string rootCommentId)
        {
            return _mapper.Map<BookRootCommentDto>(await _rootCommentRepository.FindByIdAsync(rootCommentId));
        }

        public async Task<int> Remove(params string[] ids)
        {
            if (ids.Length == 0)
            {
                return 0;
            }
            else if (ids.Length == 1)
            {
                var deleteResult = await _rootCommentRepository.DeleteByIdAsync(ids.First());
                return Convert.ToInt32(deleteResult.DeletedCount);
            }
            else
            {
                string rootId = "";
                string childId = "";
                List<(string nestedArrayName, string itemId)> path = new List<(string nestedArrayName, string itemId)>();

                foreach (string id in ids)
                {
                    if (rootId == "")
                    {
                        rootId = id;
                    }
                    else if (id == ids.Last())
                    {
                        childId = id;
                    }
                    else
                    {
                        path.Add(("Comments", id));
                    }
                }

                var updateResult = await _childCommentRepository.PullAsync(
                    rootId,
                    childId,
                    path,
                    "Comments");
                return Convert.ToInt32(updateResult.ModifiedCount);
            }
        }

        public async Task<int> Update(BookCommentUpdateDto updateDto, params string[] ids)
        {
            if (ids.Length == 0)
            {
                return 0;
            }
            if (ids.Length == 1)
            {
                var updateResult = await _rootCommentRepository.UpdateByIdAsync(ids.First(), new BookRootComment() { Text = updateDto.Text });
                return Convert.ToInt32(updateResult.ModifiedCount);
            }
            else
            {
                string rootId = "";
                List<(string nestedArrayName, string itemId)> path = new List<(string nestedArrayName, string itemId)>();

                foreach (string id in ids)
                {
                    if (rootId == "")
                    {
                        rootId = id;
                    }
                    else
                    {
                        path.Add(("Comments", id));
                    }
                }

                var updateResult = await _childCommentRepository.SetAsync(
                    rootId,
                    new BookChildComment() { Text = updateDto.Text },
                    path);
                return Convert.ToInt32(updateResult.ModifiedCount);
            }
        }
    }
}
