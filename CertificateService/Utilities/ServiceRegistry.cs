// Utilities/ServiceRegistry.cs
using System;
using CertificateService.AsyncDataServices;
using CertificateService.Data;
using CertificateService.Features;
using CertificateService.SyncDataServices.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace CertificateService.Utilities
{
    public static class ServiceRegistry
    {
        public static void ConfigureApplicationServices(IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
        {
            // Add Swagger for API documentation
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Certificate API", Version = "v1" });
            });

            // Configure database context based on the environment
            if (environment.IsDevelopment())
            {
                Console.WriteLine("--> Using InMemory database");
                services.AddDbContext<AppDBContext>(opt => opt.UseInMemoryDatabase("InMemory"));
            }
            else if (environment.IsProduction())
            {
                string connectionString = configuration.GetConnectionString("CertificateConn")
                                          ?? throw new Exception("Connection string 'CertificateConn' not found");

                Console.WriteLine("--> Using SQL Server database");
                services.AddDbContext<AppDBContext>(opt => opt.UseSqlServer(connectionString));
            }

            services.AddHttpClient<ISkillDataClient, HttpSkillDataClient>();

            services.AddScoped<ICertificateRepo, CertificateRepo>();
            services.AddSingleton<IMessageBusClient, MessageBusClient>();
            services.AddScoped<CertificatesService>();

            // services.AddControllers(); // Register MVC controllers
            services.AddGrpc();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            // Add other services as needed (e.g., controllers, GRPC, etc.)
            services.AddControllers();
        }
    }
}
