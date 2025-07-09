using MyApp.Application.DTOs;
using MyApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.Application.Interface
{
    public interface IUserService
    {
        Task RegisterUserAsync(RegisterUserDto dto);
        Task<UserDto> AuthenticateAsync(LoginDto dto); // متد جدید برای احراز هویت
    }
}
