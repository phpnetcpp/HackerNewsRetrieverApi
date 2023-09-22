using HackerNewsRetrieverApi.Controllers;
using HackerNewsRetrieverApi.Utils;
using Microsoft.AspNetCore.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient(Constants.HackerNewsApi.Name, (_, httpClient)
    => httpClient.BaseAddress = new Uri(builder.Configuration.GetValue<string>(Constants.HackerNewsApi.UrlSetting)));

builder.Services.AddMemoryCache();

builder.Services.AddRateLimiter(limiterOptions =>
{
    limiterOptions.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    limiterOptions.AddFixedWindowLimiter(
        Constants.FixedRateLimiter.Name,
        options =>
        {
            options.Window = TimeSpan.FromSeconds(10);
            options.PermitLimit = 5;
        });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseRateLimiter();

app.MapGet("api/v1/stories/best", BestStoriesEndpoints.Get)
    .RequireRateLimiting(Constants.FixedRateLimiter.Name);

app.Run();
