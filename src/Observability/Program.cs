using Observability.Helper;
using Observability.Models;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

// Configure OpenTelemetry
builder.Services.AddOpenTelemetry()
    .ConfigureResource(resource => resource.AddService(serviceName: builder.Environment.ApplicationName))
    .WithTracing(tracing =>
    {
        tracing.AddAspNetCoreInstrumentation()
                .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(serviceName: "PlanAObservability", serviceVersion: "1.0.0"))
                .AddHttpClientInstrumentation()
                //.AddConsoleExporter()
                .AddOtlpExporter(configure =>
                {
                    configure.Endpoint = new Uri("http://localhost:4317");
                    configure.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.Grpc;
                    configure.TimeoutMilliseconds = 10000;
                })
                .SetSampler(new AlwaysOnSampler()); 
    })
    .WithMetrics(metrics =>
    {
        metrics.AddAspNetCoreInstrumentation()
        .AddHttpClientInstrumentation()
        //.AddConsoleExporter()
        .AddOtlpExporter(configure =>
        {
            configure.Endpoint = new Uri("http://localhost:4317");
            configure.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.Grpc;
            configure.TimeoutMilliseconds = 10000;
        });
    });

    var loggerConfiguration = new LoggerConfiguration()
            .ReadFrom
            .Configuration(builder.Configuration)
            .Enrich.FromLogContext();
  
        Log.Logger = loggerConfiguration.CreateLogger();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAllOrigins");

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/api/observability/health", () => Results.Ok(new { status = "ok" }));

app.MapGet("/weatherforecast", async () =>
{
    Log.Logger.Information("Weather Forecast input");
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    var httpClient = new HttpClient();
    var result = await httpClient.GetAsync("https://www.google.com.br");

    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

var jsonOptions = new JsonSerializerOptions
{
    PropertyNameCaseInsensitive = true,
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
};

app.MapPost("/api/observability", async (ClientPayload payload, HttpRequest request, ILoggerFactory loggerFactory) =>
{
    var logger = Log.Logger;

    if (payload is null)
        return Results.BadRequest(new { error = "Malformed payload" });

    // Create correlation id (prefer client-generated if present)
    var correlationId = payload.Meta?.ClientGeneratedId ?? Guid.NewGuid().ToString("n");

    // Example: enrich with server info
    var receivedAt = DateTimeOffset.UtcNow;

    var geoPoint = (payload.Geolocation?.Latitude != null && payload.Geolocation?.Longitude != null)
        ? $"{payload.Geolocation.Latitude.Value},{payload.Geolocation.Longitude.Value}"
        : null;

    string? geoHash = (payload.Geolocation?.Latitude != null && payload.Geolocation?.Longitude != null)
        ? GeoHelper.GeoHashEncode(payload.Geolocation.Latitude, payload.Geolocation.Longitude)
        : null;
    logger = logger.ForContext("GeoLocationPoint", geoPoint);

    // Log with proper property name
    logger.Information(
        "Observability event {CorrelationId} at {ReceivedAt} UA={UserAgent} IP={PublicIp} URL={Url}, GEO={@GeoLocation}, GEOHASH={GeoHash}, GeoLocationPoint={GeoLocationPoint}, Screen={@ScreenData}, Connection={@Connection}, MetaData={@MetaData}, TimeZone={TZ}",
        correlationId,
        receivedAt,
        payload.Navigator?.UserAgent,
        payload.PublicIp,
        payload.Url?.Href,
        payload.Geolocation,
        geoHash,
        geoPoint,
        payload.Screen,
        payload.Connection,
        payload.Meta,
        payload.Timezone
    );

    return Results.Ok(new
    {
        ok = true,
        correlationId,
        receivedAt,
    });
})
.Accepts<ClientPayload>("application/json")
.Produces(StatusCodes.Status200OK)
.Produces(StatusCodes.Status400BadRequest);

app.Run();