using TollFeeCalculatorApp.Abstractions;
using TollFeeCalculatorApp.Models;

namespace TollFeeCalculatorApp.Mappers;

public static class VehicleMapper
{
    private static readonly Dictionary<string, Func<IVehicle>> VehicleMappings =
        new(StringComparer.OrdinalIgnoreCase)
        {
            { "Car", () => new Car() },
            { "Motorbike", () => new Motorbike() }
        };

    public static IVehicle Map(string vehicleType)
    {
        if (string.IsNullOrWhiteSpace(vehicleType))
        {
            throw new ArgumentException("VehicleType cannot be null or empty.", nameof(vehicleType));
        }

        if (!VehicleMappings.TryGetValue(vehicleType, out var vehicleFactory))
        {
            throw new ArgumentException($"Invalid VehicleType: {vehicleType}.", nameof(vehicleType));
        }

        return vehicleFactory();
    }
}