namespace Entities;

public class Firm
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int OwnershipSystemId { get; set; }
    public int TaxationSystemId { get; set; }
    public string Description { get; set; }
    public int ManagerId { get; set; }
}
