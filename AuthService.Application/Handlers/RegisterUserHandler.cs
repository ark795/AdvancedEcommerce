using AuthService.Application.Commands;
using AuthService.Application.DTOs;
using AuthService.Domain.Entities;
using AuthService.Domain.Interfaces;
using AuthService.Infrastructure.Data;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Application.Handlers;

public class RegisterUserHandler : IRequestHandler<RegisterUserCommand, UserDto>
{
    private readonly AuthDbContext _db;
    private readonly ITokenService _tokenService;

    public RegisterUserHandler(AuthDbContext db, ITokenService tokenService)
    {
        _db = db;
        _tokenService = tokenService;
    }

    public async Task<UserDto> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        if (await _db.Users.AnyAsync(u => u.Email == request.Email, cancellationToken))
            throw new Exception("User already exists.");

        var hasher = new PasswordHasher<User>();
        var user = new User
        {
            Email = request.Email,
            Username = request.Username
        };

        user.PasswordHash = hasher.HashPassword(user, request.Password);
        user.RefreshToken = _tokenService.GenerateRefreshToken();
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

        _db.Users.Add(user);
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
