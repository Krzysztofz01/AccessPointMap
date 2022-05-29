using AccessPointMap.Domain.AccessPoints;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace AccessPointMap.Application.AccessPoints
{
    public static class Queries
    {
        public static async Task<IEnumerable<AccessPoint>> GetAllAccessPoints(this IQueryable<AccessPoint> accessPoints)
        {
            return await accessPoints
                .Where(a => a.DisplayStatus.Value)
                .AsNoTracking()
                .ToListAsync();
        }

        public static async Task<IEnumerable<AccessPoint>> GetAllAccessPointsAdministration(this IQueryable<AccessPoint> accessPoints)
        {
            return await accessPoints
                .AsNoTracking()
                .ToListAsync();
        }

        public static async Task<AccessPoint> GetAccessPointById(this IQueryable<AccessPoint> accessPoints, Guid id)
        {
            return await accessPoints
                .Include(a => a.Stamps)
                .Where(a => a.DisplayStatus.Value)
                .AsNoTracking()
                .SingleAsync(a => a.Id == id);
        }

        public static async Task<AccessPoint> GetAccessPointByIdAdministration(this IQueryable<AccessPoint> accessPoints, Guid id)
        {
            return await accessPoints
                .Include(a => a.Stamps)
                .AsNoTracking()
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
                .AsNoTracking()
                .ToListAsync();
        }

        public static async Task<IEnumerable<AccessPoint>> GetAccessPointsWithGreatestSignalRange(this IQueryable<AccessPoint> accessPoints, int limit)
        {
            return await accessPoints
                .Where(x => x.DisplayStatus.Value)
                .OrderByDescending(a => a.Positioning.SignalArea)
                .Take(limit)
                .AsNoTracking()
                .ToListAsync();
        }

        public static async Task<IEnumerable<object>> GetMostCommonUsedFrequency(this IQueryable<AccessPoint> accessPoints, int limit)
        {
            return await accessPoints
                .Where(a => a.DisplayStatus.Value)
                .Where(a => a.Frequency.Value != default)
                .GroupBy(a => a.Frequency.Value)
                .OrderByDescending(a => a.Count())
                .Take(limit)
                .Select(a => new { Frequnecy = a.Key, Count = a.Count() })
                .AsNoTracking()
                .ToListAsync();
        }

        public static async Task<IEnumerable<object>> GetMostCommonUsedManufacturer(this IQueryable<AccessPoint> accessPoints, int limit)
        {
            return await accessPoints
                .Where(a => a.DisplayStatus.Value)
                .Where(a => !string.IsNullOrEmpty(a.Manufacturer.Value))
                .GroupBy(a => a.Manufacturer.Value)
                .OrderByDescending(a => a.Count())
                .Take(limit)
                .Select(a => new { Manufacturer = a.Key, Count = a.Count() })
                .AsNoTracking()
                .ToListAsync();
        }

        public static async Task<IEnumerable<object>> GetMostCommonUsedEncryptionTypes(this IQueryable<AccessPoint> accessPoints, int limit)
        {
            const string _none = "None";

            var encryptionCountMap = Constants.EncryptionTypes
                .OrderByDescending(e => e.Priority)
                .ToDictionary(k => k.Name, v => 0);

            encryptionCountMap.Add(_none, 0);

            var accessPointsEncryptions = await accessPoints
                .Where(a => a.DisplayStatus.Value)
                .Select(a => a.Security.SerializedSecurityPayload)
                .AsNoTracking()
                .ToListAsync();

            foreach(var serializedEncryption in accessPointsEncryptions)
            {
                var encryption = JsonSerializer.Deserialize<string[]>(serializedEncryption).FirstOrDefault();

                if (encryption is null) encryptionCountMap[_none]++;
                if (encryption is not null) encryptionCountMap[encryption]++;
            }

            return (limit == default)
                ? encryptionCountMap.Select(e => new { Encryption = e.Key, Count = e.Value }).OrderByDescending(e => e.Count)
                : encryptionCountMap.Select(e => new { Encryption = e.Key, Count = e.Value }).OrderByDescending(e => e.Count).Take(limit);                
        }
    }
}
