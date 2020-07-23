using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Application.Dto.OuterSource
{
    public class OuterAuthorDto
    {
        [XmlElement("name")]
        public string Name { get; set; }
    }
}
