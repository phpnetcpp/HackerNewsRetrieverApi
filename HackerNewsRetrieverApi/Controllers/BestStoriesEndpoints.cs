using HackerNewsRetrieverApi.Models;
using HackerNewsRetrieverApi.Utils;
using System.Text.Json;

namespace HackerNewsRetrieverApi.Controllers;

public static class BestStoriesEndpoints
{
    public static async Task<IResult> Get(int number, IHttpClientFactory factory)
    {
        if (number <= 0)
        {
            return Results.BadRequest($"Invalid value for '{nameof(number)}' parameter.");
        }

        var httpClient = factory.CreateClient();

        try
        {
            var bestStoryIds = await GetBestStoryIdsAsync(httpClient);
            var stories = await GetStoriesAsync(httpClient, bestStoryIds.Take(number));

            return Results.Ok(stories.OrderByDescending(s => s.Score));
        }
        catch (Exception)
        {
            return Results.BadRequest("Error on data retrieving.");
        }
    }

    private static async Task<IEnumerable<int>> GetBestStoryIdsAsync(HttpClient httpClient)
    {
        var response = await httpClient.GetStringAsync("https://hacker-news.firebaseio.com/v0/beststories.json");

        return JsonSerializer.Deserialize<IEnumerable<int>>(response);
    }

    private static async Task<IEnumerable<HackerNewsStory>> GetStoriesAsync(HttpClient httpClient, IEnumerable<int> storyIds)
    {
        var tasks = storyIds.Select(async id =>
        {
            var response = await httpClient.GetStringAsync($"https://hacker-news.firebaseio.com/v0/item/{id}.json");
            var options = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            };
            options.Converters.Add(new DateTimeOffsetJsonConverter());

            return JsonSerializer.Deserialize<HackerNewsStory>(response, options);
        });

        return await Task.WhenAll(tasks);
    }
}
