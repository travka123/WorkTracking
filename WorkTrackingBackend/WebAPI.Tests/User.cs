using System.Net.Http.Json;
using WebAPI.Data.Views;
using WebAPI.Tests.Utils;
using Xunit;

namespace WebAPI.Tests;

// admin/user/firm/task

public class User
{
    [Fact]
    public async void LoginTest()
    {
        var application = new WTApiApplication();
        var client = application.CreateClient();
        var userInfo = await TestHelper.Authorize(client, "user1.1", "user1.1");
        Assert.True(userInfo.role == "User");
    }

    [Fact]
    public async void GetTasksTest()
    {
        var application = new WTApiApplication();
        var client = application.CreateClient();
        var userInfo = await TestHelper.Authorize(client, "user1.1", "user1.1");
        var response = await client.GetAsync("/user/tasks");
        var tasks = (await (response).Content.ReadFromJsonAsync<IEnumerable<TaskView>>())!;
        Assert.True(tasks.All(t => t.name.StartsWith("task1.1")));
    }
}