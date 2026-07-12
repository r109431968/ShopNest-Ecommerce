using ShopNest.Application.DTOs.Auth;
using ShopNest.Application.Interfaces;
using ShopNest.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopNest.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJwtService _jwtService;

        public AuthService(IUnitOfWork unitOfWork, IJwtService jwtService)
        {
            _unitOfWork = unitOfWork;
            _jwtService = jwtService;
        }

        public async Task<AuthResponseDto> LoginAsync(LoginRequest request)
        {
            var existingUser = await _unitOfWork.Users.FindAsync(u => u.Email == request.Email);
            var user = existingUser.FirstOrDefault();
            if (user == null)
                throw new Exception("Invalid email or password.");

            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);
            if (!isPasswordValid)
                throw new Exception("Invalid email or password.");

            var accessToken = _jwtService.GenerateAccessToken(user);

            return new AuthResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = string.Empty,
                FullName = user.FullName,
                Email = user.Email,
                Role = user.Role
            };
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterRequest request)
        {
            var existingUser = await _unitOfWork.Users.FindAsync(u => u.Email == request.Email);

            if(existingUser.Any())
                throw new Exception("Email already Registered.");

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var user = new User
            {
                FullName = request.FullName,
                Email = request.Email,
                PasswordHash = hashedPassword,
                Role = "Customer",
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();

            var accessToken = _jwtService.GenerateAccessToken(user);

            return new AuthResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = string.Empty,
                FullName = user.FullName,
                Email = user.Email,
                Role = user.Role
            };
        }
    }
}
