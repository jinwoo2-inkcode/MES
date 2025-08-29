namespace main.Startup;

//using Microsoft.VisualBasic;
using Scalar.AspNetCore;



public static class OpenAPIConfig
{

    public static void AddOpenApiServices(this IServiceCollection services)
    {
        services.AddOpenApi();
    }
    
    public static void UseOpenApi(this WebApplication app)
    {
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.MapScalarApiReference(options =>
            {
                options.Title = "The sample api"; //title of the screen
                options.Theme = ScalarTheme.Moon; //this changes the color theme of the screen
                options.Layout = ScalarLayout.Modern; //layout of screen
                options.HideClientButton = true; //hides client

            }
            ); //this is the scalar nuget add-on
        }
    }
    
}

