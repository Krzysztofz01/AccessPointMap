using AccessPointMap.Application.Abstraction;
using AccessPointMap.Application.Authentication;
using AccessPointMap.Application.Integration.Core;
using AccessPointMap.Domain.Core.Events;
using Microsoft.Extensions.Logging;
using System.Text;

namespace AccessPointMap.Application.Logging
{
    public static class ILoggerExtensions
    {
        public static void LogDomainEvent(this ILogger logger, IEventBase @event)
        {
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

        public static void LogAuthenticationRequest(this ILogger logger, IAuthenticationRequest request, string ipAddress)
        {
            const string message = "Authentication request: {RequestName} received from address: {IpAddress}.";
            logger.LogInformation(message, request.GetType().Name, ipAddress);
        }

        public static void LogCommandController(this ILogger logger, ICommand command, string ipAddress)
        {
            if (logger.IsEnabled(LogLevel.Debug) || logger.IsEnabled(LogLevel.Trace))
            {
                logger.LogCommandControllerDebug(command, ipAddress);
                return;
            }

            logger.LogCommandControllerInformation(command, ipAddress);
        }

        public static void LogCommandController(this ILogger logger, IIntegrationCommand command, string ipAddress)
        {
            if (logger.IsEnabled(LogLevel.Debug) || logger.IsEnabled(LogLevel.Trace))
            {
                logger.LogCommandControllerDebug(command, ipAddress);
                return;
            }

            logger.LogCommandControllerInformation(command, ipAddress);
        }

        public static void LogCommandController(this ILogger logger, object request, string ipAddress)
        {
            if (logger.IsEnabled(LogLevel.Debug) || logger.IsEnabled(LogLevel.Trace))
            {
                logger.LogCommandControllerDebug(request, ipAddress);
                return;
            }

            logger.LogCommandControllerInformation(request, ipAddress);
        }

        private static void LogCommandControllerInformation(this ILogger logger, object request, string ipAddress)
        {
            const string message = "Command controller request: {Request} for service execution received from: {IpAddress}.";
            logger.LogInformation(message, request.GetType().Name, ipAddress);
        }

        private static void LogCommandControllerDebug(this ILogger logger, object request, string ipAddress)
        {
            var values = new StringBuilder(string.Empty);
            foreach (var prop in request.GetType().GetProperties())
            {
                values.Append(prop.Name);
                values.Append('=');
                values.Append(prop.GetValue(request, null));
                values.Append(';');
            }

            const string message = "Command controller request: {Request} for service execution received from: {IpAddress}. Request properties: {CommandPropertyDump}";
            logger.LogDebug(message, request.GetType().Name, ipAddress, values.ToString());
        }

        private static void LogCommandControllerInformation(this ILogger logger, ICommand command, string ipAddress)
        {
            const string message = "Command controller command: {CommandName} received from: {IpAddress}.";
            logger.LogInformation(message, command.GetType().Name, ipAddress);
        }

        private static void LogCommandControllerDebug(this ILogger logger, ICommand command, string ipAddress)
        {
            var values = new StringBuilder(string.Empty);
            foreach (var prop in command.GetType().GetProperties())
            {
                values.Append(prop.Name);
                values.Append('=');
                values.Append(prop.GetValue(command, null));
                values.Append(';');
            }

            const string message = "Command controller command: {CommandName} received from: {IpAddress}. Command properties: {CommandPropertyDump}";
            logger.LogDebug(message, command.GetType().Name, ipAddress, values.ToString());
        }

        private static void LogCommandControllerInformation(this ILogger logger, IIntegrationCommand command, string ipAddress)
        {
            const string message = "Command controller integration command: {CommandName} received from: {IpAddress}.";
            logger.LogInformation(message, command.GetType().Name, ipAddress);
        }

        private static void LogCommandControllerDebug(this ILogger logger, IIntegrationCommand command, string ipAddress)
        {
            var values = new StringBuilder(string.Empty);
            foreach (var prop in command.GetType().GetProperties())
            {
                values.Append(prop.Name);
                values.Append('=');
                values.Append(prop.GetValue(command, null));
                values.Append(';');
            }

            const string message = "Command controller integration command: {CommandName} received from: {IpAddress}. Command properties: {CommandPropertyDump}";
            logger.LogDebug(message, command.GetType().Name, ipAddress, values.ToString());
        }

        public static void LogQueryController(this ILogger logger, string path, string ipAddress)
        {
            logger.LogQueryControllerInformation(path ?? "Unknown", ipAddress);
        }

        private static void LogQueryControllerInformation(this ILogger logger, string path, string ipAddress)
        {
            const string message = "Query controller on path: {QueryPath} resolved for: {IpAddress}.";
            logger.LogInformation(message, path, ipAddress);
        }
    }
}
