using HackerNewsRetrieverApi.Controllers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient("hackernews", (_, httpClient)
    => httpClient.BaseAddress = new Uri(builder.Configuration.GetValue<string>("HackerNewsApiUrl")));

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.MapGet("api/v1/stories/best", BestStoriesEndpoints.Get);

app.Run();
