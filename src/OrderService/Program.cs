using Microsoft.AspNetCore.Mvc;
using SharedKernel;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.MapGet("/health", () => Results.Ok(HealthEndpoints.GetHealthResponse()));
app.MapGet("/hello", () => Results.Ok("Hello World!!!"));

app.MapGet("/api/orders", () =>
{
    Task.Delay(500).Wait(); // Simulate some processing delay
    var orders = new List<OrderDto>
    {
        new("ORD-1001", "Pending", 125.00m),
        new("ORD-1002", "Shipped", 89.50m),
        new("ORD-1003", "Shipped", 89.50m),
        new("ORD-1004", "Delivered", 42.25m)
    };

    return Results.Ok(orders);
});

app.MapGet("/api/users", () => Results.Ok(UserEndpoints.GetSampleUsers()));

app.MapGet("/version", () =>
{
    var version = typeof(Program).Assembly
        .GetName()
        .Version?
        .ToString() ?? "unknown";

    return Results.Ok(new
    {
        version,
        environment = app.Environment.EnvironmentName,
        timestamp = DateTime.UtcNow
    });
});

app.Run();

public sealed record HealthResponse(string Status);

public static class HealthEndpoints
{
    public static HealthResponse GetHealthResponse() => new("ok");
}

public sealed record UserResponse(int Id, string Name, string Email, string Password);

public static class UserEndpoints
{
    public static IReadOnlyList<UserResponse> GetSampleUsers() =>
    [
        new(1, "Alice Johnson", "alice.johnson@example.com", "Password123!"),
        new(2, "Bob Smith", "bob.smith@example.com", "Password123!"),
        new(3, "Charlie Lee", "charlie.lee@example.com", "Password123!")
    ];
}
