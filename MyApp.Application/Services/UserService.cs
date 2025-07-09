using AutoMapper;
using MyApp.Application.DTOs;
using MyApp.Application.Interface;
using MyApp.Domain.Entities;
using MyApp.Domain.Interfaces;

namespace MyApp.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(
            IUserRepository userRepository,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<UserDto> AuthenticateAsync(LoginDto dto)
        {
            // پیدا کردن کاربر بر اساس نام کاربری
            var user = await _userRepository.GetByUsernameAsync(dto.Username);
            if (user == null)
            {
                return null; // کاربر یافت نشد
            }

            // اعتبارسنجی رمز عبور
            if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            {
                return null; // رمز عبور اشتباه
            }

            return _mapper.Map<UserDto>(user); // کاربر معتبر است
        }

        public async Task RegisterUserAsync(RegisterUserDto dto)
        {
            // اعتبارسنجی و هش کردن رمز عبور
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            var user = new User(dto.Username, dto.Email, hashedPassword);
            await _userRepository.AddAsync(user);
        }
    }
}
