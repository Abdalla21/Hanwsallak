using Hanwsallak.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Hanwsallak.Infrastructure
{
    public static class InfrastructureDependencyInjections
    {
        public static void AddInfrastructureDependencyInjections(this IServiceCollection services, IConfiguration configuration)
        {
            // Write database (Primary)
            services.AddDbContext<ApplicationDBContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("WriteConnection") ?? configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly(typeof(ApplicationDBContext).Assembly.FullName)));

            // Read-only database (Replica)
            services.AddDbContext<ReadOnlyDBContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("ReadOnlyConnection") ?? configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly(typeof(ReadOnlyDBContext).Assembly.FullName)));
        }
    }
}
