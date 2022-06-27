using EFData;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using UseCases.UseCases;
using WebAPI.Data.Forms;
using WebAPI.Data.Views;
using WebAPI.ServiceCollectionExtensions;
using WebAPI.Utils;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors();

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

builder.Services.AddAppServices();

var app = builder.Build();

app.UseCors(builder => builder.WithOrigins("http://localhost:3000").AllowAnyMethod().AllowAnyHeader().AllowCredentials());

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapPost("/login", ([FromBody] LoginRequest request, [FromServices] LoginInteractor interactor) =>
{
    var user = interactor.Handle(request).user;
    if (user is null)
    {
        return Results.Unauthorized();
    }
    string token = AuthHelper.CreateToken(user.Id, user.GetType().Name.ToString());
    return Results.Ok(new { token = token, userId = user.Id, login = user.Login, role = user.GetType().Name });
});

app.MapPost("/users", [Authorize(Roles = "Administrator")] ([FromBody] UserAddForm request,
    ClaimsPrincipal claimsPrincipal, [FromServices] AdminAddUserInteractor interactor) =>
{
    int userId = AuthHelper.GetUserId(claimsPrincipal);
    var result = interactor.Handle(new AdminAddUserRequest(userId, request.login, request.password));
    return result.user;
});

app.MapGet("/user/tasks", [Authorize] (ClaimsPrincipal claimsPrincipal, [FromServices] UserGetTasksInteractor interactor) =>
{
    int userId = AuthHelper.GetUserId(claimsPrincipal);
    var result = interactor.Handle(userId);
    return result.Select(t => new TaskView(t)).ToList();
});

app.MapPost("/user/tasks", [Authorize] ([FromBody] TaskAddForm form, ClaimsPrincipal claimsPrincipal, 
    [FromServices] AddTaskInteractor addTask, UserGetFirmsInteractor getFirms) =>
{
    int userId = AuthHelper.GetUserId(claimsPrincipal);
    var result = addTask.Handle(new AddTaskRequest(getFirms, form.name, form.unitId, 
        form.quantity, form.description, form.reportingDate, form.firmId, userId));
    return result.task is null ? Results.BadRequest() : 
        Results.Created($"/user/tasks/{result.task.Id}", new TaskView(result.task));
});

app.MapDelete("/user/tasks", [Authorize] ([FromBody] IEnumerable<int> tasksIds, ClaimsPrincipal claimsPrincipal,
    [FromServices] RemoveTasksInteractor removeInteractor, UserGetTasksInteractor getInteractor) =>
{
    int userId = AuthHelper.GetUserId(claimsPrincipal);
    var result = removeInteractor.Handle(new RemoveTasksRequest(getInteractor, tasksIds, userId));
    return result ? Results.Ok() : Results.BadRequest();
});

app.MapPut("/user/tasks", [Authorize] ([FromBody] IEnumerable<TaskUpdate> form, ClaimsPrincipal claimsPrincipal,
    [FromServices] UpdateTasksInteractor updateInteractor, UserGetTasksInteractor getTasksInteractor,
    UserGetFirmsInteractor getFirmsInteractor) =>
{
    int userId = AuthHelper.GetUserId(claimsPrincipal);
    var result = updateInteractor.Handle(new UpdateTasksRequest(getTasksInteractor, getFirmsInteractor,
        form, userId));
    return result ? Results.Ok() : Results.BadRequest();
});

app.MapGet("/user/firms", [Authorize] (ClaimsPrincipal claimsPrincipal, 
    [FromServices] UserGetFirmsInteractor interactor) =>
{
    int userId = AuthHelper.GetUserId(claimsPrincipal);
    var result = interactor.Handle(userId);
    return result.Select(f => new ItemView(f.Id, f.Name)).ToList();
});

app.MapGet("/user/units", [Authorize] (ClaimsPrincipal claimsPrincipal,
    [FromServices] GetUnitsInteractor interactor) =>
{
    int userId = AuthHelper.GetUserId(claimsPrincipal);
    var result = interactor.Handle(userId);
    return result.Select(f => new ItemView(f.Id, f.Name));
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
        await DevelopmentHelper.FillDatabase(scope.ServiceProvider.GetService<AppDbContext>()!);
    }
}

app.Run();

public partial class Program { }