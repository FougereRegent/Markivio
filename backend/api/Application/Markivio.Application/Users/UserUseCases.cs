using Markivio.Domain.Repositories;
using Markivio.Application.Dto;
using FluentResults;


namespace Markivio.Application.Users;

public interface IUserUseCase
{
    Result CreateNewUserOnConnection(UserConnectionDto user);
    Result<bool> UserExist(UserConnectionDto user);
}

public class UserUseCase : IUserUseCase
{
    private readonly IUserRepository userRepository;

    public UserUseCase(IUserRepository userRepository)
    {
        this.userRepository = userRepository;
    }

    public Result CreateNewUserOnConnection(UserConnectionDto user)
    {
        throw new NotImplementedException();
    }

    public Result<bool> UserExist(UserConnectionDto user)
    {
        throw new NotImplementedException();
    }
}
