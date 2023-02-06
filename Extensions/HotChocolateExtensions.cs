using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using hc_vo_filtering.Interfaces;
using HotChocolate.Data.Filters;
using HotChocolate.Execution.Configuration;
using HotChocolate.Utilities;

namespace hc_vo_filtering.Extensions;

public static class HotChocolateExtensions
{
    /// <summary>
    /// Binds runtime types and adds type converters for strongly typed identifiers to Hot
    /// Chocolate.
    /// </summary>
    public static IRequestExecutorBuilder AddIdConverters(this IRequestExecutorBuilder builder)
    {
        // Find all value types assignable to the `IId` interface.
        var idTypes = Assembly.GetExecutingAssembly().GetTypes()
            .Where(type => type.IsValueType && type.IsAssignableTo(typeof(IId)));

        foreach (var idType in idTypes)
        {
            Console.WriteLine(idType.FullName);

            builder.BindRuntimeType(idType, typeof(StringType));

            builder.AddConvention<IFilterConvention>(new FilterConventionExtension(
                x => x.BindRuntimeType(idType, typeof(LongOperationFilterInputType))));
        }

        // Id to string converter.
        builder.AddTypeConverter((
            Type source,
            Type target,
            [NotNullWhen(true)] out ChangeType? converter) =>
        {
            if (source.IsAssignableTo(typeof(IId)) && target == typeof(string))
            {
                converter = (input) => input?.ToString();

                return true;
            }

            converter = null;

            return false;
        });

        // String to Id converter.
        builder.AddTypeConverter((
            Type source,
            Type target,
            [NotNullWhen(true)] out ChangeType? converter) =>
        {
            if (source == typeof(string) && target.IsAssignableTo(typeof(IId)))
            {
                converter = (input) =>
                {
                    if (input is null)
                    {
                        return null;
                    }

                    return Activator.CreateInstance(
                        target,
                        BindingFlags.Instance | BindingFlags.NonPublic,
                        null,
                        new object[] { long.Parse((string)input) },
                        null);
                };

                return true;
            }

            converter = null;

            return false;
        });

        return builder;
    }
}
