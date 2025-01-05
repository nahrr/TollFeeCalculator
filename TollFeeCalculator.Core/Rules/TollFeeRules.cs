using TollFeeCalculatorApp.Core.Abstractions;
using TollFeeCalculatorApp.Core.Models.Enums;

namespace TollFeeCalculatorApp.Core.Rules;

public sealed class TollFeeRules(ITollFreeDateProvider dateProvider) : ITollFeeRules
{
    private readonly ITollFreeDateProvider _dateProvider =
        dateProvider ?? throw new ArgumentNullException(nameof(dateProvider));

    private static readonly List<FeeScheduleEntry> FeeSchedule =
    [
        new FeeScheduleEntry(StartHour: 6, StartMinute: 0, EndHour: 6, EndMinute: 29, Fee: 8),
        new FeeScheduleEntry(StartHour: 6, StartMinute: 30, EndHour: 6, EndMinute: 59, Fee: 13),
        new FeeScheduleEntry(StartHour: 7, StartMinute: 0, EndHour: 7, EndMinute: 59, Fee: 18),
        new FeeScheduleEntry(StartHour: 8, StartMinute: 0, EndHour: 8, EndMinute: 29, Fee: 13),
        new FeeScheduleEntry(StartHour: 8, StartMinute: 30, EndHour: 14, EndMinute: 59, Fee: 8),
        new FeeScheduleEntry(StartHour: 15, StartMinute: 0, EndHour: 15, EndMinute: 29, Fee: 13),
        new FeeScheduleEntry(StartHour: 15, StartMinute: 30, EndHour: 16, EndMinute: 59, Fee: 18),
        new FeeScheduleEntry(StartHour: 17, StartMinute: 0, EndHour: 17, EndMinute: 59, Fee: 13),
        new FeeScheduleEntry(StartHour: 18, StartMinute: 0, EndHour: 18, EndMinute: 29, Fee: 8)
    ];

    private static readonly HashSet<VehicleType> TollFreeVehicles =
    [
        VehicleType.Motorbike,
        VehicleType.Tractor,
        VehicleType.Emergency,
        VehicleType.Diplomat,
        VehicleType.Foreign,
        VehicleType.Military
    ];

    public bool IsTollFreeVehicle(IVehicle? vehicle) =>
        vehicle is not null &&
        TollFreeVehicles.Contains(vehicle.GetVehicleType());

    public Task<bool> IsTollFreeDate(DateTime date) => _dateProvider.IsTollFreeDate(date);

    public int GetFeeForTime(DateTime date) =>
        FeeSchedule
            .Where(entry => entry.IsWithinRange(date.TimeOfDay))
            .Select(entry => entry.Fee)
            .FirstOrDefault();
}

public sealed record FeeScheduleEntry(int StartHour, int StartMinute, int EndHour, int EndMinute, int Fee)
{
    public bool IsWithinRange(TimeSpan time)
    {
        var start = new TimeSpan(StartHour, StartMinute, 0);
        var end = new TimeSpan(EndHour, EndMinute, 59);
        return time >= start && time <= end;
    }
}