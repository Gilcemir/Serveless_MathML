using System.Text.Json;
using Microsoft.AspNetCore.Cors.Infrastructure;
using ServerlessAPI;
using ServerlessAPI.Application;
using ServerlessAPI.Infrastructure;


var builder = WebApplication.CreateBuilder(args);

//Logger
builder.Logging
        .ClearProviders()
        .AddJsonConsole();
 
// Add services to the container.
builder.Services
        .AddControllers()
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        });

builder.Services.AddScoped<IWordProcessorService, WordProcessorService>();
builder.Services.AddScoped<IMathMLService, MathMLService>();
builder.Services.AddHttpClient();

// Add AWS Lambda support. When running the application as an AWS Serverless application, Kestrel is replaced
// with a Lambda function contained in the Amazon.Lambda.AspNetCoreServer package, which marshals the request into the ASP.NET Core hosting framework.
builder.Services.AddAWSLambdaHosting(LambdaEventSource.HttpApi);

var corsPolicy = new CorsPolicyBuilder()
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowAnyOrigin()
    .WithExposedHeaders("Content-Disposition")
    .Build();
builder.Services.AddCors(options =>
{
    options.AddPolicy(Consts.CorsPolicyName, corsPolicy);
});

var app = builder.Build();

app.UseHttpsRedirection();
app.UseCors(Consts.CorsPolicyName);
app.UseAuthorization();
app.MapControllers();

app.MapGet("/", () => "Welcome to running ASP.NET Core Minimal API on AWS Lambda");

app.Run();
