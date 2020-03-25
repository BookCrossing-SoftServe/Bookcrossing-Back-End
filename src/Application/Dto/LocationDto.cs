using System.Collections.Generic;

namespace Application.Dto
{
    public class LocationDto
    {
        public int Id { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string OfficeName { get; set; }
        public List<int> Rooms { get; set; }
    }
}
