using System.Reflection;

namespace MillerDemo.Application.Reflection;

/// <summary>
/// Contains extension methods used for reflection.
/// </summary>
public static class ReflectionExtensions
{
    /// <summary>
    /// Gets the application assemblies.
    /// </summary>
    /// <returns>An enumerable list of assemblies.</returns>
    public static IEnumerable<Assembly> GetAssemblies()
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies()
            .Where(x => x.FullName is not null && x.FullName.StartsWith("MillerDemo.", StringComparison.InvariantCulture));
        return assemblies;
    }

    /// <summary>
    /// Gets the types defined in the application.
    /// </summary>
    /// <typeparam name="TType"></typeparam>
    /// <returns>An enumerable list of types.</returns>
    public static IEnumerable<TypeInfo> GetDefinedTypes<TType>()
    {
        return GetDefinedTypes(typeof(TType));
    }

    /// <summary>
    /// Gets the generic interface of the specified generic type.
    /// </summary>
    /// <param name="type">The type that inherits from the generic type.</param>
    /// <param name="genericType">The generic type of the interface.</param>
    /// <returns>The generic interface.</returns>
    /// <exception cref="ArgumentException">Thrown when the generic type parameter is not a generic type.</exception>
    /// <exception cref="InvalidOperationException">Thrown when the generic type does not have a generic interface.</exception>
    public static Type GetGenericInterface(this Type type, Type genericType)
    {
        if (!genericType.IsGenericType)
            throw new ArgumentException($"The type {genericType.Name} is not a generic type.", nameof(genericType));

        return type.GetGenericInterfaceOrDefault(genericType) ??
               throw new InvalidOperationException($"No interface of type {type.GenericName} was found.");
    }

    private static IEnumerable<TypeInfo> GetDefinedTypes()
    {
        var definedTypes = GetAssemblies()
            .SelectMany(x => x.DefinedTypes)
            .Where(x => x.FullName is not null && x.FullName.StartsWith("MillerDemo.", StringComparison.InvariantCulture) && x is { IsInterface: false, IsAbstract: false });
        return definedTypes;
    }

    private static IEnumerable<TypeInfo> GetDefinedTypes(Type type)
    {
        if (type.IsInterface)
        {
            if (type.IsGenericType)
            {
                return GetDefinedTypes()
                    .Where(t => Array.Exists(t.GetInterfaces(), y => y.IsGenericType && y.GetGenericTypeDefinition() == type));
            }

            return GetDefinedTypes()
                .Where(t => t.ImplementedInterfaces.Contains(type));
        }

        return GetDefinedTypes()
            .Where(t => t.InheritsFrom(type));
    }

    private static bool InheritsFrom(this Type type, Type baseType)
    {
        while (true)
        {
            if (type.BaseType is null)
                return false;

            type = type.BaseType.IsGenericType ? type.BaseType.GetGenericTypeDefinition() : type.BaseType;

            if (type == baseType)
                return true;
        }
    }

    private static Type? GetGenericInterfaceOrDefault(this Type type, Type genericType)
    {
        if (!genericType.IsGenericType)
            throw new ArgumentException($"The type {genericType.Name} is not a generic type.", nameof(genericType));

        return Array.Find(type.GetInterfaces(), x => x == genericType || x.IsGenericType && x.GetGenericTypeDefinition() == genericType);
    }

    private static string GenericName(this Type genericType)
    {
        if (!genericType.IsGenericType)
            throw new ArgumentException("The type is not a generic type.", nameof(genericType));

        var typeName = genericType.Name.Split('`')[0];
        var arguments = string.Join(", ", genericType.GetGenericArguments()
            .Select(x => x.Name));

        return $"{typeName}<{arguments}>";
    }
}