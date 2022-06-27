namespace WebAPI.Data.Views;

public record ItemView(int id, string name) 
{
    public ItemView() : this(0, String.Empty)
    {

    }
}
