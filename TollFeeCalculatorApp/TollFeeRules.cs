using TollFeeCalculatorApp.Abstractions;

namespace TollFeeCalculatorApp;

public sealed class TollFeeRules(ITollFreeDateProvider dateProvider) : ITollFeeRules
{
    private readonly ITollFreeDateProvider _dateProvider =
        dateProvider ?? throw new ArgumentNullException(nameof(dateProvider));

    private enum TollFreeVehicleType
    {
        Motorbike,
        Tractor,
        Emergency,
        Diplomat,
        Foreign,
        Military
    }

    private static readonly List<FeeScheduleEntry> FeeSchedule =
    [
        new FeeScheduleEntry(6, 0, 6, 29, 8),
        new FeeScheduleEntry(6, 30, 6, 59, 13),
        new FeeScheduleEntry(7, 0, 7, 59, 18),
        new FeeScheduleEntry(8, 0, 8, 29, 13),
        new FeeScheduleEntry(8, 30, 14, 59, 8),
        new FeeScheduleEntry(15, 0, 15, 29, 13),
        new FeeScheduleEntry(15, 30, 16, 59, 18),
        new FeeScheduleEntry(17, 0, 17, 59, 13),
        new FeeScheduleEntry(18, 0, 18, 29, 8)
    ];

    private static readonly HashSet<TollFreeVehicleType> TollFreeVehicles =
    [
        TollFreeVehicleType.Motorbike,
        TollFreeVehicleType.Tractor,
        TollFreeVehicleType.Emergency,
        TollFreeVehicleType.Diplomat,
        TollFreeVehicleType.Foreign,
        TollFreeVehicleType.Military
    ];


    public bool IsTollFreeVehicle(IVehicle? vehicle) =>
        vehicle is not null && Enum.TryParse(vehicle.GetVehicleType(), out TollFreeVehicleType type) &&
        TollFreeVehicles.Contains(type);


    public bool IsTollFreeDate(DateTime date) => _dateProvider.IsTollFreeDate(date);

    public int GetFeeForTime(DateTime date) =>
        (from entry in FeeSchedule where entry.IsWithinRange(date.TimeOfDay) select entry.Fee).FirstOrDefault();
}

public record FeeScheduleEntry(int StartHour, int StartMinute, int EndHour, int EndMinute, int Fee)
{
    public bool IsWithinRange(TimeSpan time)
    {
        var start = new TimeSpan(StartHour, StartMinute, 0);
        var end = new TimeSpan(EndHour, EndMinute, 59);
        return time >= start && time <= end;
    }
}