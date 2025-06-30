using MediatR;
using AuthService.Application.DTOs;

namespace AuthService.Application.Commands;

public record RefreshTokenCommand(string RefreshToken, string Email) : IRequest<UserDto>;
