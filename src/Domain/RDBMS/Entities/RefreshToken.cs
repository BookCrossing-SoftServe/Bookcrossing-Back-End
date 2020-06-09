using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.RDBMS.Entities
{
    public class RefreshToken : IEntityBase
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public int UserId { get; set; }

        public User User { get; set; }
    }
}
