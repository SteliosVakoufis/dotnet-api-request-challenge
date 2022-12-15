using ipstack_lib;
using ipstack_lib.interfaces;
using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.OData.Batch;
using Microsoft.AspNetCore.OData.NewtonsoftJson;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using web_api.Model;
using web_api.Services;
using web_api.utils;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        //var batchHandler = new DefaultODataBatchHandler();

        //batchHandler.MessageQuotas.MaxNestingDepth = 2;
        //batchHandler.MessageQuotas.MaxOperationsPerChangeset = 10;
        //batchHandler.MessageQuotas.MaxReceivedMessageSize = 100;

        // Add services to the container.
        //builder.Services.AddControllers()
        //    .AddOData(options => options.AddRouteComponents("odata", GetEdmModel(), batchHandler)
        //    .Select().OrderBy().Filter());

        builder.Services.AddControllers()
            .AddOData(options => options.Select().OrderBy().Filter());

        builder.Services.AddScoped<IIPInfoProvider, IPInfoProvider>();
        builder.Services.AddScoped<IPServiceImpl>();
        builder.Services.AddScoped<IIPService, CachedIPService>();
        builder.Services.AddScoped<WebApiUtils>();

        builder.Services.AddMemoryCache();

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

        app.UseODataBatching();
        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthorization();

        app.Run();
    }

    private static IEdmModel GetEdmModel()
    {
        var builder = new ODataConventionModelBuilder();
        builder.EntitySet<IPInfoEntity>("ipdetails");

        return builder.GetEdmModel();
    }
}