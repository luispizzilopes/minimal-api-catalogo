using Microsoft.EntityFrameworkCore;
using MinimalApiCatalogo.Context;

namespace MinimalApiCatalogo.AppServicesExtensions
{
    public static class ServiceCollectionExtensions
    {
        public static WebApplicationBuilder AddApiSwagger(this WebApplicationBuilder builder)
        {
            builder.Services.AddSwagger(); 
            return builder;
        }

        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            return services;
        }

        public static WebApplicationBuilder AddPersistence(this WebApplicationBuilder builder)
        {
            string? mySqlConnection = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<AppDbContext>(options => options.UseMySql(mySqlConnection,
                ServerVersion.AutoDetect(mySqlConnection)));

            return builder;
        }
    }
}
