using AccessPointMap.Domain.AccessPoints;
using AccessPointMap.Domain.Core.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccessPointMap.Application.AccessPoints
{
    public static class Queries
    {
        public static async Task<IEnumerable<AccessPoint>> GetAllAccessPoints(this IQueryable<AccessPoint> accessPoints)
        {
            return await accessPoints
                .Where(a => a.DisplayStatus.Value)
                .ToListAsync();
        }

        public static async Task<IEnumerable<AccessPoint>> GetAllAccessPointsAdministration(this IQueryable<AccessPoint> accessPoints)
        {
            return await accessPoints
                .ToListAsync();
        }

        public static async Task<AccessPoint> GetAccessPointById(this IQueryable<AccessPoint> accessPoints, Guid id)
        {
            return await accessPoints
                .Include(a => a.Stamps)
                .Where(a => a.DisplayStatus.Value)
                .SingleAsync(a => a.Id == id);
        }

        public static async Task<AccessPoint> GetAccessPointByIdAdministration(this IQueryable<AccessPoint> accessPoints, Guid id)
        {
            return await accessPoints
                .Include(a => a.Stamps)
                .SingleAsync(a => a.Id == id);
        }

        public static async Task<IEnumerable<AccessPoint>> SearchByKeyword(this IQueryable<AccessPoint> accessPoints, string keyword)
        {
            string kw = keyword.Trim().ToLower();

            return await accessPoints
                .Where(a =>
                    a.Ssid.Value.ToLower().Contains(kw) ||
                    a.DeviceType.Value.ToLower().Contains(kw) ||
                    a.Security.RawSecurityPayload.ToLower().Contains(kw))
                .ToListAsync();
        }

        public static async Task<IEnumerable<AccessPoint>> GetAccessPointsWithGreatestSignalRange(this IQueryable<AccessPoint> accessPoints, int limit)
        {
            return await accessPoints
                .Where(x => x.DisplayStatus.Value)
                .OrderByDescending(a => a.Positioning.SignalArea)
                .Take(limit)
                .ToListAsync();
        }

        public static async Task<IEnumerable<object>> GetMostCommonUsedFrequency(this IQueryable<AccessPoint> accessPoints, int limit)
        {
            return await accessPoints
                .Where(a => a.DisplayStatus.Value)
                .Where(a => a.Frequency != default)
                .GroupBy(a => a.Frequency)
                .OrderByDescending(a => a.Count())
                .Take(limit)
                .Select(a => new { Frequnecy = a.Key, Count = a.Count() })
                .ToListAsync();
        }

        public static async Task<IEnumerable<object>> GetMostCommonUsedManufacturer(this IQueryable<AccessPoint> accessPoints, int limit)
        {
            return await accessPoints
                .Where(a => a.DisplayStatus.Value)
                .Where(a => !a.Manufacturer.Value.IsEmpty())
                .GroupBy(a => a.Manufacturer.Value)
                .OrderByDescending(a => a.Count())
                .Take(limit)
                .Select(a => new { Manufacturer = a.Key, Count = a.Count() })
                .ToListAsync();
        }

        public static async Task<IEnumerable<object>> GetMostCommonUsedEncryptionTypes(this IQueryable<AccessPoint> accessPoints, int limit)
        {
            throw new NotImplementedException();
        }
    }
}
