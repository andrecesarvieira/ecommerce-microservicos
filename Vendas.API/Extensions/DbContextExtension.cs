using Microsoft.EntityFrameworkCore;
using Vendas.API.Data;

namespace Vendas.API.Extensions
{
    public static class DbContextExtension
    {
        public static void AddCustomDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<VendasContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"),
                    sqlOptions => sqlOptions.EnableRetryOnFailure()
                )
            );
        }        
    }
}