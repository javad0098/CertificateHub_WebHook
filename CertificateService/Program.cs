using CertificateService.Data;
using CertificateService.Utilities;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the containerssds
        ServiceRegistry.ConfigureApplicationServices(builder.Services, builder.Configuration, builder.Environment);

        // Build the application
        var app = builder.Build();

        // Configure the middleware pipeline
        ConfigureMiddleware(app, builder.Environment.IsProduction());
        EndpointMapper.MapAllEndpoints(app);

        // Start the application
        app.Run();
    }

    private static void ConfigureMiddleware(WebApplication app, bool isProd)
    {
        // Use Swagger only in development
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        // Enforce HTTPS redirection
        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseAuthorization();

        PrepDB.PrepPopulation(app, isProd);
    }
}
