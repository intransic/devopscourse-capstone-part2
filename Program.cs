using (var context = new LogiTrackContext())
{
    // Add test inventory item if none exist
    if (!context.InventoryItems.Any())
    {
        context.InventoryItems.Add(new InventoryItem
        {
            Name = "Pallet Jack",
            Quantity = 12,
            Location = "Warehouse A"
        });

        context.SaveChanges();
    }

    // Retrieve and print inventory to confirm
    var items = context.InventoryItems.ToList();
    foreach (var item in items)
    {
        item.DisplayInfo(); // Should print: Item: Pallet Jack | Quantity: 12 | Location: Warehouse A
    }
}

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// controllers support
builder.Services.AddControllers();
// Endpoints API explorer for OpenAPI
builder.Services.AddEndpointsApiExplorer();

// register LogiTrackContext; OnConfiguring sets up SQLite
builder.Services.AddDbContext<LogiTrackContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // minimal API swagger endpoint
    app.MapOpenApi();
    
    // controller swagger middleware
    // Note as Swashbuckle is deprecated since .NET 9, we use the built-in OpenAPI support instead, as per:
    // https://stackoverflow.com/questions/79769908/getswagger-does-not-have-an-implementation
    //app.UseSwagger();
    //app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// map controller routes
app.MapControllers();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
