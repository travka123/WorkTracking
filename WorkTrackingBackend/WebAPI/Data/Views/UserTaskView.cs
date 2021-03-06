using Entities;

namespace WebAPI.Data.Views;

public record UserTaskView(int id, string name, ItemView unit, int quantity, 
    string description, DateTime reportingDate, ItemView firm, DateTime creationDate)
{
    public UserTaskView(AccountableTask task) : this(task.Id, task.Name, 
        new ItemView(task.Unit.Id, task.Unit.Name), task.Quantity, task.Description,
        task.ReportingDate, new ItemView(task.Firm.Id, task.Firm.Name), task.CreationDate)
    {

    }

    public UserTaskView() : this(0, String.Empty, new ItemView(0, String.Empty), 0, String.Empty,
        DateTime.MinValue, new ItemView(0, String.Empty), DateTime.MinValue)
    {

    }
}