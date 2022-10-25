using AccessPointMap.Application.Abstraction;
using AccessPointMap.Application.Extensions;
using AccessPointMap.Domain.Identities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AccessPointMap.Application.Identities
{
    public static class Queries
    {
        public static async Task<Result<IEnumerable<Identity>>> GetAllIdentities(
            this IIdentityRepository identityRepository,
            CancellationToken cancellationToken = default)
        {
            return await identityRepository.Entities
                .AsNoTracking()
                .ToListAsync(cancellationToken)
                .ToResultObjectAsync();
        }

        public static async Task<Result<Identity>> GetIdentityById(
            this IIdentityRepository identityRepository,
            Guid id,
            CancellationToken cancellationToken = default)
        {
            return await identityRepository.Entities
                .AsNoTracking()
                .SingleOrDefaultAsync(i => i.Id == id, cancellationToken)
                .ToResultObjectAsync();
        }
    }
}
