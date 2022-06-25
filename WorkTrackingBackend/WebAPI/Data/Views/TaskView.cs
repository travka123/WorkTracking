﻿using Entities;

namespace WebAPI.Data.Views;

public record TaskView(int id, string name, ItemView unit, int quantity, 
    string description, DateTime reportingDate, ItemView firm, DateTime creationDate)
{
    public TaskView(AccountableTask task) : this(task.Id, task.Name, 
        new ItemView(task.Unit.Id, task.Unit.Name), task.Quantity, task.Description,
        task.ReportingDate, new ItemView(task.Firm.Id, task.Firm.Name), task.CreationDate)
    {

    }
}