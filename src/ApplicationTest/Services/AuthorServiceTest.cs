using NUnit.Framework;
using System.Threading.Tasks;
using Application.Dto;
using Application.Services.Implementation;
using Application.Services.Interfaces;
using AutoMapper;
using Domain.RDBMS;
using Domain.RDBMS.Entities;
using FluentAssertions;
using Moq;

namespace ApplicationTest.Services
{
    [TestFixture]
    internal class AuthorServiceTest
    {
        private AuthorService _authorService;
        private Mock<IRepository<Author>> _authorRepositoryMock;
        private Mock<IPaginationService> _paginationMock;
        private Mock<IMapper> _mapper;

        [SetUp]
        public void Setup()
        {
            _authorRepositoryMock = new Mock<IRepository<Author>>();
            _mapper = new Mock<IMapper>();
            _paginationMock = new Mock<IPaginationService>();
            _authorService = new AuthorService(_authorRepositoryMock.Object, _mapper.Object, _paginationMock.Object);
        }

        #region GetById

        [Test]
        [TestCase(201)]
        public async Task GetById_AuthorExists_Returns_AuthorDtoWithRequestedId(int id)
        {
            var expectedAuthor = new Author() {Id = 201};
            _authorRepositoryMock.Setup(s => s.FindByIdAsync(It.IsAny<int>())).ReturnsAsync(expectedAuthor);
            _mapper.Setup(s => s.Map<AuthorDto>(It.IsAny<Author>())).Returns(new AuthorDto() {Id = expectedAuthor.Id});

            var authorResult = await _authorService.GetById(id);

            authorResult.Should().BeOfType<AuthorDto>();
            authorResult.Id.Should().Be(id);
        }

        [Test]
        public async Task GetById_AuthorDoesNotExist_Returns_Null()
        {
            _authorRepositoryMock.Setup(s => s.FindByIdAsync(It.IsAny<int>())).ReturnsAsync(null as Author);
            _mapper.Setup(s => s.Map<AuthorDto>(It.IsAny<Author>())).Returns(null as AuthorDto);

            var authorResult = await _authorService.GetById(It.IsAny<int>());

            authorResult.Should().BeNull();
        }

        #endregion GetById

        #region Post

        [Test]
        public async Task Add_AuthorIsValid_Returns_AuthorDto()
        {
            var author = new Author();
            var authorDto = new AuthorDto();
            _authorRepositoryMock.Setup(s => s.Add(It.IsAny<Author>()));
            _mapper.Setup(s => s.Map<Author>(It.IsAny<AuthorDto>())).Returns(author);
            _mapper.Setup(s => s.Map<AuthorDto>(It.IsAny<Author>())).Returns(authorDto);

            var authorResult = await _authorService.Add(new AuthorDto());

            _authorRepositoryMock.Verify(x => x.Add(author), Times.Once);
            _authorRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
            authorResult.Should().BeOfType<AuthorDto>();
        }

        #endregion Post

        #region Delete

        [Test]
        public async Task Remove_AuthorExists_Returns_True()
        {
            var author = new Author();
            _authorRepositoryMock.Setup(s => s.FindByIdAsync(It.IsAny<int>())).ReturnsAsync(author);
            _authorRepositoryMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);

            var authorResult = await _authorService.Remove(It.IsAny<int>());

            _authorRepositoryMock.Verify(x => x.Remove(author), Times.Once);
            _authorRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
            authorResult.Should().BeTrue();
        }

        [Test]
        public async Task Remove_AuthorDoesNotExist_Returns_False()
        {
            _authorRepositoryMock.Setup(s => s.FindByIdAsync(It.IsAny<int>())).ReturnsAsync(null as Author);

            var authorResult = await _authorService.Remove(It.IsAny<int>());

            authorResult.Should().BeFalse();
        }

        #endregion Delete

        #region Update

        [Test]
        public async Task Update_AuthorExists_Returns_true()
        {
            var author = new Author();
            _authorRepositoryMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);
            _mapper.Setup(s => s.Map<Author>(It.IsAny<AuthorDto>())).Returns(author);
            _authorRepositoryMock.Setup(s => s.Update(author));

            var result = await _authorService.Update(new AuthorDto());

            _authorRepositoryMock.Verify(x => x.Update(author), Times.Once);
            _authorRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
            result.Should().BeTrue();
        }

        [Test]
        public async Task Update_AuthorDoesNotExist_Returns_false()
        {
            _mapper.Setup(s => s.Map<Author>(It.IsAny<AuthorDto>())).Returns(new Author());

            _authorRepositoryMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(0);

            var result = await _authorService.Update(new AuthorDto());

            result.Should().BeFalse();
        }

        #endregion Update
    }
}