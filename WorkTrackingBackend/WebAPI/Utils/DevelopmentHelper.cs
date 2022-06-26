using EFData;
using Entities;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace WebAPI.Utils;

public class DevelopmentHelper
{
    public static async Task FillDatabase(AppDbContext context)
    {

        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        List<OwnershipSystem> ownershipSystems = new List<OwnershipSystem>()
        {
            new OwnershipSystem() {Name = "ИП"},
            new OwnershipSystem() {Name = "Самозанятый(ремесленник)"},
            new OwnershipSystem() {Name = "ООО"},
            new OwnershipSystem() {Name = "ОАО"},
            new OwnershipSystem() {Name = "ЗАО"},
            new OwnershipSystem() {Name = "ОДО"},
            new OwnershipSystem() {Name = "УП"},
            new OwnershipSystem() {Name = "ГУ"},
        };
        context.OwnershipSystems.AddRange(ownershipSystems);

        List<TaxationSystem> taxationSystems = new List<TaxationSystem>()
        {
            new TaxationSystem() {Name="УСН с НДС с КУДИР (по оплате)"},
            new TaxationSystem() {Name="УСН с НДС с КУДИР (по отгрузке)"},
            new TaxationSystem() {Name="УСН с НДС бух. учёт."},
            new TaxationSystem() {Name="УСН без НДС с КУДИР (по оплате)"},
            new TaxationSystem() {Name="УСН без НДС с КУДИР (по отгрузке)"},
            new TaxationSystem() {Name="УСН без НДС бух. учёт."},
            new TaxationSystem() {Name="ОСН/подоходный."},
            new TaxationSystem() {Name="Единый налог"},
            new TaxationSystem() {Name="Прочее"},
        };
        context.TaxationSystems.AddRange(taxationSystems);

        List<Unit> units = new()
            {
                new Unit() {Name = "day"},
                new Unit() {Name = "month"},
                new Unit() {Name = "year"},
            };

        context.Units.AddRange(units);

        // admin/user/firm/task

        Administrator admin1 = new Administrator()
        {
            Login = "admin1",
            Password = Encoding.UTF8.GetBytes("admin1"),
        };

        Administrator admin2 = new Administrator()
        {
            Login = "admin2",
            Password = Encoding.UTF8.GetBytes("admin2"),
        };

        context.Administrators.Add(admin1);
        context.Administrators.Add(admin2);

        User user1 = new User()
        {
            Login = "user1.1",
            Password = Encoding.UTF8.GetBytes("user1.1"),
            Administrator = admin1
        };

        User user2 = new User()
        {
            Login = "user2.1",
            Password = Encoding.UTF8.GetBytes("user2.1"),
            Administrator = admin2
        };

        context.Users.Add(user1);
        context.Users.Add(user2);

        Firm firm11 = new Firm()
        {
            Name = "firm1.1.1",
            Description = "desc1.1",
            Manager = user1,
            OwnershipSystem = ownershipSystems[0],
            TaxationSystem = taxationSystems[0]
        };

        Firm firm12 = new Firm()
        {
            Name = "firm1.1.2",
            Description = "desc1.2",
            Manager = user1,
            OwnershipSystem = ownershipSystems[1],
            TaxationSystem = taxationSystems[1]
        };

        Firm firm2 = new Firm()
        {
            Name = "firm2.1.1",
            Description = "desc2",
            Manager = user2,
            OwnershipSystem = ownershipSystems[1],
            TaxationSystem = taxationSystems[1]
        };

        context.Firms.Add(firm11);
        context.Firms.Add(firm12);
        context.Firms.Add(firm2);

        List<AccountableTask> tasks = new()
            {
                new AccountableTask()
                {
                    Name = "task1.1.1.1",
                    CreationDate = DateTime.Now,
                    Description = "abcd",
                    Firm = firm11,
                    Quantity = 1,
                    ReportingDate = DateTime.Now,
                    Unit = units[0],
                },
                new AccountableTask()
                {
                    Name = "task1.1.1.2",
                    CreationDate = DateTime.Now,
                    Description = "dcba",
                    Firm = firm11,
                    Quantity = 1,
                    ReportingDate = DateTime.Now,
                    Unit = units[0],
                },
                new AccountableTask()
                {
                    Name = "task1.1.2.1",
                    CreationDate = DateTime.Now,
                    Description = "dcba",
                    Firm = firm12,
                    Quantity = 1,
                    ReportingDate = DateTime.Now,
                    Unit = units[1],
                },
                new AccountableTask()
                {
                    Name = "task2.1.1.1",
                    CreationDate = DateTime.Now,
                    Description = "q q q q",
                    Firm = firm2,
                    Quantity = 1,
                    ReportingDate = DateTime.Now,
                    Unit = units[2],
                },
            };

        context.Tasks.AddRange(tasks);

        await context.SaveChangesAsync();

    }
}
