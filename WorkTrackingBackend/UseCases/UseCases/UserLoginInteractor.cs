using Entities;
using UseCases.Repositories;

namespace UseCases.UseCases;

public class UserLoginInteractor : IRequestHandler<UserLoginRequest, UserLoginResponse>
{
    private IUserRepository _userRepository;

    public UserLoginInteractor(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public UserLoginResponse Handle(UserLoginRequest request)
    {
        User? user = _userRepository.Users.SingleOrDefault(user => user.Login == request.login && user.Password.SequenceEqual(request.password));
        return new UserLoginResponse(user);
    }
}

public record UserLoginRequest(string login, byte[] password);
public record UserLoginResponse(User? user);