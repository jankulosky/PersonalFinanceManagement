using System.Text.Json.Serialization;

namespace API.Enumerations
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum TransactionKind
    {
        dep,
        wdw,
        pmt,
        fee,
        sal
    }
}
