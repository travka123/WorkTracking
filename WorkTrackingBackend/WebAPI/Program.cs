using EFData;
using EFData.Repositories;
using Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text;
using UseCases.Repositories;
using UseCases.UseCases;
using WebAPI.Utils;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

string dbConnection = builder.Configuration.GetConnectionString("msdbconstr");
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(dbConnection);
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = AuthHelper.TokenValidationParameters;
});
builder.Services.AddAuthorization();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ITaskRepository, TaskRepository>();

builder.Services.AddScoped<AddUserInteractor>();
builder.Services.AddScoped<GetUserTasksInteractor>();
builder.Services.AddScoped<UserLoginInteractor>();

var app = builder.Build();

app.UseCors(builder => builder.WithOrigins("http://localhost:3000").AllowAnyMethod().AllowAnyHeader().AllowCredentials());

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapPost("/login", ([FromBody] UserLoginRequest request, [FromServices] UserLoginInteractor interactor) =>
{
    var user = interactor.Handle(request).user;
    if (user is null)
    {
        return Results.Unauthorized();
    }
    string token = AuthHelper.CreateToken(user.Id, user.GetType().ToString());
    return Results.Ok(new { token = token, userId = user.Id, login = user.Login, role = user.GetType().Name });
});

app.MapPost("/user", [Authorize(Roles = "Administrator")] ([FromBody] UserAddForm request,
    ClaimsPrincipal claimsPrincipal, [FromServices] AddUserInteractor interactor) =>
{
    int userId = int.Parse(claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)!.Value);
    var result = interactor.Handle(new AddUserRequest(userId, request.login, request.password));
    return result.user;
});

app.MapGet("/tasks", [Authorize] (ClaimsPrincipal claimsPrincipal, [FromServices] GetUserTasksInteractor interactor) =>
{
    int userId = int.Parse(claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)!.Value);
    var result = interactor.Handle(new GetUserTasksRequest(userId));
    return result.tasks;
});

FillDatabase();

app.Run();

async void FillDatabase()
{
    var builder = new DbContextOptionsBuilder<AppDbContext>();
    builder.UseSqlServer(dbConnection);
    using (var context = new AppDbContext(builder.Options))
    {
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        List<OwnershipSystem> ownershipSystems = new List<OwnershipSystem>()
        {
            new OwnershipSystem() {Name = "��"},
            new OwnershipSystem() {Name = "�����������(�����������)"},
            new OwnershipSystem() {Name = "���"},
            new OwnershipSystem() {Name = "���"},
            new OwnershipSystem() {Name = "���"},
            new OwnershipSystem() {Name = "���"},
            new OwnershipSystem() {Name = "��"},
            new OwnershipSystem() {Name = "��"},
        };
        context.OwnershipSystems.AddRange(ownershipSystems);

        List<TaxationSystem> taxationSystems = new List<TaxationSystem>()
        {
            new TaxationSystem() {Name="��� � ��� � ����� (�� ������)"},
            new TaxationSystem() {Name="��� � ��� � ����� (�� ��������)"},
            new TaxationSystem() {Name="��� � ��� ���. ����."},
            new TaxationSystem() {Name="��� ��� ��� � ����� (�� ������)"},
            new TaxationSystem() {Name="��� ��� ��� � ����� (�� ��������)"},
            new TaxationSystem() {Name="��� ��� ��� ���. ����."},
            new TaxationSystem() {Name="���/����������."},
            new TaxationSystem() {Name="������ �����"},
            new TaxationSystem() {Name="������"},
        };
        context.TaxationSystems.AddRange(taxationSystems);

        Administrator superadmin = new Administrator()
        {
            Login = "admin",
            Password = Encoding.UTF8.GetBytes("admin"),
        };
        context.Administrators.Add(superadmin);

        await context.SaveChangesAsync();
    }
}

public record UserAddForm(string login, byte[] password);