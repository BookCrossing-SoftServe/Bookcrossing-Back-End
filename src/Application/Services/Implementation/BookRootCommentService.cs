using Application.Dto.Comment.Book;
using Application.Services.Interfaces;
using AutoMapper;
using Domain.NoSQL;
using Domain.NoSQL.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services.Implementation
{
    public class BookRootCommentService : IBookRootCommentService
    {
        private readonly IRootRepository<BookRootComment> _rootCommentRepository;
        private readonly IMapper _mapper;
        public BookRootCommentService(IRootRepository<BookRootComment> rootCommentRepository, IMapper mapper)
        {
            _rootCommentRepository = rootCommentRepository;
            _mapper = mapper;
        }

        public async Task<int> Add(RootInsertDto insertDto)
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

        public async Task<IEnumerable<RootDto>> GetAll()
        {
            return _mapper.Map<IEnumerable<RootDto>>(await _rootCommentRepository.GetAllAsync());
        }

        public async Task<IEnumerable<RootDto>> GetByBookId(int bookId)
        {
            return _mapper.Map<IEnumerable<RootDto>>(await _rootCommentRepository.FindManyAsync(root => root.BookId == bookId));
        }

        public async Task<RootDto> GetById(string id)
        {
            return _mapper.Map<RootDto>(await _rootCommentRepository.FindByIdAsync(id));
        }

        public async Task<int> Remove(string id)
        {
            var deleteResult = await _rootCommentRepository.DeleteByIdAsync(id);
            return Convert.ToInt32(deleteResult.DeletedCount);
        }

        public async Task<int> Update(RootUpdateDto updateDto)
        {
            var updateResult = await _rootCommentRepository.UpdateByIdAsync(updateDto.Id, new BookRootComment() { Text = updateDto.Text });
            return Convert.ToInt32(updateResult.ModifiedCount);
        }
    }
}
