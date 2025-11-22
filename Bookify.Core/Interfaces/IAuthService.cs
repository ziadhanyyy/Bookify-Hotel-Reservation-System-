using Bookify.Core.DTOs;
using Bookify.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.Core.Interfaces
{
    public interface IAuthService
    {
        Task<IdentityResult> RegisterAsync (RegisterDto registerDto);
        Task<SignInResult> LoginAsync (LoginDto loginDto);
        Task LogOutAsync();

    }
}
