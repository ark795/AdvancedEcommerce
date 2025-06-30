using AuthService.Application.Commands;
using AuthService.Application.DTOs;
using AuthService.Domain.Entities;
using AuthService.Domain.Interfaces;
using AuthService.Infrastructure.Data;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Application.Handlers;

public class LoginUserHandler : IRequestHandler<LoginUserCommand, UserDto>
{
    private readonly AuthDbContext _db;
    private readonly ITokenService _tokenService;

    public LoginUserHandler(AuthDbContext db, ITokenService tokenService)
    {
        _db = db;
        _tokenService = tokenService;
    }

    public async Task<UserDto> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);

        if (user is null)
            throw new Exception("Invalid credentials.");

        var hasher = new PasswordHasher<User>();
        var result = hasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);

        if (result == PasswordVerificationResult.Failed)
            throw new Exception("Invalid credentials.");

        // Refresh token جدید
        user.RefreshToken = _tokenService.GenerateRefreshToken();
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

        await _db.SaveChangesAsync(cancellationToken);

        return new UserDto
        {
            Email = user.Email,
            Username = user.Username,
            Role = user.Role,
            AccessToken = _tokenService.GenerateAccessToken(user),
            RefreshToken = user.RefreshToken
        };
    }
}
