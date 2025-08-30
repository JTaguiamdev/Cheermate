namespace Cheermate.Application.Services;

public interface IDateTimeProvider
{
    DateTime NowLocal { get; }
    DateTime UtcNow { get; }
}