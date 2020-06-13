using Domain.RDBMS.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Dto.Dashboard
{   
    public class AvailabilityDataDto
    {
        public int Available { get; set; }
        public int Requested { get; set; }
        public int Reading { get; set; }
        public int Deactivated { get; set; }
    }
}
