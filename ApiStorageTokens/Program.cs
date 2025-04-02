using ApiStorageTokens.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddTransient<ServiceSasToken>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
}

app.MapOpenApi();

app.UseSwaggerUI(
    options =>
    {
        options.RoutePrefix = "";
        options.SwaggerEndpoint("/openapi/v1.json", "ApiStorageTokens v1");
    });


app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");


app.MapGet("/otro", () =>
{
    return "Hola mundo!";
})
.WithName("otro");

app.MapGet("/testing", () =>
{
    return "Testing!";
});

app.MapGet("/parametros/{dato}", (string dato) =>
{
    return $"Dato recibido: {dato}";
});

app.MapGet("/token/{curso}", (string curso, ServiceSasToken service) =>
{
    return service.GenerateToken(curso);
});

app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
