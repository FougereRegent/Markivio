using Markivio.Domain.Repositories;
using Markivio.Application.Dto;
using FluentResults;
using Markivio.Domain.Entities;
using Markivio.Application.Mapper;
using Markivio.Application.Errors;


namespace Markivio.Application.Users;

public interface IUserUseCase
{
    ValueTask<Result> CreateNewUserOnConnection(UserConnectionDto user, CancellationToken cancellationToken = default);
    ValueTask<Result<bool>> UserExist(UserConnectionDto user);
    ValueTask<Result<UserInformation>> GetUserInformationById(Guid id);
    IQueryable<UserInformation> GetUsers();
}

public class UserUseCase : IUserUseCase
{
    private readonly IUserRepository userRepository;

    public UserUseCase(IUserRepository userRepository)
    {
        this.userRepository = userRepository;
    }

    public async ValueTask<Result> CreateNewUserOnConnection(UserConnectionDto user, CancellationToken cancellationToken = default)
    {
        User? userFromToken = await userRepository.GetUserInfoByToken(user.Token, cancellationToken);
        if (userFromToken is null)
            return Result.Fail(new NotFoundError("User not found"));

        User? userFromDb = await userRepository.GetUserByAuthId(userFromToken.AuthId, cancellationToken);
        if (userFromDb is null)
            userRepository.Save(userFromToken);

        return Result.Ok();
    }

    public async ValueTask<Result<UserInformation>> GetUserInformationById(Guid id)
    {
        User? user = await userRepository.GetById(id);
        Result<UserInformation> result = Result.FailIf(user is null, "");
        if (result.IsFailed)
            return result;

        UserMapper userMapper = new UserMapper();
        UserInformation UserInformation = userMapper.UserToUserInformation(user!);

        return Result.Ok(UserInformation);
    }

    public ValueTask<Result<bool>> UserExist(UserConnectionDto user)
    {
        throw new NotImplementedException();
    }

    public IQueryable<UserInformation> GetUsers()
    {
        UserMapper mapper = new UserMapper();
        return userRepository.GetAll()
          .Select(user => mapper.UserToUserInformation(user));
    }
}
