// Utilities/ServiceRegistry.cs
using System;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using SkillService.Data;
using SkillService.EventProcessing;
using SkillService.Features;
using SkillService.SyncDataServices.Grpc;
using SkillsService.AsyncDataServices;

namespace SkillService.Utilities
{
    public static class ServiceRegistry
    {
        public static void ConfigureApplicationServices(IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
        {
            // Add Swagger for API documentation
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
             {
                 c.SwaggerDoc("v1", new OpenApiInfo
                 {
                     Title = "Certificate API",
                     Version = "v1",
                     Description = "API documentation for CertificateService",
                 });
             });

            services.AddDbContext<AppDbContext>(opt =>
            opt.UseInMemoryDatabase("InMemory"));
            services.AddScoped<ISkillRepo, SkillRepo>();
            services.AddScoped<ICertificateDataClient, CertificateDataClient>();

            // Register CertificateService and SkillService
            services.AddScoped<Features.CertificateService>();
            services.AddScoped<Skill_Service>();

            // Configure JSON options
            services.ConfigureHttpJsonOptions(options =>
            {
                // This ensures that enums are serialized/deserialized as strings
                options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

            services.AddControllers()
                     .AddJsonOptions(options =>
                     {
                         // This ensures that enums are serialized/deserialized as strings
                         options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                     });
            services.AddSingleton<IEventProcessor, EventProcessor>();
            services.AddHostedService<MessageBusSubscriber>();

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            // Add other services as needed (e.g., controllers, GRPC, etc.)
            services.AddControllers();
        }
    }
}
