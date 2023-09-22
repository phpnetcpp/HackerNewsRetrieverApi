using HackerNewsRetrieverApi.Models;
using System.Text.Json;

namespace HackerNewsRetrieverApi.Utils;

public class StoryNamingPolicy : JsonNamingPolicy
{
    private readonly Dictionary<string, string> NameMapping = new()
    {
        [nameof(HackerNewsStory.Uri)] = "url",
        [nameof(HackerNewsStory.PostedBy)] = "by",
        [nameof(HackerNewsStory.CommentCount)] = "descendants",
    };

    public override string ConvertName(string name)
    {
        return NameMapping.GetValueOrDefault(name, name);
    }
}
