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
            return Results.BadRequest(string.Format(Constants.ErrorCodes.InvalidValue, nameof(number)));
        }

        var httpClient = factory.CreateClient(Constants.HackerNewsApi.Name);

        try
        {
            var bestStoryIds = await GetBestStoryIdsAsync(httpClient);
            var stories = await GetStoriesAsync(httpClient, bestStoryIds.Take(number));

            return Results.Ok(stories.OrderByDescending(s => s.Score));
        }
        catch (Exception)
        {
            return Results.BadRequest(Constants.ErrorCodes.StoriesRetrieving);
        }
    }

    private static async Task<IEnumerable<int>> GetBestStoryIdsAsync(HttpClient httpClient)
    {
        var response = await httpClient.GetStringAsync(Constants.HackerNewsApi.Urls.BestStories);

        return JsonSerializer.Deserialize<IEnumerable<int>>(response);
    }

    private static async Task<IEnumerable<HackerNewsStory>> GetStoriesAsync(HttpClient httpClient, IEnumerable<int> storyIds)
    {
        var tasks = storyIds.Select(async id =>
        {
            var response = await httpClient.GetStringAsync(string.Format(Constants.HackerNewsApi.Urls.Story, id));
            var options = new JsonSerializerOptions(JsonSerializerDefaults.Web)
            {
                PropertyNamingPolicy = new StoryNamingPolicy()
            };
            options.Converters.Add(new DateTimeOffsetJsonConverter());

            return JsonSerializer.Deserialize<HackerNewsStory>(response, options);
        });

        return await Task.WhenAll(tasks);
    }
}
