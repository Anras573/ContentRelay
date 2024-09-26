using ContentDistributionMetadataProducer.Publishers;
using ContentDistributionMetadataProducer.Readers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDaprClient();

builder.Services.AddScoped<ContentDistributionMetadataReader>();
builder.Services.AddScoped<ContentDistributionMetadataPublisher>();

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

app.MapGet("/produce", async (ContentDistributionMetadataReader reader) =>
    {
        var metadata = await reader.ReadMetadataAsync();
        
        return metadata.Match(
            some: Results.Ok,
            none: () => Results.NotFound());
    })
    .WithName("GET ProduceContentDistributionMetadata")
    .WithOpenApi();

app.MapPost("/produce",
        async (ContentDistributionMetadataReader reader, ContentDistributionMetadataPublisher publisher, CancellationToken ctx) =>
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
    .WithName("POST ProduceContentDistributionMetadata")
    .WithOpenApi();

app.Run();