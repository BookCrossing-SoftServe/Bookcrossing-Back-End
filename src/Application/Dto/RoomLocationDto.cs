using System;
using System.Collections.Generic;
using System.Text;
using Domain.Entities;

namespace Application.Dto
{
    public class RoomLocationDto
    {
       public LocationDto Location { get; set; }
       public int RoomNumber { get; set; }
    }
}
