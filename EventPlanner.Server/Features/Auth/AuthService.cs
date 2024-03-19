using System;
using EventPlanner.Server.Data.Models;
using EventPlanner.Server.Data.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EventPlanner.Server.Features.Auth;

public interface IAuthService
{
    Task<GlobalResponse<AuthResponseDto>> Register(RegisterRequestDto request);
    Task<GlobalResponse<AuthResponseDto>> Login(LoginRequestDto request);
    Task<GlobalResponse<AuthResponseDto>> Logout();
}

public class AuthService : IAuthService
{
    private readonly RepositoryContext _repository;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly IJwtTokenManager _jwtTokenManager;

    public AuthService(RepositoryContext repository, IPasswordHasher<User> passwordHasher, IJwtTokenManager jwtTokenManager)
    {
        _repository = repository;
        _passwordHasher = passwordHasher;
        _jwtTokenManager = jwtTokenManager;
    }


    public Task<GlobalResponse<AuthResponseDto>> Login(LoginRequestDto request)
    {
        throw new NotImplementedException();
    }

    public Task<GlobalResponse<AuthResponseDto>> Logout()
    {
        throw new NotImplementedException();
    }

    public async Task<GlobalResponse<AuthResponseDto>> Register(RegisterRequestDto request)
    {
        var isEmailTaken = await _repository.Users.AnyAsync(u => u.Email == request.Email);
        if (isEmailTaken)
        {
            return GlobalResponse<AuthResponseDto>.Failure(["Email is already taken"]);
        }

        var validationResult = new AuthValidator().Validate(request);
        if (!validationResult.IsValid)
        {
            return GlobalResponse<AuthResponseDto>.Failure(validationResult.Errors.Select(e => e.ErrorMessage).ToArray());
        }

        var user = new User
        {
            FullName = request.Fullname,
            Email = request.Email,
        };

        user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);

        await _repository.Users.AddAsync(user);
        await _repository.SaveChangesAsync();

        var token = _jwtTokenManager.GenerateToken(user);

        return GlobalResponse<AuthResponseDto>.Success(new AuthResponseDto
        {
            Token = token,
            UserId = user.Id
        });


    }
}
