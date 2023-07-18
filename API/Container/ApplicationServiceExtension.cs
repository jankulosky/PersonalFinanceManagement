using API.Data;
using API.Data.Repositories;
using API.Mappings;
using API.Services;
using Microsoft.EntityFrameworkCore;

namespace API.Container
{
    public static class ApplicationServiceExtension
    {
        public const string AngularUiOrigins = "_AngularUiOrigins";

        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(name: AngularUiOrigins, builder =>
                {
                    var corsDomain = config.GetValue<string>("Cors");
                    builder.WithOrigins("http://localhost:4200", "http://localhost", corsDomain)
                    .WithMethods("GET", "PUT", "POST", "DELETE", "HEAD", "OPTIONS")
                    .AllowAnyHeader()
                    .AllowCredentials();
                });
            });
            services.AddAutoMapper(typeof(AutoMapperProfile).Assembly);
            services.AddDbContext<DataContext>(options =>
            {
                options.UseNpgsql(config.GetConnectionString("DefaultConnection"));
            });
            services.AddScoped<TransactionService>();
            services.AddScoped<TransactionRepository>();

            return services;
        }
    }
}
