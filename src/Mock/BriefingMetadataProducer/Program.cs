using BriefingMetadataProducer.Publishers;
using BriefingMetadataProducer.Readers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<BriefingMetadataReader>();

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

app.MapGet("/produce", async (BriefingMetadataReader reader) =>
    {
        var metadata = await reader.ReadMetadataAsync();
        
        return metadata.Match(
            some: Results.Ok,
            none: () => Results.NotFound());
    })
    .WithName("GET ProduceBriefingMetadata")
    .WithOpenApi();

app.MapPost("/produce",
    async (BriefingMetadataReader reader, BriefingMetadataPublisher publisher, CancellationToken ctx) =>
    {
        var metadata = await reader.ReadMetadataAsync();

        return metadata.MatchAsync(
            some: async data =>
            {
                await publisher.PublishMetadataAsync(data, ctx);
                return Results.Created("/produce", data);
            },
            none: () => Task.FromResult(Results.NotFound()));
    })
    .WithName("POST ProduceBriefingMetadata")
    .WithOpenApi();

app.Run();
