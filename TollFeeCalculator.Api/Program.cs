using TollFeeCalculatorApp.Api.Endpoints;
using TollFeeCalculatorApp.Api.Middlewares;
using TollFeeCalculatorApp.Core.Abstractions;
using TollFeeCalculatorApp.Core.Models;
using TollFeeCalculatorApp.Core.Rules;
using TollFeeCalculatorApp.Core.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<IVehicle, Car>();
builder.Services.AddTransient<IVehicle, Motorbike>();
builder.Services.AddScoped<TollCalculator>();
builder.Services.AddSingleton<ITollFreeDateProvider, TollFreeDateProvider>();
builder.Services.AddSingleton<ITollFeeRules, TollFeeRules>();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseExceptionHandler();
app.AddTollFeeEndpoints();
app.UseHttpsRedirection();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options => 
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });
}

app.Run();
