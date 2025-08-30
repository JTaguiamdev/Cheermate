using Cheermate.Application.Services;

namespace Cheermate.Infrastructure.Services;

public class SystemDateTimeProvider : IDateTimeProvider
{
    public DateTime NowLocal => DateTime.Now;
    public DateTime UtcNow => DateTime.UtcNow;
}