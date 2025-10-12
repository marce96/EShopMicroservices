//using Discount.Grpc;
using Discount.Grpc;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

// add services to the container

/////////////////////////////////////////////////////////////////////
// Application Services
/////////////////////////////////////////////////////////////////////
var assembly = typeof(Program).Assembly;
builder.Services.AddCarter();
builder.Services.AddMediatR(opt =>
    {
        opt.RegisterServicesFromAssembly(assembly);
        opt.AddOpenBehavior(typeof(ValidationBehavior<,>));
        opt.AddOpenBehavior(typeof(LoggingBehavior<,>));
    }
 );
/////////////////////////////////////////////////////////////////////
// Data Services
/////////////////////////////////////////////////////////////////////

builder.Services.AddValidatorsFromAssembly(assembly);

var postgres = builder.Configuration.GetConnectionString("Database")!;
builder.Services.AddMarten(opts =>
{
    opts.Connection(postgres);
    opts.Schema.For<ShoppingCart>().Identity(x => x.UserName);
    //opts.AutoCreateSchemaObjects = true;
}).UseLightweightSessions();

var redis = builder.Configuration.GetConnectionString("Redis")!;
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = redis;
    options.InstanceName = "basket-";
});

builder.Services.AddScoped<IBasketRepository, BasketRepository>();
builder.Services.TryDecorate<IBasketRepository, CachedBasketRepository>();

/////////////////////////////////////////////////////////////////////
// Grpc Services
/////////////////////////////////////////////////////////////////////

builder.Services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(options =>
{
    options.Address = new Uri(builder.Configuration["GrpcSettings:DiscountUrl"]!);
}).ConfigurePrimaryHttpMessageHandler(() =>
{
    var handler = new HttpClientHandler
    {
        ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
    };
    return handler;
});

/////////////////////////////////////////////////////////////////////
// Cross-cutting Services
/////////////////////////////////////////////////////////////////////
builder.Services.AddExceptionHandler<CustomExceptionHandler>();

builder.Services.AddHealthChecks().AddNpgSql(postgres).AddRedis(redis);

var app = builder.Build();

// configure HTTP request pipeline
app.MapCarter();
app.UseExceptionHandler(options => {});
app.UseHealthChecks("/health", new HealthCheckOptions()
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});
app.Run();
