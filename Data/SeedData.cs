using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace TaskManager.Data
{
    public static class SeedData
    {
        public static async Task EnsureAdminCreatedAsync(IServiceProvider services)
        {
            var userMgr = services.GetRequiredService<UserManager<IdentityUser>>();
            var roleMgr = services.GetRequiredService<RoleManager<IdentityRole>>();

            // 1. Adaugă roluri dacă nu există
            var roles = new[] { "Admin", "Manager", "User" };
            foreach (var role in roles)
            {
                if (!await roleMgr.RoleExistsAsync(role))
                    await roleMgr.CreateAsync(new IdentityRole(role));
            }
            //de sters daca fucntioneaxa mai sus
            //if (!await roleMgr.RoleExistsAsync("Admin"))
            //    await roleMgr.CreateAsync(new IdentityRole("Admin"));

            // 2. Adaugă userul admin dacă nu există
            const string adminUser = "adminuser";
            const string adminPass = "P@ssw0rd!";

            var user = await userMgr.FindByNameAsync(adminUser);
            if (user == null)
            {
                user = new IdentityUser { UserName = adminUser, Email = "admin@local" };
                var result = await userMgr.CreateAsync(user, adminPass);
                if (result.Succeeded)
                {
                    // 3. Adaugă userul în rolul Admin
                    await userMgr.AddToRoleAsync(user, "Admin");
                }
                else
                {
                    throw new Exception("Failed to create admin user: " +
                      string.Join(", ", result.Errors.Select(e => e.Description))); 
                }
            }


            //Atribuie rolul Admin userului 
            if (user != null && !(await userMgr.IsInRoleAsync(user, "Admin"))) 
            {
                await userMgr.AddToRoleAsync(user, "Admin");
            }

        }
    }
}
