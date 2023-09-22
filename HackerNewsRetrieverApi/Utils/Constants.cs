namespace HackerNewsRetrieverApi.Utils;

public static class Constants
{
    public static class HackerNewsApi
    {
        public const string Name = "hackernews";

        public const string UrlSetting = "HackerNewsApiUrl";

        public static class Urls
        {
            public const string Story = "v0/item/{0}.json";
            public const string BestStories = "v0/beststories.json";
        }
    }

    public static class ErrorMessages
    {
        public const string InvalidValue = "Invalid value for '{0}' parameter.";
        public const string StoriesRetrieving = "Error on stories retrieving.";
        public const string CacheSetting = "Error on cache setting.";
    }

    public static class FixedRateLimiter
    {
        public const string Name = "fixed";
    }

    public static class CacheBestStories
    {
        public const string Name = "beststories";
    }
}
