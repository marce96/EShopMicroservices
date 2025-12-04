using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddReverseProxy()
        .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("fixed", opt =>
    {
        opt.PermitLimit = 4;
        opt.Window = TimeSpan.FromSeconds(10);
        //opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        //opt.QueueLimit = 2;
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseRateLimiter();
app.MapReverseProxy();

app.Run();
