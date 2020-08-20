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

            _rootRepository.Setup(m => m.GetById(It.IsAny<string>()))
                .ReturnsAsync(new RootDto()
                {
                    Comments = new List<ChildDto>()
                    {
                        new ChildDto() { Id = "5e9c9ee859231a63bc853bf1", Comments = new List<ChildDto>() },
                    }
                });


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

        [Test]
        public async Task SetAsDeleted_ReturnsNumberOfUpdatedObjects()
        {
            var bookChildCommentServiceMock = new BookChildCommentServiceMock(
                _childRepository.Object, 
                _rootRepository.Object, 
                _mapper.Object);

            UpdateResult updateResult = new UpdateResult.Acknowledged(1, 1, 1);

            _childRepository.Setup(m => m.SetAsync(
                    It.IsAny<string>(),
                    It.IsAny<BookChildComment>(),
                    It.IsAny<IEnumerable<(string, string)>>()))
                .ReturnsAsync(updateResult);

            var result = await bookChildCommentServiceMock.SetAsDeleted(
                new List<string> {"5e9c9ee859231a63bc853bf1", "5e9c9ee859231a63bc853bf2"}, 
                new ChildDto() {Comments = new List<ChildDto>()
                {
                    new ChildDto() {Id = "5e9c9ee859231a63bc85dt2w"}
                }},
                "5e9c9ee859231a63bc853bf0");

            result.Should().Be((int) updateResult.ModifiedCount);
        }

        [Test] public async Task Delete_RootCommentIsDeletedAndDoesntContainActiveSubcomments_ReturnsNumberOfUpdatedObjects()
        {
            var bookChildCommentServiceMock = new BookChildCommentServiceMock(
                _childRepository.Object,
                _rootRepository.Object,
                _mapper.Object);
            UpdateResult updateResult = new UpdateResult.Acknowledged(1, 1, 1);
            
            _childRepository.Setup(m => m.PullAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<IEnumerable<(string, string)>>(),
                    It.IsAny<string>()))
                .ReturnsAsync(updateResult);

            _rootRepository.Setup(m => m.GetById(It.IsAny<string>()))
                .ReturnsAsync(new RootDto() {IsDeleted = true, Comments = new List<ChildDto>()});

            var result = await bookChildCommentServiceMock.Delete(
                new List<string> { "5e9c9ee859231a63bc853bf1", "5e9c9ee859231a63bc853bf2", "5e9c9ee859231a63bc853bf3" },
                "5e9c9ee859231a63bc853bf0",
                "5e9c9ee859231a63bc853bf1");

            _rootRepository.Verify(m => m.Remove(It.IsAny<string>()), Times.Once());
            result.Should().Be((int)updateResult.ModifiedCount);
        }

        [Test]
        public void HasActiveComments_Yes_ReturnTrue()
        {
            var bookChildCommentServiceMock = new BookChildCommentServiceMock(
                _childRepository.Object,
                _rootRepository.Object,
                _mapper.Object);


            List<ChildDto> childDtos = new List<ChildDto>()
            {
                new ChildDto() {IsDeleted = true, Comments = new List<ChildDto>()},
                new ChildDto() {IsDeleted = true, Comments = new List<ChildDto>()
                {
                    new ChildDto() {Comments = new List<ChildDto>()}
                }}
            };

            var result = bookChildCommentServiceMock.HasActiveComments(childDtos);

            result.Should().Be(true);
        }


        [Test]
        public void HasActiveComments_No_ReturnFalse()
        {
            var bookChildCommentServiceMock = new BookChildCommentServiceMock(
                _childRepository.Object,
                _rootRepository.Object,
                _mapper.Object);


            List<ChildDto> childDtos = new List<ChildDto>()
            {
                new ChildDto() {IsDeleted = true, Comments = new List<ChildDto>()},
                new ChildDto() {IsDeleted = true, Comments = new List<ChildDto>()
                {
                    new ChildDto() {IsDeleted = true, Comments = new List<ChildDto>()}
                }}
            };

            var result = bookChildCommentServiceMock.HasActiveComments(childDtos);

            result.Should().Be(false);
        }

        [Test]
        [TestCase("1", true)]
        [TestCase("4", true)]
        [TestCase("5", false)]
        [TestCase("6", true)]
        [TestCase("8", false)]
        public async Task FindChild_WasFounded_ReturnChildDto(string searchChildId, bool found)
        {
            var bookChildCommentServiceMock = new BookChildCommentServiceMock(
                _childRepository.Object,
                _rootRepository.Object,
                _mapper.Object);

            var children = new List<ChildDto>
            {
                new ChildDto() {Id = "1", Comments = new List<ChildDto>()},
                new ChildDto() {Id = "2", Comments = new List<ChildDto>()
                {
                    new ChildDto() {Id = "4", Comments = new List<ChildDto>()}
                }},
                new ChildDto() {Id = "3", Comments = new List<ChildDto>()
                {
                    new ChildDto() {Id = "6", Comments = new List<ChildDto>()
                    {
                        new ChildDto() {Id = "7", Comments = new List<ChildDto>()}
                    }}
                }},
            };

            var result = await bookChildCommentServiceMock.FindChild(children, searchChildId);

            (result != null).Should().Be(found);
        }


        class BookChildCommentServiceMock : BookChildCommentService
        {
            public BookChildCommentServiceMock(IChildRepository<BookRootComment, BookChildComment> childCommentRepository,
                IBookRootCommentService bookRootCommentService,
                IMapper mapper) : base(childCommentRepository, bookRootCommentService, mapper)
            {
            }

            public new async Task<int> SetAsDeleted(IEnumerable<string> ids, ChildDto childComment, string rootId)
            {
                return await base.SetAsDeleted(ids, childComment, rootId);
            }

            public new async Task<int> Delete(IEnumerable<string> ids, string rootId, string childId)
            {
                return await base.Delete(ids, rootId, childId);
            }

            public new bool HasActiveComments(IEnumerable<ChildDto> children)
            {
                return base.HasActiveComments(children);
            }

            public new Task<ChildDto> FindChild(IEnumerable<ChildDto> children, string childId)
            {
                return base.FindChild(children, childId);
            }
        }
    }
}