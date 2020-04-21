using Application.Dto.Comment;
using Application.Dto.Comment.Book;
using Application.Services.Interfaces;
using BookCrossingBackEnd.Controllers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationTest.Controllers
{
    [TestFixture]
    class BookRootCommentsControllerTest
    {
        private BookRootCommentsController _bookRootCommentsController;
        private Mock<IBookRootCommentService> _mockRootBookCommentService;
        private Mock<IUserResolverService> _mockUserResolverService;

        [OneTimeSetUp]
        public void Setup()
        {
            _mockRootBookCommentService = new Mock<IBookRootCommentService>();
            _mockUserResolverService = new Mock<IUserResolverService>();
            _bookRootCommentsController = new BookRootCommentsController(_mockRootBookCommentService.Object, _mockUserResolverService.Object);
        }

        private IEnumerable<RootDto> GetTestComments()
        {
            return new List<RootDto>()
            {
                new RootDto()
                {
                    Id="5e9c9ee859231a63bc853bf0",
                    Text="Text1",
                    Date=DateTime.UtcNow,
                    BookId=1,
                    Owner=new OwnerDto()
                    {
                        Id=1,
                        FirstName="A1",
                        MiddleName="B1",
                        LastName="C1",
                        Email="qwert@gmail.com",
                        Role="User"
                    },
                    Comments=new List<ChildDto>()
                    {
                        new ChildDto()
                        {
                            Id="5e9c9ee859231a63bc853bf1",
                            Text="Text2",
                            Date=DateTime.UtcNow,
                            Owner=new OwnerDto()
                            {
                                Id=1,
                                FirstName="A1",
                                MiddleName="B1",
                                LastName="C1",
                                Email="qwert1@gmail.com",
                                Role="User"
                            },
                            Comments = new List<ChildDto>()
                        },
                        new ChildDto()
                        {
                            Id="5e9c9ee859231a63bc853bf3",
                            Text="Text3",
                            Date=DateTime.UtcNow,
                            Owner=new OwnerDto()
                            {
                                Id=2,
                                FirstName="A2",
                                MiddleName="B2",
                                LastName="C2",
                                Email="qwert2@gmail.com",
                                Role="Admin"
                            },
                            Comments = new List<ChildDto>()
                        }

                    }
                },
                new RootDto()
                {
                    Id="5e9c9ee859231a63bc853bf4",
                    Text="Text4",
                    Date=DateTime.UtcNow,
                    BookId=2,
                    Owner=new OwnerDto()
                    {
                        Id=2,
                        FirstName="A2",
                        MiddleName="B2",
                        LastName="C2",
                        Email="qwert2@gmail.com",
                        Role="Admin"
                    },
                    Comments=new List<ChildDto>()
                }
            };
        }

        #region GetByBoolId

        [Test]
        public async Task GetByBookId_Returns_OkObjectResult()
        {
            int bookId = 1;
            IEnumerable<RootDto> rootDtos = GetTestComments().Where(dto => dto.BookId == bookId);
            _mockRootBookCommentService.Setup(s => s.GetByBookId(bookId)).ReturnsAsync(rootDtos);

            var getByBookIdResult = await _bookRootCommentsController.GetByBookId(bookId);

            var okResult = getByBookIdResult.Result as OkObjectResult;
            okResult.Should().BeOfType<OkObjectResult>();
            var books = okResult.Value as IEnumerable<RootDto>;
            books.Count().Should().Be(rootDtos.Count());
        }

        #endregion GetByBoolId


        #region GetAll

        [Test]
        public async Task GetAll_Returns_OkObjectResultWithRequestedCount()
        {
            _mockRootBookCommentService.Setup(s => s.GetAll()).ReturnsAsync(GetTestComments());

            var getAllResult = await _bookRootCommentsController.GetAll();

            var okResult = getAllResult.Result as OkObjectResult;
            okResult.Should().BeOfType<OkObjectResult>();
            var books = okResult.Value as IEnumerable<RootDto>;
            books.Count().Should().Be(GetTestComments().Count());
        }
        #endregion GetAll


        #region Delete

        [Test]
        public async Task Delete_ByUser_Returns_OkObjectResult()
        {
            RootDeleteDto deleteDto = new RootDeleteDto() { Id = "5e9c9ee859231a63bc853bf0", CommentOwnerId = 1 };
            _mockRootBookCommentService.Setup(s => s.Remove(deleteDto.Id)).ReturnsAsync(1);
            _mockUserResolverService.Setup(s => s.GetUserId()).Returns(1);
            _mockUserResolverService.Setup(s => s.IsUserAdmin()).Returns(false);

            var deleteResult = await _bookRootCommentsController.Delete(deleteDto);

            var okResult = deleteResult.Result as OkObjectResult;
            okResult.Should().BeOfType<OkObjectResult>();
            int deletedNumber = Convert.ToInt32(okResult.Value);
            deletedNumber.Should().Be(1);
        }

        [Test]
        public async Task Delete_ByUser_Returns_ForbidResult()
        {
            RootDeleteDto deleteDto = new RootDeleteDto() { Id = "5e9c9ee859231a63bc853bf0", CommentOwnerId = 1 };
            _mockRootBookCommentService.Setup(s => s.Remove(deleteDto.Id)).ReturnsAsync(1);
            _mockUserResolverService.Setup(s => s.GetUserId()).Returns(2);
            _mockUserResolverService.Setup(s => s.IsUserAdmin()).Returns(false);

            var deleteResult = await _bookRootCommentsController.Delete(deleteDto);

            var forbidResult = deleteResult.Result as ForbidResult;
            forbidResult.Should().BeOfType<ForbidResult>();
        }

        [Test]
        public async Task Delete_ByUser_Returns_NotFoundObjectResult()
        {
            RootDeleteDto deleteDto = new RootDeleteDto() { Id = "5e9c9ee859231a63bc853bf0", CommentOwnerId = 1 };
            _mockRootBookCommentService.Setup(s => s.Remove(deleteDto.Id)).ReturnsAsync(0);
            _mockUserResolverService.Setup(s => s.GetUserId()).Returns(1);
            _mockUserResolverService.Setup(s => s.IsUserAdmin()).Returns(false);

            var deleteResult = await _bookRootCommentsController.Delete(deleteDto);

            var notFoundObjectResult = deleteResult.Result as NotFoundObjectResult;
            notFoundObjectResult.Should().BeOfType<NotFoundObjectResult>();
            int deletedNumber = Convert.ToInt32(notFoundObjectResult.Value);
            deletedNumber.Should().Be(0);
        }

        [Test]
        public async Task Delete_ByAdmin_Returns_OkObjectResult()
        {
            RootDeleteDto deleteDto = new RootDeleteDto() { Id = "5e9c9ee859231a63bc853bf0", CommentOwnerId = 1 };
            _mockRootBookCommentService.Setup(s => s.Remove(deleteDto.Id)).ReturnsAsync(1);
            _mockUserResolverService.Setup(s => s.GetUserId()).Returns(2);
            _mockUserResolverService.Setup(s => s.IsUserAdmin()).Returns(true);

            var deleteResult = await _bookRootCommentsController.Delete(deleteDto);

            var okResult = deleteResult.Result as OkObjectResult;
            okResult.Should().BeOfType<OkObjectResult>();
            int deletedNumber = Convert.ToInt32(okResult.Value);
            deletedNumber.Should().Be(1);
        }

        [Test]
        public async Task Delete_ByAdmin_Returns_NotFoundObjectResult()
        {
            RootDeleteDto deleteDto = new RootDeleteDto() { Id = "5e9c9ee859231a63bc853bf0", CommentOwnerId = 1 };
            _mockRootBookCommentService.Setup(s => s.Remove(deleteDto.Id)).ReturnsAsync(0);
            _mockUserResolverService.Setup(s => s.GetUserId()).Returns(2);
            _mockUserResolverService.Setup(s => s.IsUserAdmin()).Returns(true);

            var deleteResult = await _bookRootCommentsController.Delete(deleteDto);

            var notFoundObjectResult = deleteResult.Result as NotFoundObjectResult;
            notFoundObjectResult.Should().BeOfType<NotFoundObjectResult>();
            int deletedNumber = Convert.ToInt32(notFoundObjectResult.Value);
            deletedNumber.Should().Be(0);
        }

        #endregion Delete


        #region Post      

        [Test]
        public async Task Post_Returns_OkObjectResult()
        {
            RootInsertDto insertDto = new RootInsertDto() { BookId = 1, CommentOwnerId = 1, Text = "Text" };
            _mockRootBookCommentService.Setup(s => s.Add(insertDto)).ReturnsAsync(1);
            _mockUserResolverService.Setup(s => s.GetUserId()).Returns(1);

            var postResult = await _bookRootCommentsController.Post(insertDto);

            var okResult = postResult.Result as OkObjectResult;
            okResult.Should().BeOfType<OkObjectResult>();
            int insertedNumber = Convert.ToInt32(okResult.Value);
            insertedNumber.Should().Be(1);
        }

        [Test]
        public async Task Post_Returns_BadRequestObjectResult()
        {
            RootInsertDto insertDto = new RootInsertDto() { BookId = 1, CommentOwnerId = 1, Text = "Text" };
            _mockRootBookCommentService.Setup(s => s.Add(insertDto)).ReturnsAsync(0);
            _mockUserResolverService.Setup(s => s.GetUserId()).Returns(1);

            var postResult = await _bookRootCommentsController.Post(insertDto);

            var badRequest = postResult.Result as BadRequestObjectResult;
            badRequest.Should().BeOfType<BadRequestObjectResult>();
            int insertedNumber = Convert.ToInt32(badRequest.Value);
            insertedNumber.Should().Be(0);
        }

        [Test]
        public async Task Post_Returns_ForbidResult()
        {
            RootInsertDto insertDto = new RootInsertDto() { BookId = 1, CommentOwnerId = 1, Text = "Text" };
            _mockRootBookCommentService.Setup(s => s.Add(insertDto)).ReturnsAsync(1);
            _mockUserResolverService.Setup(s => s.GetUserId()).Returns(2);

            var postResult = await _bookRootCommentsController.Post(insertDto);

            var forbidResult = postResult.Result as ForbidResult;
            forbidResult.Should().BeOfType<ForbidResult>();
        }

        #endregion Post


        #region Put

        [Test]
        public async Task Put_ByUser_Returns_OkObjectResult()
        {
            RootUpdateDto updateDto = new RootUpdateDto() { Id = "5e9c9ee859231a63bc853bf0", CommentOwnerId = 1, Text = "Text" };
            _mockRootBookCommentService.Setup(s => s.Update(updateDto)).ReturnsAsync(1);
            _mockUserResolverService.Setup(s => s.GetUserId()).Returns(1);

            var putResult = await _bookRootCommentsController.Put(updateDto);

            var okResult = putResult.Result as OkObjectResult;
            okResult.Should().BeOfType<OkObjectResult>();
            int updatedNumber = Convert.ToInt32(okResult.Value);
            updatedNumber.Should().Be(1);
        }

        [Test]
        public async Task Put_ByUser_Returns_ForbidResult()
        {
            RootUpdateDto updateDto = new RootUpdateDto() { Id = "5e9c9ee859231a63bc853bf0", CommentOwnerId = 1, Text = "Text" };
            _mockRootBookCommentService.Setup(s => s.Update(updateDto)).ReturnsAsync(1);
            _mockUserResolverService.Setup(s => s.GetUserId()).Returns(2);

            var putResult = await _bookRootCommentsController.Put(updateDto);

            var forbidResult = putResult.Result as ForbidResult;
            forbidResult.Should().BeOfType<ForbidResult>();
        }

        [Test]
        public async Task Put_ByUser_Returns_NotFoundObjectResult()
        {
            RootUpdateDto updateDto = new RootUpdateDto() { Id = "5e9c9ee859231a63bc853bf0", CommentOwnerId = 1, Text = "Text" };
            _mockRootBookCommentService.Setup(s => s.Update(updateDto)).ReturnsAsync(0);
            _mockUserResolverService.Setup(s => s.GetUserId()).Returns(1);

            var putResult = await _bookRootCommentsController.Put(updateDto);

            var notFoundObjectResult = putResult.Result as NotFoundObjectResult;
            notFoundObjectResult.Should().BeOfType<NotFoundObjectResult>();
            int updatedNumber = Convert.ToInt32(notFoundObjectResult.Value);
            updatedNumber.Should().Be(0);
        }

        #endregion Put
    }
}
