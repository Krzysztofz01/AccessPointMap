using AccessPointMap.Application.Authentication;
using AccessPointMap.Application.Core.Abstraction;
using AccessPointMap.Domain.Core.Events;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.Json;

namespace AccessPointMap.Application.Logging
{
    public static class ILoggerExtensions
    {
        private static readonly JsonSerializerOptions _jsonSerialzierOptions = new()
        {
            IgnoreReadOnlyFields = false,
            IgnoreReadOnlyProperties = false,
            IncludeFields = true,
            WriteIndented = false,
            PropertyNameCaseInsensitive = false,
            MaxDepth = 8
        };

        const string _commandControllerInformationMessage = "Command controller: {ControllerName} | Command: {CommandName} | Path: {CommandPath} | IdentityId: {IdentityId} | Host: {HostAddress}";
        const string _commandControllerDebugMessage = "Command controller: {ControllerName} | Command: {CommandName} | Path: {CommandPath} | IdentityId: {IdentityId} | Host: {HostAddress}\n    {SerializedCommand}";
        const string _queryControllerInformationMessage = "Query controller: {ControllerName} | Path: {QueryPath} | IdentityId: {IdentityId} | Host: {HostAddress}";
        const string _authenticationControllerInformationMessage = "Authentication controller: {ControllerName} | Request: {RequestName} | Path: {RequestPath} | IdentityId: {IdentityId} | Host: {HostAddress}";
        const string _authenticationControllerDebugMessage = "Authentication controller: {ControllerName} | Request: {RequestName} | Path: {RequestPath} | IdentityId: {IdentityId} | Host: {HostAddress}\n    {SerializedRequest}";

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

        public static void LogAuthenticationController<TCategoryName>(this ILogger<TCategoryName> logger, IAuthenticationRequest request, string path, string identityId, string hostAddress)
        {
            if (logger.IsEnabled(LogLevel.Debug) || logger.IsEnabled(LogLevel.Trace))
            {
                logger.LogDebug(_authenticationControllerDebugMessage,
                    typeof(TCategoryName).Name,
                    request.GetType().Name,
                    path,
                    identityId,
                    hostAddress,
                    "Request body hidden");
            }

            if (logger.IsEnabled(LogLevel.Information))
            {
                logger.LogInformation(_authenticationControllerInformationMessage,
                    typeof(TCategoryName).Name,
                    request.GetType().Name,
                    path,
                    identityId,
                    hostAddress);
            }
        }

        public static void LogCommandController<TCategoryName>(this ILogger<TCategoryName> logger, ICommand command, string path, string identityId, string hostAddress)
        {
            if (logger.IsEnabled(LogLevel.Debug) || logger.IsEnabled(LogLevel.Trace))
            {
                var serializedCommand = JsonSerializer.Serialize(command, _jsonSerialzierOptions);

                logger.LogDebug(_commandControllerDebugMessage,
                    typeof(TCategoryName).Name,
                    command.GetType().Name,
                    path,
                    identityId,
                    hostAddress,
                    serializedCommand);
            }

            if (logger.IsEnabled(LogLevel.Information))
            {
                logger.LogInformation(_commandControllerInformationMessage,
                    typeof(TCategoryName).Name,
                    command.GetType().Name,
                    path,
                    identityId,
                    hostAddress);
            }
        }

        public static void LogQueryController<TCategoryName>(this ILogger<TCategoryName> logger, string path, string identityId, string hostAddress)
        {
            if (logger.IsEnabled(LogLevel.Information))
            {
                logger.LogInformation(_queryControllerInformationMessage,
                    typeof(TCategoryName).Name,
                    path,
                    identityId,
                    hostAddress);
            }
        }
    }
}
