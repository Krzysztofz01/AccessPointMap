using AccessPointMap.Domain.Identities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccessPointMap.Application.Identities
{
    public static class Queries
    {
        public static async Task<IEnumerable<Identity>> GetAllIdentities(this IIdentityRepository identityRepository)
        {
            return await identityRepository.Entities
                .AsNoTracking()
                .ToListAsync();
        }

        public static async Task<Identity> GetIdentityById(this IIdentityRepository identityRepository, Guid id)
        {
            return await identityRepository.Entities
                .AsNoTracking()
                .SingleAsync(i => i.Id == id);
        }
    }
}
