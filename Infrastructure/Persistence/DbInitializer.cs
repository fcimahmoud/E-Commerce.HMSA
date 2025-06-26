
using Persistence.Identity;

namespace Persistence
{
    public class DbInitializer (
        StoreContext storeContext, 
        UserManager<User> userManager, 
        RoleManager<IdentityRole> roleManager,
        StoreIdentityContext storeIdentityContext
        )
        : IDbInitializer
    {

        public async Task InitializeAsync()
        {
            try
            {
                // Create DataBase If It doesn't Exist & Applying Any Pending Migrations
                if (storeContext.Database.GetPendingMigrations().Any())
                    await storeContext.Database.MigrateAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task InitializeIdentityAsync()
        {
            // Create DataBase If It doesn't Exist & Applying Any Pending Migrations
            if (storeIdentityContext.Database.GetPendingMigrations().Any())
                await storeIdentityContext.Database.MigrateAsync();

            // Seed Default Roles
            if (!roleManager.Roles.Any())
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            // Seed Default Users
            if(!userManager.Users.Any())
            {
                var adminUser = new User
                {
                    FullName = "Admin",
                    Email = "hmsa.women@gmail.com",
                    UserName = "AdminUser",
                };

                await userManager.CreateAsync(adminUser, "Passw0rd");
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }
    }
}