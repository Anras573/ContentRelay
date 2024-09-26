using AssetMetadataProducer.Publishers;
using AssetMetadataProducer.Readers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDaprClient();

builder.Services.AddScoped<AssetMetadataReader>();
builder.Services.AddScoped<AssetMetadataPublisher>();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCloudEvents();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/produce", async (AssetMetadataReader reader) =>
{
    var metadata = await reader.ReadMetadataAsync();
    
    return metadata.Match(
        some: Results.Ok,
        none: () => Results.NotFound());
})
    .WithName("GET ProduceAssetMetadata")
    .WithOpenApi();

app.MapPost("/produce", async (AssetMetadataReader reader, AssetMetadataPublisher publisher, CancellationToken ctx) =>
{
    var metadata = await reader.ReadMetadataAsync();

    return await metadata.MatchAsync(
        some: async data =>
        {
            await publisher.PublishMetadataAsync(data, ctx);
            return Results.Created("/produce", data);
        },
        none: () => Task.FromResult(Results.NotFound()));
})
    .WithName("POST ProduceAssetMetadata")
    .WithOpenApi();

app.Run();
