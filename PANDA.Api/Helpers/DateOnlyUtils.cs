namespace PANDA.Api.Helpers;

public static class DateOnlyUtils
{
    public static DateOnly Today => DateOnly.FromDateTime(DateTime.Today);

    public static DateOnly From(DateTime dateTime) =>
        DateOnly.FromDateTime(dateTime);

    public static DateTime ToDateTime(this DateOnly dateOnly) =>
        dateOnly.ToDateTime(TimeOnly.MinValue);
}