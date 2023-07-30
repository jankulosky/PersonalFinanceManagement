using API.Data;
using API.Data.Interfaces;
using API.Data.Repositories;
using API.Mappings;
using API.Services;
using API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions
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

            services.AddScoped<ITransactionRepository, TransactionRepository>();
            services.AddScoped<ITransactionService, TransactionService>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IAnalyticsService, AnalyticsService>();

            return services;
        }
    }
}
