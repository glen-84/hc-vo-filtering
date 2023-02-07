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

        // Bind runtime type.
        foreach (var idType in idTypes)
        {
            builder.BindRuntimeType(idType, typeof(LongType));
        }

        // Filter convention.
        builder.AddConvention<IFilterConvention>(new FilterConventionExtension(
            descriptor =>
            {
                foreach (var idType in idTypes)
                {
                    descriptor.BindRuntimeType(idType, typeof(LongOperationFilterInputType));
                }
            }));

        // Long to Id converter.
        builder.AddTypeConverter((
            Type source,
            Type target,
            [NotNullWhen(true)] out ChangeType? converter) =>
        {
            if (source == typeof(long) && target.IsAssignableTo(typeof(IId)))
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
                        new object[] { (long)input },
                        null);
                };

                return true;
            }

            converter = null;

            return false;
        });

        // Id to long converter.
        builder.AddTypeConverter((
            Type source,
            Type target,
            [NotNullWhen(true)] out ChangeType? converter) =>
        {
            if (source.IsAssignableTo(typeof(IId)) && target == typeof(long))
            {
                converter = (input) => ((IId?)input)?.Value;

                return true;
            }

            converter = null;

            return false;
        });

        return builder;
    }
}
