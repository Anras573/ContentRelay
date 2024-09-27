using ContentRelay.MAM.Web.Endpoints;

var builder = WebApplication.CreateBuilder(args);

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
#pragma warning enable ASP0014

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

app.Run();
