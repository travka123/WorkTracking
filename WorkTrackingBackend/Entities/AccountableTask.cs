namespace Entities;

public class AccountableTask
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int UnitId { get; set; }
    public Unit Unit { get; set; }
    public int Quantity { get; set; }
    public string Description { get; set; }
    public DateTime ReportingDate { get; set; }
    public DateTime CreationDate { get; set; }
    public int FirmId { get; set; }
    public Firm Firm { get; set; }
}