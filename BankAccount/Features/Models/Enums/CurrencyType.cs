using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace BankAccount.Features.Models.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum CurrencyType
    {
        [EnumMember(Value = "RUB")]
        Rub,

        [EnumMember(Value = "EUR")]
        Euro,

        [EnumMember(Value = "USD")]
        Usd
    }
}