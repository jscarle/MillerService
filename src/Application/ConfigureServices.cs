using FluentValidation;
using MediatR;
using MillerDemo.Application.Reflection;
using MillerDemo.Application.Validation;

namespace MillerDemo.Application;

/// <summary>
/// Contains extensions methods for configuring application services.
/// </summary>
public static class ConfigureServices
{
    /// <summary>
    /// Adds application services to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The service collection.</returns>
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddMediatR();
        services.AddValidators();

        return services;
    }

    private static void AddMediatR(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            var assemblies = ReflectionExtensions.GetAssemblies();
            foreach (var assembly in assemblies)
                cfg.RegisterServicesFromAssembly(assembly);
        });
    }

    private static void AddValidators(this IServiceCollection services)
    {
        var validators = ReflectionExtensions.GetDefinedTypes<IValidator>().Where(x => !x.IsGenericType);
        foreach (var validator in validators)
        {
            var interfaceType = validator.GetGenericInterface(typeof(IValidator<>));
            services.AddSingleton(interfaceType, validator);
        }
    }
}