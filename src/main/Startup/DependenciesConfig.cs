namespace main.Startup;

using Scalar.AspNetCore;

using System.Runtime.CompilerServices;

public static class DependenciesConfig
{
    public static void AddDependencies(this WebApplicationBuilder builder)
    {
        builder.Services.AddOpenApiServices(); //changing from .AddOpenApi to .AddOpenApiServices allows multiple api builds in Startup/OpenApiConfig.cs

    }
}