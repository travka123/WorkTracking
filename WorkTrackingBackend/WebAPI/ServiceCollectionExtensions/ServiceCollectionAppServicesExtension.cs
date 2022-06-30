using EFData.Repositories;
using UseCases.Repositories;
using UseCases.UseCases.Interactors;

namespace WebAPI.ServiceCollectionExtensions;

public static class ServiceCollectionAppServicesExtension
{
    public static IServiceCollection AddAppServices(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ITaskRepository, TaskRepository>();
        services.AddScoped<IFirmRepository, FirmRepository>();
        
        services.AddScoped<UserGetTasksInteractor>();
        services.AddScoped<LoginInteractor>();
        services.AddScoped<AddTaskInteractor>();
        services.AddScoped<RemoveTasksInteractor>();
        services.AddScoped<UpdateTasksInteractor>();
        services.AddScoped<UserGetFirmsInteractor>();
        services.AddScoped<GetUnitsInteractor>();

        services.AddScoped<AdminAddUserInteractor>();
        services.AddScoped<AdminGetTasksInteractor>();
        services.AddScoped<AdminGetFirmsInteractor>();

        return services;
    }
}
