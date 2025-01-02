using TollFeeCalculatorApp.Abstractions;
using TollFeeCalculatorApp.Models;
using TollFeeCalculatorApp.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<IVehicle, Car>();
builder.Services.AddTransient<IVehicle, Motorbike>();
builder.Services.AddScoped<TollCalculator>();

var app = builder.Build();

app.Run();