using AccessPointMap.Domain.Identities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccessPointMap.Application.Identities
{
    public static class Queries
    {
        public static async Task<IEnumerable<Identity>> GetAllIdentities(this IQueryable<Identity> identities)
        {
            return await identities
                .ToListAsync();
        }

        public static async Task<Identity> GetIdentityById(this IQueryable<Identity> identities, Guid id)
        {
            return await identities
                .SingleAsync(i => i.Id == id);
        }
    }
}
