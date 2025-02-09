using PositionsService.Services;
using PositionsService.Messaging;
using PositionsService.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);



// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IPositionService, PositionService>();
builder.Services.AddSingleton<IRabbitMQConsumer, RabbitMQConsumer>();

// Register services
builder.Services.AddDbContext<PositionsDbContext>(options =>
    options.UseInMemoryDatabase("PositionsDb"));

var app = builder.Build();

// Start RabbitMQ Consumer
var rabbitMQConsumer = app.Services.GetRequiredService<IRabbitMQConsumer>();
rabbitMQConsumer.StartListening();

// Configure middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();


using (var scope = app.Services.CreateScope())
{
    var service = scope.ServiceProvider.GetRequiredService<IPositionService>();
    await service.ImportPositionsFromCsvAsync("positions.csv");
}
app.Run();
