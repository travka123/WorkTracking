using Entities;
using UseCases.Repositories;

namespace UseCases.UseCases;

public class AdminAddUserInteractor : IRequestHandler<AdminAddUserRequest, AdminAddUserResponse>
{
    private readonly IUserRepository _userRepository;

    public AdminAddUserInteractor(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public AdminAddUserResponse Handle(AdminAddUserRequest request)
    {
        User user = new User { Login = request.login, Password = request.password, AdministratorId = request.actorId };
        _userRepository.AddUser(user);
        return new AdminAddUserResponse(user);
    }
}

public record AdminAddUserRequest(int actorId, string login, byte[] password);
public record AdminAddUserResponse(User user);