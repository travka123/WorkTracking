namespace Entities;

public class Firm
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int OwnershipSystemId { get; set; }
    public OwnershipSystem OwnershipSystem { get; set; }
    public int TaxationSystemId { get; set; }
    public TaxationSystem TaxationSystem { get; set; }
    public string Description { get; set; }
    public int ManagerId { get; set; }
    public User Manager { get; set; }
}
