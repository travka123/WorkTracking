using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;

namespace WebAPI.Tests.Utils;

public class TestHelper
{
    public static async Task<LoginInfo> GetLoginInfo(HttpClient client, string login, string password)
    {
        var loginResponse = await client.PostAsJsonAsync("/login", new
        {
            login,
            password = ToBase64(password)
        });
        return (await loginResponse.Content.ReadFromJsonAsync<LoginInfo>())!;
    }

    public static async Task<LoginInfo> Authorize(HttpClient client, string login, string password)
    {  
        var userInfo = (await GetLoginInfo(client, login, password));
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userInfo.token);
        return userInfo;
    }

    public record LoginInfo(string token, string login, string role);

    public static string ToBase64(string data) => Convert.ToBase64String(Encoding.UTF8.GetBytes(data));
}
