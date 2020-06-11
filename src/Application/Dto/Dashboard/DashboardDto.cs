using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Dto.Dashboard
{
    public class DashboardDto
    {
        public List<string> Cities { get; set; }
        public LocationDataDto LocationData { get; set; }
        public BookUserDataDto BookUserComparisonData { get; set; }
        public Dictionary<string, AvailabilityDataDto> AvailabilityData { get; set; }
    }
}
