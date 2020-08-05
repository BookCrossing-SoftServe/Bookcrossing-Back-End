using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using Application.Dto;
using Application.Dto.OuterSource;
using Application.Dto.QueryParams;
using Application.Services.Implementation;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using NUnit.Framework;
using Org.BouncyCastle.Security;

namespace ApplicationTest.Services
{
    [TestFixture]
    internal class GoodreadsServiceTests
    {
        private Mock<HttpMessageHandler> _httpMessageHandlerMock;
        private GoodreadsSettings _goodreadsSettings;
        private HttpClient _httpClient;
        private GoodreadsServiceProxy _service;
        private string _baseUrl;
        private string _validApiKey;
        private string _invalidApiKey;

        [OneTimeSetUp]
        public void InitializeClass()
        {
            _httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            _goodreadsSettings = new GoodreadsSettings();
            _baseUrl = "https://www.goodreads.com";
            _validApiKey = "Valid Api";
            _invalidApiKey = "Invalid Api";
            _httpClient = new HttpClient(_httpMessageHandlerMock.Object)
            {
                BaseAddress = new Uri(_baseUrl)
            };
            _goodreadsSettings = new GoodreadsSettings { ApiKey = _validApiKey };
            _service = new GoodreadsServiceProxy(
                _httpClient,
                new OptionsWrapper<GoodreadsSettings>(_goodreadsSettings));
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void Constructor_ApiKeyIsNullOrWhiteSpaces_ThrowsArgumentException(string apiKey)
        {
            _goodreadsSettings.ApiKey = apiKey;

            Assert.Throws<Exception>(
                () =>
            {
                var goodreadsService = new GoodreadsService(
                    Mock.Of<HttpClient>(),
                    new OptionsWrapper<GoodreadsSettings>(_goodreadsSettings));
            });
        }

        [Test]
        public void Constructor_ApiKeyInGoodreadsSettingsIsNotNullOrWhiteSpaces_ServiceConstructed()
        {
            _goodreadsSettings.ApiKey = _validApiKey;

            Assert.DoesNotThrow(() =>
            {
                var goodreadsService = new GoodreadsService(
                    Mock.Of<HttpClient>(),
                    new OptionsWrapper<GoodreadsSettings>(_goodreadsSettings));
            });
        }

        [Test]
        public void SendGetRequest_ApiKeyIsInvalid_ThrowsInvalidKeyException()
        {
            var requestUri = new Uri($"{_baseUrl}/url/with?key={_invalidApiKey}");
            _httpMessageHandlerMock.Protected()
                    .Setup<Task<HttpResponseMessage>>(
                        "SendAsync",
                        ItExpr.Is<HttpRequestMessage>(obj => obj.RequestUri == requestUri && obj.Method == HttpMethod.Get),
                        ItExpr.IsAny<CancellationToken>())
                    .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.Unauthorized))
                    .Verifiable();
            UseInvalidApiKey();

            _service.Invoking(s => s.SendGetRequestProxy(requestUri.ToString()))
                .Should()
                .Throw<InvalidKeyException>();
        }

        [Test]
        public void GetBookFromXml_NullNodePassedAsParameter_ThrowsArgumentNullException()
        {
            _service.Invoking(obj => obj.GetBookFromXmlProxy(null))
                .Should()
                .Throw<ArgumentNullException>();
        }

        [Test]
        public void GetBookFromXml_ParameterIsNotNull_ReturnsOuterBookDto()
        {
            var xmlDocument = new XmlDocument();

            var result = _service.GetBookFromXmlProxy(xmlDocument);

            result.Should().BeOfType<OuterBookDto>();
        }

        [Test]
        public async Task SearchBooks_SendGetRequestDoesNotThrowException_ReturnPaginationDtoObject()
        {
            var requestUri = new Uri(string.Format(
                "{0}/search/index.xml?key={1}&page={2}&q={3}&per_page={2}",
                _baseUrl,
                _validApiKey,
                It.IsAny<int>(),
                It.IsAny<string>()));
            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(obj => obj.RequestUri == requestUri && obj.Method == HttpMethod.Get),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent("<results><best_book type='Book'></best_book></results>")
                })
                .Verifiable();
            UseValidApiKey();

            var result = await _service.SearchBooks(Mock.Of<OuterSourceQueryParameters>());

            _httpMessageHandlerMock.Verify();

            result.Should().BeOfType<PaginationDto<OuterBookDto>>();
            result.TotalCount.Should()
                .BeGreaterOrEqualTo(0);
            result.Page.Should()
                .NotBeNull().And
                .AllBeOfType<OuterBookDto>().And
                .NotContainNulls();
        }

        [Test]
        public async Task GetBook_BookWithEnteredIdNotExists_ReturnNull()
        {
            var requestUri = new Uri(string.Format(
                "{0}/book/show?key={1}&id={2}",
                _baseUrl,
                _validApiKey,
                It.IsAny<int>()));
            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(obj => obj.RequestUri == requestUri && obj.Method == HttpMethod.Get),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.NotFound))
                .Verifiable();
            UseValidApiKey();

            var result = await _service.GetBook(It.IsAny<int>());

            _httpMessageHandlerMock.Verify();

            result.Should().Be(null);
        }

        [Test]
        public async Task GetBook_BookWithEnteredIdExists_ReturnOuterBookDtoObject()
        {
            var requestUri = new Uri(string.Format(
                "{0}/book/show?key={1}&id={2}",
                _baseUrl,
                _validApiKey,
                It.IsAny<int>()));
            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(obj => obj.RequestUri == requestUri && obj.Method == HttpMethod.Get),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent("<book></book>")
                })
                .Verifiable();
            UseValidApiKey();

            var result = await _service.GetBook(It.IsAny<int>());

            _httpMessageHandlerMock.Verify();

            result.Should().BeOfType<OuterBookDto>();
        }

        private void UseValidApiKey()
        {
            _goodreadsSettings.ApiKey = _validApiKey;
        }

        private void UseInvalidApiKey()
        {
            _goodreadsSettings.ApiKey = _invalidApiKey;
        }

        private class GoodreadsServiceProxy : GoodreadsService
        {
            public GoodreadsServiceProxy(HttpClient httpClient, IOptions<GoodreadsSettings> settings) : base(httpClient, settings)
            {
            }

            public async Task<HttpResponseMessage> SendGetRequestProxy(string requestUrl) => await base.SendGetRequest(requestUrl);

            public OuterBookDto GetBookFromXmlProxy(XmlNode bookNode) => base.GetBookFromXml(bookNode);

            protected override OuterBookDto GetBookFromXml(XmlNode xmlBookNode)
            {
                return new OuterBookDto();
            }
        }
    }
}
