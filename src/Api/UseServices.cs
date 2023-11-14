namespace MillerDemo.Api;

/// <summary>
/// Contains extensions methods for using api services.
/// </summary>
public static class UseServices
{
    /// <summary>
    /// Use the API services.
    /// </summary>
    /// <param name="app">The web application.</param>
    public static void UseApiServices(this WebApplication app)
    {
        app.UseCors();
        app.UseOpenApi();
        app.UseHttpsRedirection();
    }

    private static void UseOpenApi(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.DefaultModelsExpandDepth(-1);
            options.EnableTryItOutByDefault();
        });
    }
}