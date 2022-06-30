using Entities;

namespace WebAPI.Data.Views;

public record AdminTaskView(int id, string name, ItemView unit, int quantity,
    string description, DateTime reportingDate, AdminFirmView firm, DateTime creationDate)
{
    public AdminTaskView(AccountableTask task) : this(task.Id, task.Name,
        new ItemView(task.Unit.Id, task.Unit.Name), task.Quantity, task.Description,
        task.ReportingDate, new AdminFirmView(task.Firm), task.CreationDate)
    {

    }
}
