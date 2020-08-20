using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.RDBMS.Entities
{
    [Flags]
    public enum NotificationAction
    {
        None,
        Open,
        Request,
        StartReading
    }
}
