using TollFeeCalculatorApp;
using TollFeeCalculatorApp.Abstractions;
using TollFeeCalculatorApp.Models;
using TollFeeCalculatorApp.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<IVehicle, Car>();
builder.Services.AddTransient<IVehicle, Motorbike>();
builder.Services.AddScoped<TollCalculator>();
builder.Services.AddSingleton<ITollFreeDateProvider, TollFreeDateProvider>();
builder.Services.AddSingleton<ITollFeeRules, TollFeeRules>();

var app = builder.Build();

app.Run();