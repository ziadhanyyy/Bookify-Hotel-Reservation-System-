using Bookify.Core.DTOs;
using Bookify.Core.Entities;
using Bookify.Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signinManager;
      

        public AuthService(UserManager<User> userManager, SignInManager<User> signinManager)
        {
            _userManager = userManager;
            _signinManager = signinManager;
        }

        public async Task<IdentityResult> RegisterAsync(RegisterDto registerDto)
        {
            
            var user = new User
            {
                Country= registerDto.Country,
                Name = registerDto.Name,
                PhoneNumber = registerDto.PhoneNo,
                UserName = registerDto.Username,
                Email = registerDto.Email,
            };
            var createResult = await _userManager.CreateAsync(user, registerDto.Password);

            if (!createResult.Succeeded)
                return createResult;

            var addRoleResult = await _userManager.AddToRoleAsync(user, "User");
                
            return addRoleResult.Succeeded ? IdentityResult.Success : addRoleResult;

        }
        public async Task<SignInResult> LoginAsync(LoginDto loginDto)
        {
            var result = await _signinManager.PasswordSignInAsync(
                loginDto.Email,
                loginDto.Password,
                isPersistent: loginDto.RememberMe,
                lockoutOnFailure: false
                );
            return result;
        }

        public  async Task LogOutAsync()
        {
            await _signinManager.SignOutAsync();
        }

        
    }
}
