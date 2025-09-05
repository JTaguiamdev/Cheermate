using Xunit;
using Cheermate.Domain.Entities;

namespace Cheermate.Tests.Domain;

public class TodoTaskTests
{
    [Fact]
    public void New_task_has_expected_title_and_description()
    {
        var task = new TodoTask
        {
            Title = "Sample",
            Description = "Desc"
        };

        Assert.Equal("Sample", task.Title);
        Assert.Equal("Desc", task.Description);
    }
}