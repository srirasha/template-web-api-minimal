using Application;
using Application._Common.Interfaces;
using Infrastructure.Persistence.Context;
using Infrastructure.Persistence.Interceptors;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddApplication();

            if (configuration.GetValue<bool>("UseInMemoryDatabase"))
            {
                services.AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDatabase("InMemoryDb"));
            }
            else
            {
                services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("MyDb"),
                                                            builder => builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
            }

            services.AddScoped<IApplicationDbContext, ApplicationDbContext>();
            services.AddScoped<ApplicationDbContextInitialiser>();
            services.AddScoped<AuditedEntitySaveChangesInterceptor>();

            services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        }
    }
}