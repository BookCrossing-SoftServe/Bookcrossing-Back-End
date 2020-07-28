using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using NUnit.Framework;
using System.Threading.Tasks;
using Application.Dto;
using Application.Services.Implementation;
using Application.Services.Interfaces;
using AutoMapper;
using Domain.RDBMS;
using Domain.RDBMS.Entities;
using FluentAssertions;
using Infrastructure.RDBMS;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Moq;

namespace ApplicationTest.Services
{
    [TestFixture]
    internal class AuthorServiceTest
    {
        private AuthorService _authorService;
        private BookCrossingContext _context;
        private Mock<IRepository<Author>> _authorRepositoryMock;
        private Mock<IRepository<BookAuthor>> _bookAuthorRepositoryMock;
        private Mock<IPaginationService> _paginationMock;
        private Mock<IMapper> _mapper;

        [OneTimeSetUp]
        public void ClassSetup()
        {
            var options = new DbContextOptionsBuilder<BookCrossingContext>()
                .UseInMemoryDatabase(databaseName: "Fake DB")
                .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;
            _context = new BookCrossingContext(options);
            _authorRepositoryMock = new Mock<IRepository<Author>>();
            _bookAuthorRepositoryMock = new Mock<IRepository<BookAuthor>>();
            _mapper = new Mock<IMapper>();
            _paginationMock = new Mock<IPaginationService>();
            _authorService = new AuthorService(
                _authorRepositoryMock.Object,
                _mapper.Object,
                _paginationMock.Object,
                _bookAuthorRepositoryMock.Object,
                _context);
        }

        [SetUp]
        public void TestSetup()
        {
            _authorRepositoryMock.Invocations.Clear();
        }

        #region GetById

        [Test]
        public async Task GetAuthorById_AuthorExists_Returns_AuthorDtoWithRequestedId()
        {
            var authorId = 1;
            var author = new Author { Id = authorId };
            var authorDto = new AuthorDto { Id = authorId };
            _authorRepositoryMock.Setup(s => s.FindByIdAsync(authorId))
                .ReturnsAsync(author);
            _mapper.Setup(s => s.Map<AuthorDto>(author))
                .Returns(authorDto);

            var authorResult = await _authorService.GetById(authorId);

            authorResult.Should().Be(authorDto);
        }

        [Test]
        public async Task GetAuthorById_AuthorDoesNotExist_ReturnsNull()
        {
            var authorId = 1;
            _authorRepositoryMock.Setup(s => s.FindByIdAsync(authorId))
                .ReturnsAsync(value: null);
            _mapper.Setup(s => s.Map<AuthorDto>(null))
                .Returns(value: null);

            var authorResult = await _authorService.GetById(authorId);

            authorResult.Should().BeNull();
        }

        #endregion GetById

        #region Post

        [Test]
        public async Task AddAuthor_AuthorIsValid_Returns_AuthorDto()
        {
            var author = new Author();
            var authorDto = new AuthorDto();
            _mapper.Setup(s => s.Map<Author>(authorDto))
                .Returns(author);
            _mapper.Setup(s => s.Map<AuthorDto>(author))
                .Returns(authorDto);

            var authorResult = await _authorService.Add(authorDto);

            _authorRepositoryMock.Verify(x => x.Add(author), Times.Once);
            _authorRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
            authorResult.Should().Be(authorDto);
        }

        #endregion Post

        #region Delete

        [Test]
        public async Task RemoveAuthor_AuthorExists_ReturnsTrue()
        {
            var authorId = 1;
            var author = new Author { Id = authorId };
            _authorRepositoryMock.Setup(s => s.FindByIdAsync(authorId))
                .ReturnsAsync(author);
            _authorRepositoryMock.Setup(x => x.SaveChangesAsync())
                .ReturnsAsync(1);

            var authorResult = await _authorService.Remove(authorId);

            _authorRepositoryMock.Verify(x => x.Remove(author), Times.Once);
            _authorRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);

            authorResult.Should().BeTrue();
        }

        [Test]
        public async Task RemoveAuthor_AuthorDoesNotExist_ReturnsFalse()
        {
            var authorId = 1;
            _authorRepositoryMock.Setup(s => s.FindByIdAsync(authorId))
                .ReturnsAsync(value: null);

            var authorResult = await _authorService.Remove(authorId);

            authorResult.Should().BeFalse();
        }

        #endregion Delete

        #region Update

        [TestCase(0, ExpectedResult = false, Description = "Returns false if there are no affected rows after saving changes to database")]
        [TestCase(1, ExpectedResult = true, Description = "Returns true if there are affected rows after saving changes to database")]
        public async Task<bool> UpdateAuthor(int affectedRows)
        {
            var author = new Author();
            var authorDto = new AuthorDto();
            _mapper.Setup(s => s.Map<Author>(authorDto))
                .Returns(author);
            _authorRepositoryMock.Setup(x => x.SaveChangesAsync())
                .ReturnsAsync(affectedRows);

            var result = await _authorService.Update(authorDto);

            _authorRepositoryMock.Verify(x => x.Update(author), Times.Once);
            _authorRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);

            return result;
        }

        #endregion Update

        
    }
}