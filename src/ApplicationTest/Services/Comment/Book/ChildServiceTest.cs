using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Dto.Comment.Book;
using Application.Services.Implementation;
using Application.Services.Interfaces;
using AutoMapper;
using Domain.NoSQL;
using Domain.NoSQL.Entities;
using FluentAssertions;
using MongoDB.Driver;
using Moq;
using NUnit.Framework;

namespace ApplicationTest.Services.Comment.Book
{
    [TestFixture]
    class ChildServiceTest
    {
        private IBookChildCommentService _bookChildCommentService;
        private Mock<IChildRepository<BookRootComment,BookChildComment>> _childRepository;
        private Mock<IBookRootCommentService> _rootRepository;
        private Mock<IMapper> _mapper;

        [SetUp]
        public void Setup()
        {
            _childRepository = new Mock<IChildRepository<BookRootComment, BookChildComment>>();
            _rootRepository = new Mock<IBookRootCommentService>();
            _mapper = new Mock<IMapper>();
            _bookChildCommentService = new BookChildCommentService(_childRepository.Object, _rootRepository.Object, _mapper.Object);
        }

        #region Update

        [Test]
        [TestCase(1, "5e9c9ee859231a63bc853bf0", 1)]
        [TestCase(0, null, 0)]
        public async Task UpdateComment_BookChildCommentExistsAndNot_ReturnsNumberOfUpdatedComments(int updateValue, string commentId, int expectedResult)
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
                .ReturnsAsync(new UpdateResult.Acknowledged(updateValue, updateValue, commentId));

            var result = await _bookChildCommentService.Update(updateDto);

            result.Should().Be(expectedResult);         
        }

        #endregion

        #region Remove

        [Test]
        [TestCase(1, "5e9c9ee859231a63bc853bf0", 1)]
        [TestCase(0, null, 0)]
        public async Task RemoveComment_BookChildCommentExistsAndNot_ReturnsNumberOfRemovedComments(int updateValue, string commentId, int expectedResult)
        {
            ChildDeleteDto deleteDto = new ChildDeleteDto()
            {
                Ids = new List<string>() { "5e9c9ee859231a63bc853bf0", "5e9c9ee859231a63bc853bf1" },
            };

            _rootRepository.Setup(m => m.GetById(It.IsAny<string>()))
                .ReturnsAsync(new RootDto() {Comments = new List<ChildDto>()});

            _childRepository
                .Setup(s => s.PullAsync(
                    "5e9c9ee859231a63bc853bf0",
                    "5e9c9ee859231a63bc853bf1",
                    new List<(string, string)>(),
                    "Comments"))
                .ReturnsAsync(new UpdateResult.Acknowledged(updateValue, updateValue, commentId));

            var result = await _bookChildCommentService.Remove(deleteDto.Ids);

            result.Should().Be(expectedResult);
        }

        #endregion

        #region Add

        [Test]
        [TestCase(1, "5e9c9ee859231a63bc853bf0", 1)]
        [TestCase(0, null, 0)]
        public async Task AddComment_BookChildComment_ReturnsNumberOfAddedComments(int updateValue,string commentId, int expectedResult)
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
                .ReturnsAsync(new UpdateResult.Acknowledged(updateValue, updateValue, commentId));

            var result = await _bookChildCommentService.Add(insertDto);

            result.Should().Be(expectedResult);
        }

        #endregion
    }
}