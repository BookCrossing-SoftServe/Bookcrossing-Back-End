using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml;
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
        private readonly string _apiKey;
        private readonly HttpClient _httpClient;

        public GoodreadsService(HttpClient httpClient, IOptions<GoodreadsSettings> settings)
        {
            _httpClient = httpClient;
            _apiKey = settings.Value?.ApiKey;
            if (string.IsNullOrWhiteSpace(_apiKey))
            {
                throw new ArgumentException("GoodreadsSettings ApiKey cannot be null or white space", nameof(settings));
            }
        }

        public async Task<PaginationDto<OuterBookDto>> SearchBooks(OuterSourceQueryParameters query)
        {
            var response = await _httpClient.GetAsync(
                string.Format("search/index.xml?key={0}&page={1}&q={2}&per_page={3}", _apiKey, query.Page, query.SearchTerm, query.PageSize));
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new InvalidKeyException("Api key for 'Goodreads' is invalid");
            }

            response.EnsureSuccessStatusCode();

            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(await response.Content.ReadAsStringAsync());

            var totalCountNode = xmlDocument.SelectSingleNode("//search/total-results");
            var totalCountOfBooks = int.Parse(totalCountNode?.InnerText ?? "0");

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
            var response = await _httpClient.GetAsync(
                string.Format("book/show?key={0}&id={1}", _apiKey, bookId));
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new InvalidKeyException("Api key for 'Goodreads' is invalid");
            }

            response.EnsureSuccessStatusCode();

            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(await response.Content.ReadAsStringAsync());

            var bookNode = xmlDocument.SelectSingleNode("//GoodreadsResponse/book");

            return GetBookFromXml(bookNode);
        }

        protected virtual OuterBookDto GetBookFromXml(XmlNode xmlBookNode)
        {
            if (xmlBookNode == null)
            {
                throw new ArgumentNullException(nameof(xmlBookNode), "Xml node of book cannot be null");
            }

            var id = int.Parse(xmlBookNode.SelectSingleNode("id")?.InnerText ?? "0");
            var title = xmlBookNode.SelectSingleNode("title")?.InnerText;
            var imageUrl = xmlBookNode.SelectSingleNode("image_url")?.InnerText;
            var publisher = xmlBookNode.SelectSingleNode("publisher")?.InnerText;
            var langCode = xmlBookNode.SelectSingleNode("language_code")?.InnerText;
            var description = xmlBookNode.SelectSingleNode("description")?.InnerText;
            var authorNodes = xmlBookNode.SelectNodes("./authors/author | ./author");
            var authors = new List<string>();
            foreach (XmlNode authorNode in authorNodes)
            {
                var name = authorNode.SelectSingleNode("name")?.InnerText;
                authors.Add(name);
            }

            return new OuterBookDto()
            {
                Id = id,
                Title = title,
                AuthorsFullNames = authors.ToArray(),
                ImageUrl = imageUrl,
                LanguageCode = langCode,
                Publisher = publisher,
                Description = description
            };
        }
    }
}