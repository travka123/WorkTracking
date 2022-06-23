using Entities;
using UseCases.Repositories;

namespace UseCases.UseCases;

public class AddUserInteractor : IRequestHandler<AddUserRequest, AddUserResponse>
{
    private readonly IUserRepository _userRepository;

    public AddUserInteractor(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public AddUserResponse Handle(AddUserRequest request)
    {
        User user = new User { Login = request.login, Password = request.password, AdministratorId = request.actorId };
        _userRepository.AddUser(user);
        return new AddUserResponse(user);
    }
}

public record AddUserRequest(int actorId, string login, byte[] password);
public record AddUserResponse(User user);