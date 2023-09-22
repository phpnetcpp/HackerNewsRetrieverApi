using HackerNewsRetrieverApi.Models;
using HackerNewsRetrieverApi.Utils;
using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;

namespace HackerNewsRetrieverApi.Controllers;

public static class BestStoriesEndpoints
{
    public static async Task<IResult> Get(int number, IHttpClientFactory factory, IMemoryCache cache)
    {
        if (number <= 0)
        {
            return Results.BadRequest(string.Format(Constants.ErrorMessages.InvalidValue, nameof(number)));
        }

        if (cache.TryGetValue($"beststories_{number}", out IEnumerable<HackerNewsStory> stories))
        {
            return Results.Ok(stories);
        }

        var httpClient = factory.CreateClient(Constants.HackerNewsApi.Name);

        try
        {
            var bestStoryIds = await GetBestStoryIdsAsync(httpClient);
            stories = await GetStoriesAsync(httpClient, bestStoryIds.Take(number));
        }
        catch (Exception)
        {
            return Results.BadRequest(Constants.ErrorMessages.StoriesRetrieving);
        }

        stories = stories.OrderByDescending(s => s.Score);

        var cacheEntryOptions = new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1)
        };
        try
        {
            cache.Set($"{Constants.CacheBestStories.Name}_{number}", stories, cacheEntryOptions);
        }
        catch (Exception)
        {
            return Results.BadRequest(Constants.ErrorMessages.CacheSetting);
        }

        return Results.Ok(stories);
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
