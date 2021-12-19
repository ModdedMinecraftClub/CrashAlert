using CliFx.Extensibility;
using Remora.Rest.Core;

namespace Mmcc.CrashAlert.Converters;

public class SnowflakeConverter : BindingConverter<Snowflake>
{
    public override Snowflake Convert(string? rawValue)
        => string.IsNullOrWhiteSpace(rawValue) ? default : new(ulong.Parse(rawValue));
}