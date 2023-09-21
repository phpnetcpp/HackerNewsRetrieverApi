namespace HackerNewsRetrieverApi.Utils;

public static class DateTimeOffsetHelper
{
    public static DateTimeOffset FromUnixTimeStamp(long unixTimeStamp)
        => DateTimeOffset.UnixEpoch.AddSeconds(unixTimeStamp);
}
