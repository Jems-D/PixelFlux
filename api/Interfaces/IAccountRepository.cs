using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Account;
using api.Models;

namespace api.Interfaces
{
    public interface IAccountRepository
    {
        Task<APIAccounResult<NewUserDto>> RegisterAsync(RegisterDto registerDto);
        Task<APIAccounResult<NewUserDto>> LoginAsync(LoginDto loginDto);
    }
}