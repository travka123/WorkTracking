namespace WebAPI.Utils;

public class FormChecker
{
    public static void CheckForNull<T>(T form)
    {
        foreach (var prop in typeof(T).GetProperties())
        {
            if (prop.GetValue(form) is null)
            {
                throw new BadHttpRequestException($"{prop.Name} is null", 400);
            }
        }
    }
}
