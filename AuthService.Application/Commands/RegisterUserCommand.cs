using MediatR;
using AuthService.Application.DTOs;

namespace AuthService.Application.Commands;

public record RegisterUserCommand(string Username, string Email, string Password) : IRequest<UserDto>;
