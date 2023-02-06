using hc_vo_filtering.Interfaces;
using Vogen;

namespace hc_vo_filtering.Types;

[ValueObject<long>(Conversions.None)]
public readonly partial struct BookId : IId, IComparable
{
    private static Validation Validate(long value)
        => value > 0 ? Validation.Ok : Validation.Invalid("Invalid.");

    public static bool operator <(BookId left, BookId right)
    {
        return left.CompareTo(right) < 0;
    }

    public static bool operator >(BookId left, BookId right)
    {
        return left.CompareTo(right) > 0;
    }

    public static bool operator <=(BookId left, BookId right)
    {
        return left.CompareTo(right) <= 0;
    }

    public static bool operator >=(BookId left, BookId right)
    {
        return left.CompareTo(right) >= 0;
    }
}
