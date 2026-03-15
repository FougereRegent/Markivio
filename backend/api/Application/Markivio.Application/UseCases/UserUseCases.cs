using Markivio.Domain.Repositories;
using Markivio.Application.Dto;
using FluentResults;
using Markivio.Domain.Entities;
using Markivio.Application.Mapper;
using Markivio.Application.Errors;
using Markivio.Domain.Auth;
using Markivio.Domain.Errors;
using Markivio.Domain.Exceptions;


namespace Markivio.Application.UseCases;

public interface IUserUseCase
{
    UserInformation CurrentUser { get; }
    ValueTask<Result> CreateNewUserOnConnection(UserConnectionDto user, CancellationToken cancellationToken = default);
    ValueTask<Result<UserInformation>> GetUserInformationById(Guid id, CancellationToken cancellationToken = default);
    ValueTask<Result<UserInformation>> UpdateCurrentUser(UpdateUserInformation updateUser, CancellationToken cancellationToken = default);
    IQueryable<UserInformation> GetUsers();
}

public class UserUseCase : IUserUseCase
{
    private readonly IUserRepository userRepository;
    private readonly IAuthUser authUser;

    public UserInformation CurrentUser
    {
        get
        {
            UserMapper mapper = new UserMapper();
            return mapper.UserToUserInformation(authUser.CurrentUser);
        }
    }

    public UserUseCase(IUserRepository userRepository, IAuthUser authUser)
    {
        this.userRepository = userRepository;
        this.authUser = authUser;
    }

    public async ValueTask<Result> CreateNewUserOnConnection(UserConnectionDto user, CancellationToken cancellationToken = default)
    {
        User? userFromToken = await authUser.GetUserInfoByToken(user.Token, cancellationToken);
        if (userFromToken is null)
            return Result.Fail(new NotFoundError("User not found"));

        User? userFromDb = await userRepository.GetUserByAuthId(userFromToken.AuthId, cancellationToken);
        if (userFromDb is null)
            userRepository.Save(userFromToken);
        else
            authUser.CurrentUser = userFromDb;

        return Result.Ok();
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

		UserMapper mapper = new UserMapper();
        try
        {
            mapper.ApplyUpdate(updateUser, user);
        }
        catch (DomainException ex)
        {
            return Result.Fail(MapDomainException(ex));
        }

        User returnUser = userRepository.Update(user);
        return mapper.UserToUserInformation(returnUser);
    }

    private static Error MapDomainException(DomainException ex) =>
        ex.ErrorCode switch
        {
            "EMPTY_FIRSTNAME" => new ShouldNotBeEmptyError("FirstName"),
            "EMPTY_LASTNAME" => new ShouldNotBeEmptyError("LastName"),
            "FORMAT_FIRSTNAME" => new FormatUnexpectedError("FirstName"),
            "FORMAT_LASTNAME" => new FormatUnexpectedError("LastName"),
            _ => new Error(ex.Message)
        };
}
