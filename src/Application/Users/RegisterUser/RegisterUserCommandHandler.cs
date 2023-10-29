using Application.Abstractions.Authentication;
using Application.Abstractions.Messaging;
using Application.Abstractions.Storage;
using Domain.Abstractions;
using Domain.Users;

namespace Application.Users.RegisterUser;

internal sealed class RegisterUserCommandHandler : ICommandHandler<RegisterUserCommand, Guid>
{
    private readonly IAuthenticationService _authenticationService;
    private readonly IUserRepository _userRepository;
    private readonly IStorageService _storageService;
    private readonly IUnitOfWork _unitOfWork;

    public RegisterUserCommandHandler(
        IAuthenticationService authenticationService,
        IUserRepository userRepository,
        IStorageService storageService,
        IUnitOfWork unitOfWork)
    
    {
        _authenticationService = authenticationService;
        _userRepository = userRepository;
        _storageService = storageService;
        _unitOfWork = unitOfWork;
    }
    public async Task<Result<Guid>> Handle(
        RegisterUserCommand request, 
        CancellationToken cancellationToken)
    {
        var user = User.Create(
            new FirstName(request.FirstName),
            new LastName(request.LastName),
            new Email(request.Email),
            new ImageName(request.ImageName));

        var identityId = await _authenticationService.RegisterAsync(
            user,
            request.Password,
            cancellationToken);

        user.SetIdentityId(identityId);

        _userRepository.Add(user);

        await _unitOfWork.SaveChangesAsync();

        await _storageService.UploadFileAsync(request.ImageName, request.FileContentType, request.FileStream);

        return user.Id.value;
    }
}
