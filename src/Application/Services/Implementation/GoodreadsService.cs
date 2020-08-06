using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Application.Dto;
using Application.Dto.OuterSource;
using Application.Dto.QueryParams;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;
using Org.BouncyCastle.Security;

namespace Application.Services.Implementation
{
    public class GoodreadsService : IOuterBookSourceService
    {
        private readonly GoodreadsSettings _settings;
        private readonly HttpClient _httpClient;

        public GoodreadsService(HttpClient httpClient, IOptions<GoodreadsSettings> settings)
        {
            _httpClient = httpClient;
            _settings = settings.Value;
            if (string.IsNullOrWhiteSpace(_settings.ApiKey))
            {
                throw new Exception("Api key cannot be null or white spaces");
            }
        }

        public async Task<PaginationDto<OuterBookDto>> SearchBooks(OuterSourceQueryParameters query)
        {
            var response = await SendGetRequest(
                $"search/index.xml?key={_settings.ApiKey}&page={query?.Page}&q={query?.SearchTerm}&per_page={query?.PageSize}");

            response.EnsureSuccessStatusCode();

            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(await response.Content.ReadAsStringAsync());

            var totalCountNode = xmlDocument.SelectSingleNode("//search/total-results");
            int.TryParse(totalCountNode?.InnerText, out var totalCountOfBooks);

            var bookNodes = xmlDocument.SelectNodes("//results//best_book[@type='Book']");
            var bookList = new List<OuterBookDto>();
            foreach (XmlNode bookNode in bookNodes)
            {
                bookList.Add(GetBookFromXml(bookNode));
            }

            return new PaginationDto<OuterBookDto>
            {
                Page = bookList,
                TotalCount = totalCountOfBooks
            };
        }

        public async Task<OuterBookDto> GetBook(int bookId)
        {
            var response = await SendGetRequest($"book/show?key={_settings.ApiKey}&id={bookId}");
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            response.EnsureSuccessStatusCode();

            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(await response.Content.ReadAsStringAsync());

            var bookNode = xmlDocument.SelectSingleNode("//GoodreadsResponse/book");

            return GetBookFromXml(bookNode);
        }

        protected virtual async Task<HttpResponseMessage> SendGetRequest(string requestUrl)
        {
            var response = await _httpClient.GetAsync(requestUrl);
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new InvalidKeyException("Api key for 'Goodreads' is invalid");
            }

            return response;
        }

        protected virtual OuterBookDto GetBookFromXml(XmlNode xmlBookNode)
        {
            if (xmlBookNode == null)
            {
                throw new ArgumentNullException(nameof(xmlBookNode), "Xml node of book cannot be null");
            }

            var xmlReader = new XmlNodeReader(xmlBookNode);
            var serializer = new XmlSerializer(typeof(OuterBookDto), new XmlRootAttribute(xmlBookNode.Name));
            var obj = serializer.Deserialize(xmlReader) as OuterBookDto;

            return obj ?? new OuterBookDto();
        }
    }
}