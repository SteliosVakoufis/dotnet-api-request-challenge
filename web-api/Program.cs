using ipstack_lib;
using ipstack_lib.interfaces;
using web_api.Model;
using web_api.Services;
using web_api.utils;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddScoped<IIPInfoProvider, IPInfoProvider>();
        builder.Services.AddScoped<IPServiceImpl>();
        builder.Services.AddScoped<IIPService, CachedIPService>();
        builder.Services.AddScoped<WebApiUtils>();

        builder.Services.AddMemoryCache();
        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddDbContext<DataContext>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}