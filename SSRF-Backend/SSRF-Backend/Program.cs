using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using SSRF_Backend.Extensions;
using Microsoft.Security.Application;
using System;
using SSRF_Backend.Services;
using SSRF_Backend.Models.Options;
using System.Runtime.Intrinsics.Arm;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddRouting();
builder.Services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
builder.Services.AddHttpClient();
builder.Services.AddScoped<IUrlHelper>(sp =>
{
    var actionContext = sp.GetRequiredService<IActionContextAccessor>().ActionContext;
    var factory = sp.GetRequiredService<IUrlHelperFactory>();
    return factory.GetUrlHelper(actionContext);
});

var CoreConfig = "_myAllowSpecificOrigins";

// Represents the Url state
var isUrlAllowed = true;

builder.Services.AddCors(options =>
{
    /*
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
    */

    // Add a specified CORE policy!
    options.AddPolicy(name: CoreConfig,
                          policy =>
                          {
                              policy.WithOrigins("https://localhost:4200");
                              policy.WithOrigins("http://localhost:4200");
                              policy.AllowAnyMethod();
                              policy.AllowAnyHeader();
                          });
});

// Get options
var urlOptions = builder.Configuration.GetSection("NotAllowedUrls").Get<List<string>>();

builder.Services.Configure<UrlOptions>(options =>
{
    options.NotAllowedUrls = urlOptions;
});

// Services
builder.Services.AddSingleton<ICommonService, CommonService>();

var app = builder.Build();


app.UseCors(CoreConfig);


app.Map("/internal", app =>
{
    app.Run(async context =>
    {
        // Check if url is allowed
        if (!isUrlAllowed)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
        }
        else
        {
            await context.Response.WriteAsync("Oh no, you found secret data");
        }
    });
});


app.MapPost("/weatherapi", async (HttpContext context) =>
{
    // Read the request body
    var requestBody = await context.Request.ReadFromJsonAsync<WeatherApiUrl>();

    isUrlAllowed = requestBody!.Url.CheckIfUrlIsValid(urlOptions!);

    // Check if Url is allowed
    if (!isUrlAllowed)
    {
        context.Response.StatusCode = StatusCodes.Status400BadRequest;
    }
    else
    {
        // Represents the safe url 
        var safeUrl = requestBody!.Url.RemoveDiacritics();

        // Retrieve IHttpClientFactory from the request services
        var httpClientFactory = context.RequestServices.GetRequiredService<IHttpClientFactory>();

        var httpClient = httpClientFactory.CreateClient();
        // Process the request body or perform desired actions
        var response = await httpClient.GetAsync(safeUrl);
        var content = await response.Content.ReadAsStringAsync();
        //context.Response.ContentType = "text/plain";

        // Return a response
        context.Response.StatusCode = StatusCodes.Status200OK;
        await context.Response.WriteAsync(content);
    }
});

int currTemperature = 18;
int currLight = 65;
int currRefrigeratorTemperature = 3;

app.MapGet("/temperature", () => currTemperature);
app.MapGet("/light", () => currLight);
app.MapGet("/refrigeratorTemperature", () => currRefrigeratorTemperature);

app.MapPost("/settemperature", async (HttpContext context) =>
{
    var requestBody = await context.Request.ReadFromJsonAsync<TemperaturePayload>();
    currTemperature = requestBody.Temperature;
});

app.MapPost("/setlight", async (HttpContext context) =>
{
    var requestBody = await context.Request.ReadFromJsonAsync<LightPayload>();
    currLight = requestBody.Light;
});

app.MapPost("/setrefrigeratortemperature", async (HttpContext context) =>
{
    var requestBody = await context.Request.ReadFromJsonAsync<TemperaturePayload>();
    currRefrigeratorTemperature = requestBody.Temperature;
});

app.UseSwagger();
app.UseSwaggerUI();

app.Run();


// Model class for the request body
public class WeatherApiUrl
{
    public string Url { get; set; }
}

public class TemperaturePayload
{
    public int Temperature { get; set; }
}

public class LightPayload
{
    public int Light { get; set; }
}