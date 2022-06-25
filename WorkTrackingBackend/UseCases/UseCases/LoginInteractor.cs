using Entities;
using UseCases.Repositories;

namespace UseCases.UseCases;

public class LoginInteractor : IRequestHandler<LoginRequest, LoginResponse>
{
    private IUserRepository _userRepository;

    public LoginInteractor(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public LoginResponse Handle(LoginRequest request)
    {
        User? user = _userRepository.Users.SingleOrDefault(user => user.Login == request.login && user.Password.SequenceEqual(request.password));
        return new LoginResponse(user);
    }
}

public record LoginRequest(string login, byte[] password);
public record LoginResponse(User? user);