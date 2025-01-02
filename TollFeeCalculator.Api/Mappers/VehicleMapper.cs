using TollFeeCalculatorApp.Core.Abstractions;
using TollFeeCalculatorApp.Core.Models;

namespace TollFeeCalculatorApp.Api.Mappers;

public static class VehicleMapper
{
    private static readonly Dictionary<VehicleType, Func<IVehicle>> VehicleMappings =
        new()
        {
            { VehicleType.Car, () => new Car() },
            { VehicleType.Motorbike, () => new Motorbike() }
        };

    public static IVehicle Map(string vehicleType)
    {
        if (string.IsNullOrWhiteSpace(vehicleType))
        {
            throw new ArgumentException("VehicleType cannot be null or empty.", nameof(vehicleType));
        }

        if (!Enum.TryParse(vehicleType, true, out VehicleType parsedVehicleType))
        {
            throw new ArgumentException($"Invalid VehicleType: {vehicleType}.", nameof(vehicleType));
        }

        return Map(parsedVehicleType);
    }

    private static IVehicle Map(VehicleType vehicleType)
    {
        if (!VehicleMappings.TryGetValue(vehicleType, out var vehicleFactory))
        {
            throw new ArgumentException($"Invalid VehicleType: {vehicleType}.", nameof(vehicleType));
        }

        return vehicleFactory();
    }
}