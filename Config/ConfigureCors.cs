namespace JokeApi.Configs;

/// <summary>
///    Service Extension Configuration
/// </summary>
public static class ServiceConfiguration
{
    /// <summary>
    ///    Adds cross-origin resource sharing  configurations to the specified service
    /// </summary>
    public static void ConfigureCors(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", builder =>
                builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
        });
    }
}