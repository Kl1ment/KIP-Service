using KIP_Service.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace KIP_Service.Application
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IReportService, ReportService>();

            return services;
        }
    }
}
