using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.RDBMS.Entities
{
    public enum BookState
    {
        Available,
        Requested,
        Reading,
        InActive,
        RequestedFromCompany
    }
}
