using Producers.Models;
using Producers.Publishers;
using Producers.Readers;
using Producers.Settings;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<MetadataReader>();
builder.Services.AddScoped<MetadataPublisher>();

builder.Services.AddDaprClient();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/produce", async (MetadataReader reader, MetadataPublisher publisher, ILogger<Program> logger, CancellationToken ctx) =>
    {
        var pathToTopic = new Dictionary<string, string>
        {
            { Settings.Metadata.Assets.Path, Settings.Topics.Asset.Name },
            { Settings.Metadata.Briefing.Path, Settings.Topics.Briefing.Name },
            { Settings.Metadata.ContentDistribution.Path, Settings.Topics.ContentDistribution.Name },
            { Settings.Metadata.OrderList.Path, Settings.Topics.OrderList.Name }
        };
        
        var pathToMetadataType = new Dictionary<string, Type>
        {
            { Settings.Metadata.Assets.Path, typeof(AssetMetadata) },
            { Settings.Metadata.Briefing.Path, typeof(BriefingMetadata) },
            { Settings.Metadata.ContentDistribution.Path, typeof(ContentDistributionMetadata) },
            { Settings.Metadata.OrderList.Path, typeof(OrderListMetadata) }
        };
        
        // Simulate events published out of order
        foreach (var (path, topic) in pathToTopic.OrderBy(_ => Guid.NewGuid()))
        {
            var metadataType = pathToMetadataType[path];

            switch (metadataType)
            {
                case not null when metadataType == typeof(AssetMetadata):
                    var assetMetadata = await reader.ReadAssetMetadataAsync(path);
                    await assetMetadata.SwitchAsync(
                        some: async data => await publisher.PublishMetadataAsync(data, "pubsub", topic, ctx),
                        none: () =>
                        {
                            logger.LogWarning("No metadata found at {Path}", path);
                            return Task.CompletedTask;
                        });
                    break;
                case not null when metadataType == typeof(BriefingMetadata):
                    var briefingMetadata = await reader.ReadBriefingMetadataAsync(path);
                    await briefingMetadata.SwitchAsync(
                        some: async data => await publisher.PublishMetadataAsync(data, "pubsub", topic, ctx),
                        none: () =>
                        {
                            logger.LogWarning("No metadata found at {Path}", path);
                            return Task.CompletedTask;
                        });
                    break;
                case not null when metadataType == typeof(ContentDistributionMetadata):
                    var contentDistributionMetadata = await reader.ReadContentDistributionMetadataAsync(path);
                    await contentDistributionMetadata.SwitchAsync(
                        some: async data => await publisher.PublishMetadataAsync(data, "pubsub", topic, ctx),
                        none: () =>
                        {
                            logger.LogWarning("No metadata found at {Path}", path);
                            return Task.CompletedTask;
                        });
                    break;
                case not null when metadataType == typeof(OrderListMetadata):
                    var orderListMetadata = await reader.ReadOrderListMetadataAsync(path);
                    await orderListMetadata.SwitchAsync(
                        some: async data => await publisher.PublishMetadataAsync(data, "pubsub", topic, ctx),
                        none: () =>
                        {
                            logger.LogWarning("No metadata found at {Path}", path);
                            return Task.CompletedTask;
                        });
                    break;
                default:
                    logger.LogWarning("Unknown metadata type {Type}", metadataType);
                    break;
            }
        }
        
        return Results.Ok();
    })
    .WithName("Produce")
    .WithOpenApi();

app.Run();
