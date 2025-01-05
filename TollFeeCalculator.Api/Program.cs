using TollFeeCalculator.Infrastructure;
using TollFeeCalculator.Infrastructure.Abstractions;
using TollFeeCalculatorApp.Api.Endpoints;
using TollFeeCalculatorApp.Api.Middlewares;
using TollFeeCalculatorApp.Core.Abstractions;
using TollFeeCalculatorApp.Core.Rules;
using TollFeeCalculatorApp.Core.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<TollCalculator>();
builder.Services.AddSingleton<ITollFreeDateProvider, TollFreeDateProvider>();
builder.Services.AddSingleton<ITollFeeRules, TollFeeRules>();
builder.Services.AddSingleton<IHolidayApi, MockHolidayApi>();

builder.Services.AddSingleton<IHolidayApi, MockHolidayApi>();
builder.Services.AddMemoryCache();
builder.Services.Decorate<IHolidayApi, CachedHolidayApi>();

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
    app.UseSwaggerUI();
}

app.Run();