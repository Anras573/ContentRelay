using ContentRelay.MAM.Application.Mappers;
using ContentRelay.MAM.Application.Repositories;
using ContentRelay.MAM.Application.Services;
using ContentRelay.MAM.Infrastructure.Repositories;
using ContentRelay.MAM.Web.Endpoints;
using ContentRelay.MAM.Web.Mappers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IAssetFileSizeCalculator, AssetFileSizeCalculator>();
builder.Services.AddScoped<MetadataService>();

builder.Services.AddSingleton<IAssetRepository, AssetRepository>();
builder.Services.AddSingleton<IBriefingRepository, BriefingRepository>();
builder.Services.AddSingleton<IContentDistributionRepository, ContentDistributionRepository>();
builder.Services.AddSingleton<IOrderListRepository, OrderListRepository>();

builder.Services.AddHttpClient();
builder.Services.AddDaprClient();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCloudEvents();
app.UseRouting();

#pragma warning disable ASP0014
app.UseEndpoints(endpoint => endpoint.MapSubscribeHandler());
#pragma warning restore ASP0014

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapAssetMetadataEndpoints();
app.MapBriefingMetadataEndpoints();
app.MapContentDistributionMetadataEndpoints();
app.MapOrderListMetadataEndpoints();
app.MapMetadataEndpoints();

app.Run();
