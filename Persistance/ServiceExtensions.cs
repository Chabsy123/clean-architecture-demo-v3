using Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistance.Context;
using Persistance.IdentityModels;
using Persistance.Seeds;
using Persistance.SharedServices;

namespace Persistance
{
    public static class ServiceExtensions
    {
        public static void AddPersistance(this IServiceCollection services, IConfiguration configuration)
        {
            // register services
            services.AddDbContext<ApplicationDbContext>(option => option.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection")
                ));

            services.AddIdentityCore<ApplicationUser>()
               .AddRoles<ApplicationRole>()
               .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddScoped<IApplicationDbContext, ApplicationDbContext>();

            services.AddTransient<IAccountService, AccountService>();
            //seeds roles and users
            //DefaultRoles.SeedRolesAsync(services.BuildServiceProvider()).Wait();
            //DefaultUsers.SeedUsersAsync(services.BuildServiceProvider()).Wait();
        }
    }
}