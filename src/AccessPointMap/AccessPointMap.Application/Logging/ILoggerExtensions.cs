using AccessPointMap.Application.Authentication;
using AccessPointMap.Application.Core.Abstraction;
using AccessPointMap.Domain.Core.Events;
using AccessPointMap.Domain.Core.Models;
using Microsoft.Extensions.Logging;
using System;
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

        private const string _commandControllerInformationMessage = "Command controller: {ControllerName} | Command: {CommandName} | Path: {CommandPath} | IdentityId: {IdentityId} | Host: {HostAddress}";
        private const string _commandControllerDebugMessage = "Command controller: {ControllerName} | Command: {CommandName} | Path: {CommandPath} | IdentityId: {IdentityId} | Host: {HostAddress}\n    {SerializedCommand}";
        private const string _queryControllerInformationMessage = "Query controller: {ControllerName} | Path: {QueryPath} | IdentityId: {IdentityId} | Host: {HostAddress}";
        private const string _authenticationControllerInformationMessage = "Authentication controller: {ControllerName} | Request: {RequestName} | Path: {RequestPath} | IdentityId: {IdentityId} | Host: {HostAddress}";
        private const string _authenticationControllerDebugMessage = "Authentication controller: {ControllerName} | Request: {RequestName} | Path: {RequestPath} | IdentityId: {IdentityId} | Host: {HostAddress}\n    {SerializedRequest}";
        private const string _jobBehaviourInformationMessage = "Scheduled job: {JobName} | Behaviour: {BehaviourDescription}";
        private const string _jobBehaviourErrorMessage = "Scheduled job: {JobName} | Behaviour: {BehaviourDescription}\n    {Exception}";
        private const string _domainEventInformationMessage = "Domain event raised at: {AggregateEnvoker} | Event: {EventName} | EntityId : {EntityId}";
        private const string _domainEventDebugMessage = "Domain event raised at: {AggregateEnvoker} | Event: {EventName} | EntityId : {EntityId}\n    {SerializedEvent}";
        private const string _applicationCommandInformationMessage = "Application command envoked at: {CommandEnvoker} | Command: {CommandName} | IdentityId: {IdentityId}";
        private const string _applicationCommandDebugMessage = "Application command envoked at: {CommandEnvoker} | Command: {CommandName} | IdentityId: {IdentityId}\n  {SerializedCommand}";

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

        public static void LogApplicationCommand<TCategoryName>(this ILogger<TCategoryName> logger, ICommand command, string identityId)
        {
            if (logger.IsEnabled(LogLevel.Debug) || logger.IsEnabled(LogLevel.Trace))
            {
                var serializedCommand = JsonSerializer.Serialize(command, _jsonSerialzierOptions);

                logger.LogDebug(_applicationCommandDebugMessage,
                    typeof(TCategoryName).Name,
                    command.GetType().Name,
                    identityId,
                    serializedCommand);
            }

            if (logger.IsEnabled(LogLevel.Information))
            {
                logger.LogInformation(_applicationCommandInformationMessage,
                    typeof(TCategoryName).Name,
                    command.GetType().Name,
                    identityId);
            }
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
