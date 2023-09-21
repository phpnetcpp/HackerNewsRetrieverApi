using System.Text.Json;
using System.Text.Json.Serialization;

namespace HackerNewsRetrieverApi.Utils;

public class DateTimeOffsetJsonConverter : JsonConverter<DateTimeOffset>
{
    public override DateTimeOffset Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => DateTimeOffsetHelper.FromUnixTimeStamp(reader.GetInt64());

    public override void Write(Utf8JsonWriter writer, DateTimeOffset value, JsonSerializerOptions options)
        => throw new NotImplementedException();
}
