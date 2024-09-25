using KIP_Service.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace KIP_Service.DataAccess
{
    public static class RepositoryExtensions
    {
        public static IServiceCollection AddRepository(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration.GetConnectionString("Redis");
            });

            services.AddDbContext<KIP_ServiceDbContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString(nameof(KIP_ServiceDbContext)));
            });

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserStatisticRepository, UserStatisticRepository>();
            services.AddScoped<ICacheRepository, CacheRepository>();

            return services;
        }
    }
}
