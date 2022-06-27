using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Json;
using UseCases.Repositories;
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
        const string tasksNamesStartWith = "task1.1.";

        var application = new WTApiApplication();
        var client = application.CreateClient();
        await TestHelper.Authorize(client, "user1.1", "user1.1");

        var response = await client.GetAsync("/user/tasks");
        var actualTasks = (await response.Content.ReadFromJsonAsync<IEnumerable<TaskView>>())!;

        using (var scope = application.Services.CreateScope())
        {
            var taskRepository = scope.ServiceProvider.GetService<ITaskRepository>()!;
            int expectedCount = taskRepository.AccountableTasks.Where(t => t.Name.StartsWith(tasksNamesStartWith)).Count();
            Assert.True(actualTasks.Count() == expectedCount);
            Assert.True(actualTasks.All(t => t.name.StartsWith(tasksNamesStartWith)));
        }
    }

    [Fact]
    public async void GetFirmsTest()
    {
        const string firmsNamesStartWith = "firm1.1.";

        var application = new WTApiApplication();
        var client = application.CreateClient();
        await TestHelper.Authorize(client, "user1.1", "user1.1");

        var response = await client.GetAsync("/user/firms");
        var actualFirms = (await response.Content.ReadFromJsonAsync<IEnumerable<ItemView>>())!;

        using (var scope = application.Services.CreateScope())
        {
            var firmRepository = scope.ServiceProvider.GetService<IFirmRepository>()!;
            int expectedCount = firmRepository.Firms.Where(t => t.Name.StartsWith(firmsNamesStartWith)).Count();
            Assert.True(actualFirms.Count() == expectedCount);
            Assert.True(actualFirms.All(t => t.name.StartsWith(firmsNamesStartWith)));
        }
    }
}