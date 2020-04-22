using Application.Dto.Comment.Book;
using Application.Services.Implementation;
using Application.Services.Interfaces;
using Domain.NoSQL;
using Domain.NoSQL.Entities;
using FluentAssertions;
using MongoDB.Driver;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApplicationTest.Services.Comment.Book
{
    [TestFixture]
    class ChildServiceTest
    {
        private IBookChildCommentService _bookChildCommentService;
        private Mock<IChildRepository<BookRootComment,BookChildComment>> _childRepository;

        [SetUp]
        public void Setup()
        {
            _childRepository = new Mock<IChildRepository<BookRootComment, BookChildComment>>();
            _bookChildCommentService = new BookChildCommentService(_childRepository.Object);
        }

        #region Update

        [Test]
        public async Task Update_BookChildCommentExists_Returns_1()
        {
            ChildUpdateDto updateDto = new ChildUpdateDto()
            {
                Ids=new List<string>() { "5e9c9ee859231a63bc853bf0" , "5e9c9ee859231a63bc853bf1" },
            };
            List<(string, string)> path = new List<(string, string)>() { ("Comments", "5e9c9ee859231a63bc853bf1") };
            _childRepository
                .Setup(s => s.SetAsync(
                    "5e9c9ee859231a63bc853bf0", 
                    It.IsAny<BookChildComment>(),
                    path))
                .ReturnsAsync(new UpdateResult.Acknowledged(1, 1, "5e9c9ee859231a63bc853bf0"));

            var result = await _bookChildCommentService.Update(updateDto);

            result.Should().Be(1);         
        }

        [Test]
        public async Task Update_BookChildCommentNotExists_Returns_0()
        {
            ChildUpdateDto updateDto = new ChildUpdateDto()
            {
                Ids = new List<string>() { "5e9c9ee859231a63bc853bf0", "5e9c9ee859231a63bc853bf1" },
            };
            List<(string, string)> path = new List<(string, string)>() { ("Comments", "5e9c9ee859231a63bc853bf1") };
            _childRepository
                .Setup(s => s.SetAsync(
                    "5e9c9ee859231a63bc853bf0",
                    It.IsAny<BookChildComment>(),
                    path))
                .ReturnsAsync(new UpdateResult.Acknowledged(0, 0, null));

            var result = await _bookChildCommentService.Update(updateDto);

            result.Should().Be(0);
        }

        #endregion

        #region Remove

        [Test]
        public async Task Remove_BookChildCommentExists_Returns_1()
        {
            ChildDeleteDto deleteDto = new ChildDeleteDto()
            {
                Ids = new List<string>() { "5e9c9ee859231a63bc853bf0", "5e9c9ee859231a63bc853bf1" },
            };
            _childRepository
                .Setup(s => s.PullAsync(
                    "5e9c9ee859231a63bc853bf0",
                    "5e9c9ee859231a63bc853bf1",
                    new List<(string, string)>(),
                    "Comments"))
                .ReturnsAsync(new UpdateResult.Acknowledged(1, 1, "5e9c9ee859231a63bc853bf0"));

            var result = await _bookChildCommentService.Remove(deleteDto.Ids);

            result.Should().Be(1);
        }

        [Test]
        public async Task Remove_BookChildCommentNotExists_Returns_0()
        {
            ChildDeleteDto deleteDto = new ChildDeleteDto()
            {
                Ids = new List<string>() { "5e9c9ee859231a63bc853bf0", "5e9c9ee859231a63bc853bf1" },
            };
            _childRepository
                .Setup(s => s.PullAsync(
                    "5e9c9ee859231a63bc853bf0",
                    "5e9c9ee859231a63bc853bf1",
                    new List<(string,string)>(),
                    "Comments"))
                .ReturnsAsync(new UpdateResult.Acknowledged(0, 0, null));

            var result = await _bookChildCommentService.Remove(deleteDto.Ids);

            result.Should().Be(0);
        }

        #endregion

        #region Add

        [Test]
        public async Task Add_BookChildComment_Returns_1()
        {
            ChildInsertDto insertDto = new ChildInsertDto()
            {
                Ids = new List<string>() { "5e9c9ee859231a63bc853bf0"},
            };
            _childRepository
                .Setup(s => s.PushAsync(
                    "5e9c9ee859231a63bc853bf0",
                    It.IsAny<BookChildComment>(),
                    new List<(string, string)>(),
                    "Comments"))
                .ReturnsAsync(new UpdateResult.Acknowledged(1, 1, "5e9c9ee859231a63bc853bf0"));

            var result = await _bookChildCommentService.Add(insertDto);

            result.Should().Be(1);
        }

        [Test]
        public async Task Add_BookChildComment_Returns_0()
        {
            ChildInsertDto insertDto = new ChildInsertDto()
            {
                Ids = new List<string>() { "5e9c9ee859231a63bc853bf0" },
            };
            _childRepository
                .Setup(s => s.PushAsync(
                    "5e9c9ee859231a63bc853bf0",
                    It.IsAny<BookChildComment>(),
                    new List<(string, string)>(),
                    "Comments"))
                .ReturnsAsync(new UpdateResult.Acknowledged(0, 0, null));

            var result = await _bookChildCommentService.Add(insertDto);

            result.Should().Be(0);
        }

        #endregion
    }
}