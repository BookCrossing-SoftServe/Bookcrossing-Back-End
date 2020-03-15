using System;
using System.Collections.Generic;
using System.Text;
using Domain.Entities;

namespace Application.Models
{
    public class UserProfileModel
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public UserLocation UserLocation { get; set; }
        public virtual List<Book> AllUserBooks { get; set; } 

    }
}
