using Markivio.Domain.Repositories;
using Markivio.Application.Dto;
using FluentResults;
using Markivio.Domain.Entities;
using Markivio.Application.Mapper;
using Markivio.Application.Errors;
using Markivio.Extensions.Identity;


namespace Markivio.Application.UseCases;

public interface IAuthUser
{
    UserInformation CurrentUser { get; set; }
}

public interface IUserUseCase : IAuthUser
{
    ValueTask<Result> CreateNewUserOnConnection(UserConnectionDto user, CancellationToken cancellationToken = default);
    ValueTask<Result<UserInformation>> Me(UserConnectionDto user, CancellationToken cancellationToken = default);
    ValueTask<Result<UserInformation>> GetUserInformationById(Guid id, CancellationToken cancellationToken = default);
    ValueTask<Result<UserInformation>> UpdateCurrentUser(UpdateUserInformation updateUser, CancellationToken cancellationToken = default);
    IQueryable<UserInformation> GetUsers();
}

public class UserUseCase : IUserUseCase
{
    private readonly IUserRepository userRepository;

    public UserInformation CurrentUser { get; set; }

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

    public async ValueTask<Result<UserInformation>> Me(UserConnectionDto user, CancellationToken cancellationToken = default)
    {
        UserMapper mapper = new UserMapper();
        JwtTokenInfo token = JwtTokenExtentions.ParseToken(user.Token);
        string authId = token.Subject;
        User? userDb = await userRepository.GetUserByAuthId(authId, cancellationToken);
        if (userDb is null)
            return Result.Fail(new NotFoundError("User not found"));

        return Result.Ok(mapper.UserToUserInformation(userDb));
    }

    public async ValueTask<Result<UserInformation>> GetUserInformationById(Guid id, CancellationToken cancellationToken = default)
    {
        User? user = await userRepository.GetById(id);
        Result<UserInformation> result = Result.FailIf(user is null, "");
        if (result.IsFailed)
            return result;

        UserMapper userMapper = new UserMapper();
        UserInformation UserInformation = userMapper.UserToUserInformation(user!);

        return Result.Ok(UserInformation);
    }

    public IQueryable<UserInformation> GetUsers()
    {
        UserMapper mapper = new UserMapper();
        return userRepository.GetAll()
          .Select(user => mapper.UserToUserInformation(user));
    }

    public async ValueTask<Result<UserInformation>> UpdateCurrentUser(UpdateUserInformation updateUser,
        CancellationToken cancellationToken = default)
    {
        User? user = await userRepository.GetById(CurrentUser.Id, cancellationToken);
        if (user is null)
            return Result.Fail(new NotFoundError("Cannot found"));

        user.FirstName = updateUser.FirstName;
        user.LastName = updateUser.LastName;

        User returnUser = userRepository.Update(user);
        UserMapper userMapper = new UserMapper();

        return userMapper.UserToUserInformation(returnUser);
    }
}
