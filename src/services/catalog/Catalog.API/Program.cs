var builder = WebApplication.CreateBuilder(args);

// Add services to the container
var assembly = typeof(Program).Assembly;
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(assembly);
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
    config.AddOpenBehavior(typeof(LoggingBehavior<,>));
}); // mediator pattern midleware

builder.Services.AddValidatorsFromAssembly(assembly);

builder.Services.AddCarter(); // handles minimal api endpoints

var connString = builder.Configuration.GetConnectionString("Database")!;

builder.Services.AddMarten(config => config.Connection(connString))
    .UseLightweightSessions();

if (builder.Environment.IsDevelopment()) 
    builder.Services.InitializeMartenWith<CatalogInitialData>();

builder.Services.AddExceptionHandler<CustomExceptionHandler>();

builder.Services.AddHealthChecks().AddNpgSql(connString);

var app = builder.Build();

// Configure the HTTP request pipeline
app.MapCarter();
// Endpoints

app.UseExceptionHandler(options => { });

app.UseHealthChecks("/health",
    new HealthCheckOptions
    {
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });

app.Run();
