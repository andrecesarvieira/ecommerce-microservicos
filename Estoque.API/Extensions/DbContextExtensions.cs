using Estoque.API.Data;
using Microsoft.EntityFrameworkCore;

namespace Estoque.API.Extensions
{
    public static class DbContextExtensions
    {
        public static void AddCustomDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<EstoqueContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        }
    }
}