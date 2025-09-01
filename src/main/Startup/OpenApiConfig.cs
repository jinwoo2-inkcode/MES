namespace main.Startup;

using Microsoft.VisualBasic;
using Scalar.AspNetCore;
using System.Data.SqlClient;
using System.Text;

public static class OpenAPIConfig
{

    //connection string variable - did not decide where to put this
    public static string ConnectionString = "Data Source=FREEPDB1;Initial Catalog=MES;User id=dbo;Password=Duksu123!;";

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