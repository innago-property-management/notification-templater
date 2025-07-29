namespace Innago.Public.NotificationTemplater;

internal static class LogMessages
{
    public static void Error(this ILogger logger, Exception? exception)
    {
        LogErrorDefinition(logger, exception?.Message, exception);
    }
    
    private static Action<ILogger, string?, Exception?> LogErrorDefinition => LoggerMessage.Define<string?>(LogLevel.Error, new EventId(-1), "{Error}");
}