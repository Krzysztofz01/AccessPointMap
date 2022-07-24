using AccessPointMap.Application.Abstraction;
using AccessPointMap.Domain.Core.Events;
using Microsoft.Extensions.Logging;
using System.Text;

namespace AccessPointMap.Application.Logging
{
    internal static class ILoggerExtensions
    {
        public static void LogDomainEvent(this ILogger logger, IEventBase @event)
        {
            //TODO: Change log-levels
            if (logger.IsEnabled(LogLevel.Debug) || logger.IsEnabled(LogLevel.Trace))
            {
                logger.LogDomainEventDebug(@event);
                return;
            }

            logger.LogDomainEventInformation(@event);
        }

        private static void LogDomainEventInformation(this ILogger logger, IEventBase @event)
        {
            const string message = "Domain event: {EventName} triggered.";
            logger.LogInformation(message, @event.GetType().Name);
        }

        private static void LogDomainEventDebug(this ILogger logger, IEventBase @event)
        {
            var values = new StringBuilder(string.Empty);        
            foreach (var prop in @event.GetType().GetProperties())
            {
                values.Append(prop.Name);
                values.Append('=');
                values.Append(prop.GetValue(@event, null));
                values.Append(';');
            }

            const string message = "Domain event: {EventName} triggered. Domain event properties: {EventPropertyDump}";
            logger.LogDebug(message, @event.GetType().Name, values.ToString());
        }

        public static void LogApplicationCommand(this ILogger logger, ICommand command)
        {
            //TODO: Change log-levels
            if (logger.IsEnabled(LogLevel.Debug) || logger.IsEnabled(LogLevel.Trace))
            {
                logger.LogApplicationCommandDebug(command);
                return;
            }

            logger.LogApplicationCommandInformation(command);
        }

        private static void LogApplicationCommandInformation(this ILogger logger, ICommand command)
        {
            const string message = "Application command: {CommandName} requested.";
            logger.LogInformation(message, command.GetType().Name);
        }

        private static void LogApplicationCommandDebug(this ILogger logger, ICommand command)
        {
            var values = new StringBuilder(string.Empty);
            foreach (var prop in command.GetType().GetProperties())
            {
                values.Append(prop.Name);
                values.Append('=');
                values.Append(prop.GetValue(command, null));
                values.Append(';');
            }

            const string message = "Application command: {CommandName} requested. Application command properties: {CommandPropertyDump}";
            logger.LogDebug(message, command.GetType().Name, values.ToString());
        }
    }
}
