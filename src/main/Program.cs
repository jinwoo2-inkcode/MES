using main.Endpoints;
using main.Startup;
//using Scalar.AspNetCore;


var builder = WebApplication.CreateBuilder(args);
//url is http://localhost:5100/scalar/v1
// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.AddDependencies();

var app = builder.Build();

app.UseOpenApi();

app.UseHttpsRedirection();

app.AddRootEndPoints();
app.AddLoadEndpoints();
app.AddOrderEndpoints();
app.CheckConnection();

app.Run();


//this line is for testing if git push works

/*
// A skeleton of a C# program
using System;
namespace YourNamespace
{
    class YourClass
    {
    }

    struct YourStruct
    {
    }

    interface IYourInterface
    {
    }

    delegate int YourDelegate();

    enum YourEnum
    {
    }

    namespace YourNestedNamespace
    {
        struct YourStruct
        {
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello world!");
        }
    }
}
*/