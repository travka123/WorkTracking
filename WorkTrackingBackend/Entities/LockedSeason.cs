namespace Entities;

public class LockedSeason
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public int AdministratorId { get; set; }
    public Administrator Administrator { get; set; }
}
