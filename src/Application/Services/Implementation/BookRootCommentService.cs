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
        private readonly ICommentOwnerMapper _commentOwnerMapper;
        public BookRootCommentService(IRootRepository<BookRootComment> rootCommentRepository, ICommentOwnerMapper commentOwnerMapper)
        {
            _rootCommentRepository = rootCommentRepository;
            _commentOwnerMapper = commentOwnerMapper;
        }

        public async Task<int> Add(RootInsertDto insertDto)
        {
            return await _rootCommentRepository.InsertOneAsync(
                    new BookRootComment(true)
                    {
                        Text = insertDto.Text,
                        BookId = insertDto.BookId,
                        OwnerId = insertDto.OwnerId,
                        Date = DateTime.Now.ToUniversalTime().ToString()
                    });
        }

        public async Task<IEnumerable<RootDto>> GetAll()
        {
            return await _commentOwnerMapper.MapAsync(await _rootCommentRepository.GetAllAsync());
        }

        public async Task<IEnumerable<RootDto>> GetByBookId(int bookId)
        {
            return await _commentOwnerMapper.MapAsync(await _rootCommentRepository.FindManyAsync(root=>root.BookId==bookId));
        }

        public async Task<RootDto> GetById(string id)
        {
            return await _commentOwnerMapper.MapAsync(await _rootCommentRepository.FindByIdAsync(id));
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
