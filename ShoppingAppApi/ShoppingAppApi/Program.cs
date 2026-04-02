using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ShoppingAppApi.Repositories;
using ShoppingAppApi.Services;
using ShoppingAppApi.Exceptions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AngularDev", policy =>
    {
        policy
            .WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddControllers();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IUserRoleService, UserRoleService>();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

var logger = app.Services
  .GetRequiredService<ILoggerFactory>()
  .CreateLogger("GlobalExceptionHandler");

app.UseExceptionHandler(builder =>
{
  builder.Run(async httpContext =>
  {
      Exception? exception = httpContext.Features
        .Get<IExceptionHandlerFeature>()?.Error;

    if (exception is null)
    {
      httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
      httpContext.Response.ContentType = "application/problem+json";
      await httpContext.Response.WriteAsJsonAsync(new ProblemDetails
      {
        Status = StatusCodes.Status500InternalServerError,
        Title = "Internal Server Error",
        Detail = "An unexpected error occurred."
      });
      return;
    }

    if (exception is ApiException apiException)
    {
      logger.LogWarning(
        exception,
        "API error {StatusCode}: {Title}",
        apiException.StatusCode,
        apiException.Title);

      var problemDetails = new ProblemDetails
      {
        Status = apiException.StatusCode,
        Title = apiException.Title,
        Detail = apiException.ClientMessage,
        Instance = httpContext.Request.Path
      };

      if (apiException.ErrorCode is not null)
      {
        problemDetails.Extensions["errorCode"] = apiException.ErrorCode;
      }

      httpContext.Response.StatusCode = apiException.StatusCode;
      httpContext.Response.ContentType = "application/problem+json";
      await httpContext.Response.WriteAsJsonAsync(problemDetails);
      return;
    }

    logger.LogError(exception, "Unhandled exception");

    httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
    httpContext.Response.ContentType = "application/problem+json";
    await httpContext.Response.WriteAsJsonAsync(new ProblemDetails
    {
      Status = StatusCodes.Status500InternalServerError,
      Title = "Internal Server Error",
      Detail = "An unexpected error occurred.",
      Instance = httpContext.Request.Path
    });
  });
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseCors("AngularDev");

app.UseAuthorization();

app.MapControllers();

app.Run();
