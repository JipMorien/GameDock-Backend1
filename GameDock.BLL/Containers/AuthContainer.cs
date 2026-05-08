using GameDock.Domain;
using GameDock.DTO.Dtos;
using GameDock.DTO.Interfaces;
using GameDock.BLL.Services;
using GameDock.Shared.Mappers;
using Microsoft.AspNetCore.Identity;

namespace GameDock.BLL.Containers
{
    public class AuthContainer
    {
        private readonly IGameDockUserDAL _userDal;
        private readonly IProfileDAL _profileDal;
        private readonly PasswordHasher<GameDockUserDto> _passwordHasher;
        private readonly JwtTokenService _jwtTokenService;
        
        public AuthContainer(
            IGameDockUserDAL userDal, 
            IProfileDAL profileDal, 
            JwtTokenService jwtTokenService)
        {
            _userDal = userDal;
            _profileDal = profileDal;
            _jwtTokenService = jwtTokenService;
            _passwordHasher = new PasswordHasher<GameDockUserDto>();
        }

        private void CheckAuth(RegisterRequestDto request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (string.IsNullOrWhiteSpace(request.UserName))
                throw new ArgumentException("Username cannot be empty");

            if (string.IsNullOrWhiteSpace(request.Email))
                throw new ArgumentException("Email cannot be empty");

            if (!request.Email.Contains("@"))
                throw new ArgumentException("Email is invalid");

            if (string.IsNullOrWhiteSpace(request.Password))
                throw new ArgumentException("Password cannot be empty");

            var existingUser = _userDal.GetUserByEmail(request.Email);

            if (existingUser != null)
                throw new ArgumentException("A user with this email already exists");
        }
        
        public AuthResponseDto Register(RegisterRequestDto request)
        {
            CheckAuth(request);

            var userDto = new GameDockUserDto
            {
                IsAdmin = false,
                UserName = request.UserName,
                Email = request.Email,
                CreatedAt = DateTime.UtcNow
            };

            userDto.PasswordHash = _passwordHasher.HashPassword(userDto, request.Password);

            var createdUser = _userDal.CreateUser(userDto);

            var profile = new Profile(
                0,
                createdUser.UserName,
                createdUser.GameDockUserId,
                string.Empty,
                1,
                DateTime.UtcNow
            );

            var profileDto = ProfileMapper.ToProfileDto(profile);
            _profileDal.CreateProfile(profileDto);

            return new AuthResponseDto
            {
                GameDockUserId = createdUser.GameDockUserId,
                UserName = createdUser.UserName,
                Email = createdUser.Email,
                IsAdmin = createdUser.IsAdmin,
                Token = _jwtTokenService.GenerateToken(createdUser)
            };
        }

        public AuthResponseDto Login(LoginRequestDto request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (string.IsNullOrWhiteSpace(request.Email))
                throw new ArgumentException("Email cannot be empty");

            if (string.IsNullOrWhiteSpace(request.Password))
                throw new ArgumentException("Password cannot be empty");

            var user = _userDal.GetUserByEmail(request.Email);

            if (user == null)
                throw new UnauthorizedAccessException("Invalid email or password");

            var result = _passwordHasher.VerifyHashedPassword(
                user,
                user.PasswordHash,
                request.Password
            );

            if (result == PasswordVerificationResult.Failed)
                throw new UnauthorizedAccessException("Invalid email or password");

            return new AuthResponseDto
            {
                GameDockUserId = user.GameDockUserId,
                UserName = user.UserName,
                Email = user.Email,
                IsAdmin = user.IsAdmin,
                Token = _jwtTokenService.GenerateToken(user)
            };
        }
    }
}