using hc_vo_filtering.Interfaces;
using Vogen;

namespace hc_vo_filtering.Types;

[ValueObject<long>(Conversions.None)]
public readonly partial struct BookId : IId
{
    private static Validation Validate(long value)
        => value > 0 ? Validation.Ok : Validation.Invalid("Invalid.");
}
