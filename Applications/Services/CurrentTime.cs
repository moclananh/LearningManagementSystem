using Applications.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Services;

public class CurrentTime : ICurrentTime
{
    DateTime ICurrentTime.CurrentTime()
    {
        return DateTime.UtcNow;
    }
}
