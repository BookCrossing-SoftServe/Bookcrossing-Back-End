using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Application.Dto;
using Application.Dto.OuterSource;
using Application.Dto.QueryParams;
using Application.Services.Interfaces;
using Domain.RDBMS.Entities;
using Microsoft.Extensions.Options;

namespace Application.Services.Implementation
{
    public class GoodreadsService : IOuterBookSourceService
    {
        private string _apiKey;
        private HttpClient _httpClient;

        public GoodreadsService(HttpClient httpClient, IOptions<GoodreadsSettings> settings)
        {
            _httpClient = httpClient;
            _apiKey = settings.Value.ApiKey;
        }

        public async Task<PaginationDto<OuterBookDto>> SearchBooks(OuterSourceQueryParameters query)
        {
            var response = await _httpClient.GetStringAsync(
                string.Format("search/index.xml?key={0}&page={1}&q={2}&per_page={3}", _apiKey, query.Page, query.SearchTerm, query.PageSize));
            var xmlReader = new XmlDocument();
            xmlReader.LoadXml(response);

            var bookNodes = xmlReader.SelectNodes("//results//best_book[@type='Book']");
            var bookList = new List<OuterBookDto>();
            foreach (XmlNode bookNode in bookNodes)
            {
                bookList.Add(GetBookFromXml(bookNode));
            }

            return new PaginationDto<OuterBookDto>
            {
                Page = bookList
            };
        }

        public async Task<OuterBookDto> GetBook(int? bookId)
        {
            var response = await _httpClient.GetStringAsync(
                string.Format("book/show?key={0}&id={1}", _apiKey, bookId));
            var xmlReader = new XmlDocument();
            xmlReader.LoadXml(response);
            var bookNode = xmlReader.SelectSingleNode("//GoodreadsResponse/book");

            return GetBookFromXml(bookNode);
        }

        protected virtual OuterBookDto GetBookFromXml(XmlNode xmlBookNode)
        {
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
    };
}
