using AccessPointMap.Application.Authentication;
using AccessPointMap.Application.Core.Abstraction;
using AccessPointMap.Domain.Core.Events;
using AccessPointMap.Domain.Core.Models;
using Microsoft.Extensions.Logging;
using System;
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
        const string _jobBehaviourInformationMessage = "Scheduled job: {JobName} | Behaviour: {BehaviourDescription}";
        const string _jobBehaviourErrorMessage = "Scheduled job: {JobName} | Behaviour: {BehaviourDescription}\n    {Exception}";
        const string _domainEventInformationMessage = "Domain event envoked at: {AggregateEnvoker} | Event: {EventName} | EntityId : {EntityId}";
        const string _domainEventDebugMessage = "Domain event envoked at: {AggregateEnvoker} | Event: {EventName} | EntityId : {EntityId}\n    {SerializedEvent}";

        public static void LogDomainEvent<TCategoryName>(this ILogger<TCategoryName> logger, IEvent @event)
        {
            logger.LogDomainEvent(@event, @event.Id.ToString());
        }

        public static void LogDomainCreationEvent<TCategoryName>(this ILogger<TCategoryName> logger, IEventBase @event)
        {
            logger.LogDomainEvent(@event, string.Empty);
        }

        public static void LogDomainEvent<TCategoryName>(this ILogger<TCategoryName> logger, IEventBase @event, string entityId)
        {
            if (logger.IsEnabled(LogLevel.Debug) || logger.IsEnabled(LogLevel.Trace))
            {
                var serializedEvent = JsonSerializer.Serialize(@event, _jsonSerialzierOptions);

                logger.LogDebug(_domainEventDebugMessage,
                    typeof(TCategoryName).Name,
                    @event.GetType().Name,
                    entityId,
                    serializedEvent);
            }

            if (logger.IsEnabled(LogLevel.Information))
            {
                logger.LogInformation(_domainEventInformationMessage,
                    typeof(TCategoryName).Name,
                    @event.GetType().Name,
                    entityId);
            }
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

        public static void LogScheduledJobBehaviour<TCategoryName>(this ILogger<TCategoryName> logger, string behaviourDescription)
        {
            if (logger.IsEnabled(LogLevel.Information))
            {
                logger.LogInformation(_jobBehaviourInformationMessage,
                    typeof(TCategoryName).Name,
                    behaviourDescription);
            }
        }

        public static void LogScheduledJobBehaviour<TCategoryName>(this ILogger<TCategoryName> logger, string behaviourDescription, Exception exception)
        {
            if (logger.IsEnabled(LogLevel.Error))
            {
                logger.LogError(_jobBehaviourErrorMessage,
                    typeof(TCategoryName).Name,
                    behaviourDescription,
                    exception);
            }
        }
    }
}
