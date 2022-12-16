using Hangfire;
using ipstack_lib;
using ipstack_lib.interfaces;
using web_api.BackgroundWorkers;
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

        builder.Services.AddTransient<IWorkerDb, WorkerDbImpl>();

        builder.Services.AddMemoryCache();
        builder.Services.AddControllers();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddDbContext<DataContext>();

        builder.Services.AddHangfire(conf => conf
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseSqlServerStorage(builder.Configuration.GetConnectionString("HangfireDb")));

        builder.Services.AddHangfireServer();

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

        app.UseHangfireDashboard();
        app.MapHangfireDashboard();

        app.Run();
    }
}