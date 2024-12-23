using SkillService.Data;
using SkillService.Utilities;

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
        ConfigureMiddleware(app);
        EndpointMapper.MapAllEndpoints(app);
        app.MapControllers();

        // Start the application
        app.Run();
    }

    private static void ConfigureMiddleware(WebApplication app)
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
        PrepDb.PrepPopulation(app);
    }
}
