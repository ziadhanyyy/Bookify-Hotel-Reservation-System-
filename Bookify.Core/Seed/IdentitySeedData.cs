using Bookify.Core.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.Core.Seed
{
    public static class IdentitySeedData
    {
        public static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            string[] roles = new[] { "Admin", "User" };


            foreach (var r in roles)
            {
                if (!await roleManager.RoleExistsAsync(r))
                {
                    await roleManager.CreateAsync(new IdentityRole(r));
                }
            }

        }
        public static async Task SeedAdminAsync(UserManager<User> userManager)
        {
            string adminEmail = "admin@bookify.com";
            string adminPassword = "Admin@123";

            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                adminUser = new User
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                await userManager.CreateAsync(adminUser, adminPassword);
            }

            if (!await userManager.IsInRoleAsync(adminUser, "Admin"))
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }
    }
}
