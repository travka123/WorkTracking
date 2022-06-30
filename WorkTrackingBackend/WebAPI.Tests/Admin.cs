using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Json;
using System.Text;
using UseCases.Repositories;
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
    public async void UserAddTest()
    {
        var application = new WTApiApplication();
        var client = application.CreateClient();
        await TestHelper.Authorize(client, "admin1", "admin1");

        var response = await client.PostAsJsonAsync("/users", new
        {
            login = "user1.special",
            password = TestHelper.ToBase64("user1.special")
        });

        Assert.True(response.IsSuccessStatusCode);

        using (var scope = application.Services.CreateScope())
        {
            var userRepository = scope.ServiceProvider.GetService<IUserRepository>()!;

            int selfId = userRepository.Users.Where(u => u.Login == "admin1").Select(u => u.Id).Single();
            var user = userRepository.Users.Where(u => u.Login == "user1.special").Single();

            Assert.True(user.AdministratorId == selfId);
        }
    }

    [Fact]
    public async void GetUsersTasksTest()
    {
        const string tasksNamesStartWith = "task1.";

        var application = new WTApiApplication();
        var client = application.CreateClient();
        await TestHelper.Authorize(client, "admin1", "admin1");

        var response = await client.GetAsync("/admin/tasks");
        var actualTasks = (await response.Content.ReadFromJsonAsync<IEnumerable<UserTaskView>>())!;

        using (var scope = application.Services.CreateScope())
        {
            var taskRepository = scope.ServiceProvider.GetService<ITaskRepository>()!;
            int expectedCount = taskRepository.AccountableTasks
                .Where(t => t.Name.StartsWith(tasksNamesStartWith))
                .Count();
            Assert.True(actualTasks.Count() == expectedCount);
            Assert.True(actualTasks.All(t => t.name.StartsWith(tasksNamesStartWith)));
        }
    }
}
