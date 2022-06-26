using System.Net.Http.Json;
using WebAPI.Data.Views;
using WebAPI.Tests.Utils;
using Xunit;

namespace WebAPI.Tests;

// admin/user/firm/task

public class Admin
{
    [Fact]
    public async void LoginTest()
    {
        var application = new WTApiApplication();
        var client = application.CreateClient();
        var userInfo = await TestHelper.Authorize(client, "admin1", "admin1");
        Assert.True(userInfo.role == "Administrator");
    }

    [Fact]
    public async void GetUsersTasksTest()
    {
        var application = new WTApiApplication();
        var client = application.CreateClient();
        await TestHelper.Authorize(client, "admin1", "admin1");
        var response = await client.GetAsync("/admin/tasks");
        var tasks = (await (response).Content.ReadFromJsonAsync<IEnumerable<TaskView>>())!;
        Assert.True(tasks.All(t => t.name.StartsWith("task1.")));
    }
}
