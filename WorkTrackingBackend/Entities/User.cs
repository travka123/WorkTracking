namespace Entities;

public class User
{
    public int Id { get; set; }
    public string Login { get; set; }
    public byte[] Password { get; set; }
    public int? AdministratorId { get; set; }
    public Administrator? Administrator { get; set; }
}