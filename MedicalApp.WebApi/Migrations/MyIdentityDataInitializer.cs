using MedicalApp.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalApp.WebApi.Migrations
{
    public class MyIdentityDataInitializer
    {
        public static void SeedData(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            SeedRoles(roleManager);
            SeedUsers(userManager);
        }

        public static void SeedUsers(UserManager<ApplicationUser> userManager)
        {
            if (userManager.FindByEmailAsync("admin@medicalapp.com").Result == null)
            {
                ApplicationUser user = new ApplicationUser
                {
                    EmailConfirmed = true,
                    UserName = "admin@medicalapp.com",
                    Email = "admin@medicalapp.com",
                    IsActive = true
                };
                IdentityResult result = userManager.CreateAsync(user, "Admin#123").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Administrator").Wait();
                }
            }
        }

        public static void SeedRoles(RoleManager<ApplicationRole> roleManager)
        {
            if (!roleManager.RoleExistsAsync("Administrator").Result)
            {
                ApplicationRole role = new ApplicationRole();
                role.Name = "Administrator";
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }

            if (!roleManager.RoleExistsAsync("Doctor").Result)
            {
                ApplicationRole role = new ApplicationRole();
                role.Name = "Doctor";
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }
        }
    }
}
