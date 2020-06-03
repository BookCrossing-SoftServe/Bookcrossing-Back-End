using Application.Dto.Comment.Book;
using Application.Services.Interfaces;
using AutoMapper;
using Domain.NoSQL;
using Domain.NoSQL.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.RDBMS;
using Domain.RDBMS.Entities;

namespace Application.Services.Implementation
{
    public class BookRootCommentService : IBookRootCommentService
    {
        private readonly IRootRepository<BookRootComment> _rootCommentRepository;
        private readonly ICommentOwnerMapper _commentOwnerMapper;
        private readonly IRepository<Book> _bookRepository;
        public BookRootCommentService(IRootRepository<BookRootComment> rootCommentRepository, ICommentOwnerMapper commentOwnerMapper,
            IRepository<Book> bookRepository)
        {
            _rootCommentRepository = rootCommentRepository;
            _commentOwnerMapper = commentOwnerMapper;
            _bookRepository = bookRepository;
        }

        public async Task<int> Add(RootInsertDto insertDto)
        {
            var comment =  await _rootCommentRepository.InsertOneAsync(
                    new BookRootComment(true)
                    {
                        Text = insertDto.Text,
                        BookId = insertDto.BookId,
                        OwnerId = insertDto.OwnerId,
                        Date = DateTime.Now.ToUniversalTime().ToString(),
                        Rating = insertDto.Rating
                    });
            var book = _bookRepository.FindByIdAsync(insertDto.BookId).Result;
            book.Rating = await _rootCommentRepository.GetAvgRatingAsync(book.Id);
            await _bookRepository.Update(book, new List<string>() { "Rating" });
            await _bookRepository.SaveChangesAsync();
            return comment;
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
            var comment = await _rootCommentRepository.FindByIdAsync(id);
            var book = _bookRepository.FindByIdAsync(comment.BookId).Result;
            book.Rating = await _rootCommentRepository.GetAvgRatingAsync(book.Id);
            await _bookRepository.Update(book, new List<string>() { "Rating" });
            await _bookRepository.SaveChangesAsync();
            return Convert.ToInt32(deleteResult.DeletedCount);
        }

        public async Task<int> Update(RootUpdateDto updateDto)
        {
            var updateResult = await _rootCommentRepository.UpdateByIdAsync(updateDto.Id, new BookRootComment() { Text = updateDto.Text, Rating = updateDto.Rating});
            var comment = await _rootCommentRepository.FindByIdAsync(updateDto.Id);
            var book = _bookRepository.FindByIdAsync(comment.BookId).Result;
            book.Rating = await _rootCommentRepository.GetAvgRatingAsync(book.Id);
            await _bookRepository.Update(book, new List<string>() { "Rating" });
            await _bookRepository.SaveChangesAsync();
            return Convert.ToInt32(updateResult.ModifiedCount);
        }
    }
}
