using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;

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

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

var app = builder.Build();

app.UseCors();

app.Map("/internal", app =>
{
    app.Run(async context =>
    {
        await context.Response.WriteAsync("Oh no, you found secret data");
    });
});
/*
app.MapPost("/weatherapi", async (HttpContext context, IHttpClientFactory httpClientFactory) =>
{
    var httpClient = httpClientFactory.CreateClient();

    // Make an internal request to the /internal endpoint
    var response = await httpClient.GetAsync("http://localhost/internal");

    // Read the response from the internal endpoint
    var content = await response.Content.ReadAsStringAsync();

    // Return the response from the internal endpoint
    context.Response.ContentType = "text/plain";
    await context.Response.WriteAsync(content);
});
*/

app.MapPost("/weatherapi", async (HttpContext context) =>
{
    // Read the request body
    var requestBody = await context.Request.ReadFromJsonAsync<WeatherApiUrl>();
    // Retrieve IHttpClientFactory from the request services
    var httpClientFactory = context.RequestServices.GetRequiredService<IHttpClientFactory>();

    var httpClient = httpClientFactory.CreateClient();
    // Process the request body or perform desired actions
    var response = await httpClient.GetAsync(requestBody.Url);
    var content = await response.Content.ReadAsStringAsync();
    //context.Response.ContentType = "text/plain";

    // Return a response
    context.Response.StatusCode = StatusCodes.Status200OK;
    await context.Response.WriteAsync(content);
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