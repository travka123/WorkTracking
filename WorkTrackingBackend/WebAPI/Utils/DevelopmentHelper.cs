using EFData;
using Entities;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace WebAPI.Utils;

public class DevelopmentHelper
{
    public static async Task FillDatabase(string connection)
    {
        var builder = new DbContextOptionsBuilder<AppDbContext>();
        builder.UseSqlServer(connection);
        using (var context = new AppDbContext(builder.Options))
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

            Administrator superadmin = new Administrator()
            {
                Login = "admin",
                Password = Encoding.UTF8.GetBytes("admin"),
            };
            context.Administrators.Add(superadmin);

            Firm testFirm = new Firm()
            {
                Name = "Рога и копыта",
                Description = "desc",
                Manager = superadmin,
                OwnershipSystem = ownershipSystems[0],
                TaxationSystem = taxationSystems[0]
            };

            context.Firms.Add(testFirm);

            AccountableTask testTask = new AccountableTask()
            {
                Name = "test task",
                CreationDate = DateTime.Now,
                Description = "abcd",
                Firm = testFirm,
                Quantity = 1,
                ReportingDate = DateTime.Now,
                Unit = new Unit() { Name = "qwerty" }
            };

            context.Tasks.Add(testTask);

            await context.SaveChangesAsync();
        }
    }
}
