using Cheermate.Application.Services;

namespace Cheermate.Infrastructure.Services;

public class SystemGreetingService : IGreetingService
{
    private readonly IDateTimeProvider _time;

    public SystemGreetingService(IDateTimeProvider time)
    {
        _time = time;
    }

    public string GetGreeting(string name)
    {
        var now = _time.NowLocal;
        var dayPart = now.Hour switch
        {
            >= 5 and < 12 => "Good morning",
            >= 12 and < 18 => "Good afternoon",
            >= 18 and < 22 => "Good evening",
            _ => "Hello"
        };
        return $"{dayPart}, {name}! Time: {now:T}";
    }
}