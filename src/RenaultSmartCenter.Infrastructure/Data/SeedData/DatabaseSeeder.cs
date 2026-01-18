using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RenaultSmartCenter.Domain.Entities;
using RenaultSmartCenter.Domain.Enums;

namespace RenaultSmartCenter.Infrastructure.Data.SeedData;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(ApplicationDbContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        // Seed Roles
        await SeedRolesAsync(roleManager);

        // Seed Branches
        await SeedBranchesAsync(context);

        // Seed Users
        await SeedUsersAsync(context, userManager);

        await context.SaveChangesAsync();
    }

    private static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
    {
        var roles = new[]
        {
            UserRole.SuperAdmin,
            UserRole.BranchManager,
            UserRole.Reception,
            UserRole.Mechanic,
            UserRole.Accountant,
            UserRole.InventoryManager
        };

        foreach (var roleName in roles)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }
    }

    private static async Task SeedBranchesAsync(ApplicationDbContext context)
    {
        if (await context.Branches.AnyAsync())
            return;

        var branches = new[]
        {
            new Branch
            {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                Name = "Main Branch - Downtown",
                Address = "123 Renault Street",
                City = "Beirut",
                Phone = "+961-1-1234567",
                Email = "downtown@renault-smart-center.com",
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "System"
            },
            new Branch
            {
                Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                Name = "Branch - North",
                Address = "456 Service Avenue",
                City = "Tripoli",
                Phone = "+961-6-7654321",
                Email = "north@renault-smart-center.com",
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "System"
            }
        };

        await context.Branches.AddRangeAsync(branches);
    }

    private static async Task SeedUsersAsync(ApplicationDbContext context, UserManager<IdentityUser> userManager)
    {
        if (await context.Users.AnyAsync())
            return;

        var mainBranchId = Guid.Parse("11111111-1111-1111-1111-111111111111");

        // Super Admin
        var superAdminIdentity = new IdentityUser
        {
            UserName = "admin@renault.com",
            Email = "admin@renault.com",
            EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(superAdminIdentity, "Admin@123");
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(superAdminIdentity, UserRole.SuperAdmin);

            var superAdminUser = new User
            {
                Id = Guid.NewGuid(),
                IdentityUserId = superAdminIdentity.Id,
                BranchId = mainBranchId,
                FirstName = "Super",
                LastName = "Admin",
                EmployeeNumber = "EMP001",
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "System"
            };

            await context.Users.AddAsync(superAdminUser);
        }

        // Branch Manager
        var managerIdentity = new IdentityUser
        {
            UserName = "manager@renault.com",
            Email = "manager@renault.com",
            EmailConfirmed = true
        };

        result = await userManager.CreateAsync(managerIdentity, "Manager@123");
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(managerIdentity, UserRole.BranchManager);

            var managerUser = new User
            {
                Id = Guid.NewGuid(),
                IdentityUserId = managerIdentity.Id,
                BranchId = mainBranchId,
                FirstName = "Branch",
                LastName = "Manager",
                EmployeeNumber = "EMP002",
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "System"
            };

            await context.Users.AddAsync(managerUser);
        }

        // Mechanic
        var mechanicIdentity = new IdentityUser
        {
            UserName = "mechanic@renault.com",
            Email = "mechanic@renault.com",
            EmailConfirmed = true
        };

        result = await userManager.CreateAsync(mechanicIdentity, "Mechanic@123");
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(mechanicIdentity, UserRole.Mechanic);

            var mechanicUser = new User
            {
                Id = Guid.NewGuid(),
                IdentityUserId = mechanicIdentity.Id,
                BranchId = mainBranchId,
                FirstName = "John",
                LastName = "Mechanic",
                EmployeeNumber = "EMP003",
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "System"
            };

            await context.Users.AddAsync(mechanicUser);
        }

        // Reception
        var receptionIdentity = new IdentityUser
        {
            UserName = "reception@renault.com",
            Email = "reception@renault.com",
            EmailConfirmed = true
        };

        result = await userManager.CreateAsync(receptionIdentity, "Reception@123");
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(receptionIdentity, UserRole.Reception);

            var receptionUser = new User
            {
                Id = Guid.NewGuid(),
                IdentityUserId = receptionIdentity.Id,
                BranchId = mainBranchId,
                FirstName = "Sarah",
                LastName = "Reception",
                EmployeeNumber = "EMP004",
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "System"
            };

            await context.Users.AddAsync(receptionUser);
        }
    }
}
