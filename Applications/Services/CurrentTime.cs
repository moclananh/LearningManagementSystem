using Applications.Interfaces;

namespace Applications.Services;

public class CurrentTime : ICurrentTime
{
    DateTime ICurrentTime.CurrentTime()
    {
        return DateTime.UtcNow;
    }
}
