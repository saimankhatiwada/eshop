using Application.Abstractions.Authentication;
using Application.Abstractions.Messaging;
using Application.Abstractions.Storage;
using Domain.Abstractions;
using Domain.Users;

namespace Application.Users.LoginUser;

internal sealed class LoginUserCommandHandler : ICommandHandler<LoginUserCommand, AccessTokenResponse>
{
    private readonly IJwtService _jwtService;
    private readonly IUserRepository _userRepository;
    private readonly IStorageService _storageService;

    public LoginUserCommandHandler(
        IJwtService jwtService,
        IUserRepository userRepository,
        IStorageService storageService)
    {
        _jwtService = jwtService;
        _userRepository = userRepository;
        _storageService = storageService;
    }
    public async Task<Result<AccessTokenResponse>> Handle(
        LoginUserCommand request,
        CancellationToken cancellationToken)
    {
        var result = await _jwtService.GetAccessTokenAsync(
            request.Email,
            request.Password,
            cancellationToken);

        if (result.IsFailure)
        {
            return Result.Failure<AccessTokenResponse>(UserErrors.InvalidCredentials);
        }

        var user = await _userRepository.GetByEmailAsync(new Email(request.Email), cancellationToken);

        if (user is null)
        {
            return Result.Failure<AccessTokenResponse>(UserErrors.NotFound);
        }

        return new AccessTokenResponse(result.Value, _storageService.GetPreSignedUrlAsync(user.ImageName.Value).GetAwaiter().GetResult().Value);
    }
}
