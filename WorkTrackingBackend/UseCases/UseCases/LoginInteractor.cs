using Entities;
using UseCases.Repositories;

namespace UseCases.UseCases;

public class LoginInteractor : IRequestHandler<LoginRequest, User>
{
    private IUserRepository _userRepository;

    public LoginInteractor(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public User Handle(LoginRequest request)
    {
        var user = _userRepository.Users
            .SingleOrDefault(user => user.Login == request.login &&
                user.Password.SequenceEqual(request.password));

        if (user is null) throw new UseCaseExeption("login failed");

        return user;
    }
}

public record LoginRequest(string login, byte[] password);