public static class TimeExtension
{
    /// <summary>
    /// Unix time, seconds passed since 1.1.1970. Used for serialization.
    /// </summary>
    public static long TotalSeconds(this GameContext gameContext) => gameContext.currentTime.Value;
}
