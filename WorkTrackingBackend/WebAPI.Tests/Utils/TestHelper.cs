using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;

namespace WebAPI.Tests.Utils;

public class TestHelper
{
    public static async Task<LoginInfo> Authorize(HttpClient client, string login, string password)
    {
        var loginResponse = await client.PostAsJsonAsync("/login", new
        {
            login,
            password = Convert.ToBase64String(Encoding.UTF8.GetBytes(password))
        });
        var userInfo = (await loginResponse.Content.ReadFromJsonAsync<LoginInfo>())!;

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userInfo.token);

        return userInfo!;
    }

    public record LoginInfo(string token, string login, string role);
}
