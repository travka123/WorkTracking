using Entities;

namespace WebAPI.Data.Views;

public record AdminFirmView(int id, string name, ItemView ownershipSystem, ItemView taxationSystem, 
    string description, ItemView manager)
{
    public AdminFirmView(Firm firm) : this(firm.Id, firm.Name, new ItemView(firm.OwnershipSystem.Id, firm.OwnershipSystem.Name),
        new ItemView(firm.TaxationSystem.Id, firm.TaxationSystem.Name), firm.Description,
        new ItemView(firm.Manager.Id, firm.Manager.Login))
    {

    }
}
