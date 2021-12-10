using AutoMapper;
using AccessPointMap.Application.Abstraction;
using AccessPointMap.Domain.Core.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace AccessPointMap.API.Utility
{
    public static class RequestHandler
    {
        public static async Task<IActionResult> Command<TAggregateRoot>(IApplicationCommand<TAggregateRoot> command, Func<IApplicationCommand<TAggregateRoot>, Task> serviceHandler) where TAggregateRoot : AggregateRoot
        {
            await serviceHandler(command);

            return new OkResult();
        }

        public static async Task<IActionResult> MapQuery<TResponse, TResponseDto>(Task<TResponse> query, IMapper mapper)
        {
            var result = await query;

            if (result is null) return new NotFoundResult();

            var mappedResult = mapper.Map<TResponseDto>(result);

            return new OkObjectResult(mappedResult);
        }

        public static async Task<IActionResult> IntegrationServiceCommand<TRequest>(TRequest request, Func<TRequest, Task> serviceMethod)
        {
            await serviceMethod(request);

            return new OkResult();
        }
    }
}
