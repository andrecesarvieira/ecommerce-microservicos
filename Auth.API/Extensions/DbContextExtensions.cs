using Auth.API.Data;
using Microsoft.EntityFrameworkCore;

namespace Auth.API.Extensions
{
    public static class DbContextExtensions
    {
        public static void AddCustomDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AuthContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"),
                    sqlOptions => sqlOptions.EnableRetryOnFailure()
                )
            );
        }
    }
}