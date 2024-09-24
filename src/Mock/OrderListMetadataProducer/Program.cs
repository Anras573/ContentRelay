using OrderListMetadataProducer.Publishers;
using OrderListMetadataProducer.Readers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDaprClient();

builder.Services.AddScoped<OrderListMetadataReader>();
builder.Services.AddScoped<OrderListMetadataPublisher>();

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

app.MapGet("/produce", async (OrderListMetadataReader reader) =>
    {
        var metadata = await reader.ReadMetadataAsync();
        
        return metadata.Match(
            some: Results.Ok,
            none: () => Results.NotFound());
    })
    .WithName("GET ProduceOrderListMetadata")
    .WithOpenApi();

app.MapPost("/produce",
        async (OrderListMetadataReader reader, OrderListMetadataPublisher publisher, CancellationToken ctx) =>
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
    .WithName("POST ProduceOrderListMetadata")
    .WithOpenApi();

app.Run();
