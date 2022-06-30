using EFData;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using UseCases.UseCases;
using WebAPI.Data.Forms;
using WebAPI.Data.Views;
using WebAPI.Middleware;
using WebAPI.ServiceCollectionExtensions;
using WebAPI.Utils;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors();

string dbConnection = builder.Configuration.GetConnectionString("msdbconstr");

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(dbConnection);
});

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
{
    options.TokenValidationParameters = AuthHelper.TokenValidationParameters;
});

builder.Services.AddAuthorization();

builder.Services.AddAppServices();

var app = builder.Build();

app.UseCors(builder => builder
    .WithOrigins("http://localhost:3000")
    .AllowAnyMethod()
    .AllowAnyHeader()
    .AllowCredentials());

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapPost("/login", ([FromBody] LoginForm form, [FromServices] LoginInteractor login) =>
{
    FormChecker.CheckForNull(form);
    var user = login.Handle(new LoginRequest(form.login!, form.password!));
    string token = AuthHelper.CreateToken(user.Id, user.GetType().Name.ToString());
    return Results.Ok(new { token = token, userId = user.Id, login = user.Login, role = user.GetType().Name });
});

app.MapPost("/users", [Authorize(Roles = "Administrator")] ([FromBody] UserAddForm form,
    ClaimsPrincipal claimsPrincipal, [FromServices] AdminAddUserInteractor addUser) =>
{
    FormChecker.CheckForNull(form);
    int userId = AuthHelper.GetUserId(claimsPrincipal);
    var result = addUser.Handle(new AdminAddUserRequest(userId, form.login, form.password));
    return result.user;
});

app.MapGet("/user/tasks", [Authorize] (ClaimsPrincipal claimsPrincipal, 
    [FromServices] UserGetTasksInteractor getTasks) =>
{
    int userId = AuthHelper.GetUserId(claimsPrincipal);
    var result = getTasks.Handle(userId);
    return result.Select(t => new TaskView(t)).ToList();
});

app.MapPost("/user/tasks", [Authorize] ([FromBody] TaskAddForm form, ClaimsPrincipal claimsPrincipal, 
    [FromServices] AddTaskInteractor addTask, [FromServices] UserGetFirmsInteractor getFirms) =>
{
    FormChecker.CheckForNull(form);
    int userId = AuthHelper.GetUserId(claimsPrincipal);
    var task = addTask.Handle(new AddTaskRequest(getFirms, form.name!, form.unitId!.Value, 
        form.quantity!.Value, form.description!, form.reportingDate!.Value, form.firmId!.Value,
        userId));
    return Results.Created($"/user/tasks/{task.Id}", new TaskView(task));
});

app.MapDelete("/user/tasks", [Authorize] ([FromBody] IEnumerable<int> tasksIds, ClaimsPrincipal claimsPrincipal,
    [FromServices] RemoveTasksInteractor removeTasks, [FromServices] UserGetTasksInteractor getTasks) =>
{
    int userId = AuthHelper.GetUserId(claimsPrincipal);
    var result = removeTasks.Handle(new RemoveTasksRequest(getTasks, tasksIds, userId));
    return result ? Results.Ok() : Results.BadRequest();
});

app.MapPut("/user/tasks", [Authorize] ([FromBody] IEnumerable<TaskUpdate> form, ClaimsPrincipal claimsPrincipal,
    [FromServices] UpdateTasksInteractor updateTasks, [FromServices] UserGetTasksInteractor getTasks,
    [FromServices] UserGetFirmsInteractor getFirms) =>
{
    int userId = AuthHelper.GetUserId(claimsPrincipal);
    var result = updateTasks.Handle(new UpdateTasksRequest(getTasks, getFirms, form, userId));
    return result ? Results.Ok() : Results.BadRequest();
});

app.MapGet("/user/firms", [Authorize] (ClaimsPrincipal claimsPrincipal, 
    [FromServices] UserGetFirmsInteractor getFirms) =>
{
    int userId = AuthHelper.GetUserId(claimsPrincipal);
    var result = getFirms.Handle(userId);
    return result.Select(f => new ItemView(f.Id, f.Name)).ToList();
});

app.MapGet("/user/units", [Authorize] (ClaimsPrincipal claimsPrincipal,
    [FromServices] GetUnitsInteractor getUnits) =>
{
    int userId = AuthHelper.GetUserId(claimsPrincipal);
    var result = getUnits.Handle(userId);
    return result.Select(f => new ItemView(f.Id, f.Name)).ToList();
});

app.MapGet("/admin/tasks", [Authorize(Roles = "Administrator")] (ClaimsPrincipal claimsPrincipal,
    [FromServices] AdminGetTasksInteractor getTasks) =>
{
    int userId = AuthHelper.GetUserId(claimsPrincipal);
    return getTasks.Handle(userId).Select(t => new TaskView(t)).ToList();
});

app.MapGet("/admin/firms", [Authorize(Roles = "Administrator")] (ClaimsPrincipal claimsPrincipal,
    [FromServices] AdminGetFirmsInteractor getFirms) =>
{
    int userId = AuthHelper.GetUserId(claimsPrincipal);
    var result = getFirms.Handle(userId);
    return result.Select(f => new FirmView(f)).ToList();
});

if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetService<AppDbContext>();
        if (context is not null)
        {
            await DevelopmentHelper.FillDatabase(context);
        }
    }
}

app.Run();

public partial class Program { }