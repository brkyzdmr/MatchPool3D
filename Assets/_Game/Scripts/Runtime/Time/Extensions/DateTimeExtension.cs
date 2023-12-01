using System;

public static class DateTimeExtension
{
    public static long ToEpochUnixTimestamp(this DateTime time)
    {
        return ((DateTimeOffset)time).ToUnixTimeSeconds();
    }
}
