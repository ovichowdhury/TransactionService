using TransactionService.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Register RandomIDService as Singleton, Scoped, and Transient
builder.Services.AddSingleton<IRandomIDService, RandomIDService>();
builder.Services.AddScoped<RandomIDService>();
builder.Services.AddTransient<TransientRandomIDService>();

// Build the app.
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// Minimal API endpoint to demonstrate service lifetimes
app.MapGet("/random-ids", (
    IRandomIDService singletonService1,
    RandomIDService scopedService1,
    RandomIDService scopedService2,
    TransientRandomIDService transientService1,
    TransientRandomIDService transientService2
) =>
{
    return Results.Ok(new
    {
        SingletonID1 = singletonService1.RandomID,
        ScopedID1 = scopedService1.RandomID,
        ScopedID2 = scopedService2.RandomID,
        TransientID1 = transientService1.RandomID,
        TransientID2 = transientService2.RandomID,
    });
});

app.Run();

