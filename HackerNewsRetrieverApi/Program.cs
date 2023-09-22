using HackerNewsRetrieverApi.Controllers;
using HackerNewsRetrieverApi.Utils;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient(Constants.HackerNewsApi.Name, (_, httpClient)
    => httpClient.BaseAddress = new Uri(builder.Configuration.GetValue<string>(Constants.HackerNewsApi.UrlSetting)));

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.MapGet("api/v1/stories/best", BestStoriesEndpoints.Get);

app.Run();
