using System.Text.Json.Serialization;

namespace API.Enumerations
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum SortOrder
    {
        asc,
        desc,
    }
}
