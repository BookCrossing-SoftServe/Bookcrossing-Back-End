using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Dto;
using Application.Services.Implementation;
using AutoMapper;
using Domain.RDBMS;
using Domain.RDBMS.Entities;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace ApplicationTest.Services
{
    [TestFixture]
    class AuthorServiceTest
    {
        private AuthorService _authorService;
        private Mock<IRepository<Author>> _authorRepositoryMock;
        private Mock<IMapper> _mapper;

        [SetUp]
        public void Setup()
        {
            _authorRepositoryMock = new Mock<IRepository<Author>>();
            _mapper = new Mock<IMapper>();
            _authorService = new AuthorService(_authorRepositoryMock.Object, _mapper.Object);
        }

        //GET by ID
        [Test]
        public async Task GetById_AuthorExists_Returns_AuthorDto()
        {
            _authorRepositoryMock.Setup(s => s.FindByIdAsync(It.IsAny<int>())).ReturnsAsync(new Author());
            _mapper.Setup(s => s.Map<AuthorDto>(It.IsAny<Author>())).Returns(new AuthorDto());

            var authorResult = await _authorService.GetById(It.IsAny<int>());

            authorResult.Should().BeOfType<AuthorDto>();
        }

        [Test]
        public async Task GetById_AuthorDoesNotExist_Returns_Null()
        {
            _authorRepositoryMock.Setup(s => s.FindByIdAsync(It.IsAny<int>())).ReturnsAsync(null as Author);
            _mapper.Setup(s => s.Map<AuthorDto>(It.IsAny<Author>())).Returns(null as AuthorDto);

            var authorResult = await _authorService.GetById(It.IsAny<int>());

            authorResult.Should().BeNull();
        }

        //POST
        [Test]
        public async Task Add_AuthorIsValid_Returns_AuthorDto()
        {
            var author = new Author();
            var authorDto = new AuthorDto();
            _authorRepositoryMock.Setup(s => s.Add(It.IsAny<Author>()));
            _mapper.Setup(s => s.Map<Author>(It.IsAny<InsertAuthorDto>())).Returns(author);
            _mapper.Setup(s => s.Map<AuthorDto>(It.IsAny<Author>())).Returns(authorDto);

            var authorResult = await _authorService.Add(new InsertAuthorDto());

            _authorRepositoryMock.Verify(x => x.Add(author), Times.Once);
            _authorRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
            authorResult.Should().BeOfType<AuthorDto>();
        }

        //DELETE
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


        //UPDATE
        [Test]
        public async Task Update_AuthorExists_Returns_true()
        {
            var author = new Author();
            _authorRepositoryMock.Setup(s => s.FindByIdAsync(It.IsAny<int>())).ReturnsAsync(author);
            _authorRepositoryMock.Setup(s => s.Update(author));
            _authorRepositoryMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);
            _mapper.Setup(s => s.Map<Author>(It.IsAny<AuthorDto>())).Returns(author);

            var result = await _authorService.Update(new AuthorDto());

            _authorRepositoryMock.Verify(x => x.Update(author), Times.Once);
            _authorRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
            result.Should().BeTrue();
        }
        [Test]
        public async Task Update_AuthorDoesNotExist_Returns_false()
        {
            _authorRepositoryMock.Setup(s => s.FindByIdAsync(It.IsAny<int>())).ReturnsAsync(null as Author);
            _mapper.Setup(s => s.Map<Author>(It.IsAny<AuthorDto>())).Returns(new Author());
            var result = await _authorService.Update(new AuthorDto());

            result.Should().BeFalse();
        }
    }

}
