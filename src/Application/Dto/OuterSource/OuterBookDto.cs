using System;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;

namespace Application.Dto.OuterSource
{
    public class OuterBookDto
    {
        [XmlElement("id")]
        public int? Id { get; set; }

        [XmlElement("title")]
        public string Title { get; set; }

        [XmlArray("authors")]
        [XmlArrayItem("author")]
        public OuterAuthorDto[] Authors { get; set; }

        [XmlElement("author")]
        public OuterAuthorDto Author { get; set; }

        [XmlElement("publisher")]
        public string Publisher { get; set; }

        [XmlElement("language_code")]
        public string LanguageCode { get; set; }

        [XmlElement("image_url")]
        public string ImageUrl { get; set; }

        [XmlElement("description")]
        public string Description { get; set; }
    }
}
