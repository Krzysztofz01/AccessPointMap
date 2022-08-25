using AccessPointMap.Application.Extensions;
using AccessPointMap.Domain.AccessPoints;
using AccessPointMap.Domain.AccessPoints.AccessPointPackets;
using AccessPointMap.Domain.AccessPoints.AccessPointStamps;
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
        public static async Task<IEnumerable<AccessPoint>> GetAllAccessPoints(
            this IQueryable<AccessPoint> accessPoints,
            DateTime? startingDate,
            DateTime? endingData,
            double? latitude,
            double? longitude,
            double? distance,
            string keyword,
            int? page,
            int? pageSize)
        {
            // Database handled query filtering
            var databaseResult = await accessPoints
                .Where(a => a.DisplayStatus.Value)
                .WhereParamPresent(startingDate.HasValue, a => a.CreationTimestamp.Value > startingDate.Value)
                .WhereParamPresent(endingData.HasValue, a => a.CreationTimestamp.Value < endingData.Value)
                .AsNoTracking()
                .ToListAsync();

            // Server handled query filtering
            return databaseResult
                .WhereParamPresent(latitude.HasValue && longitude.HasValue && distance.HasValue, a => Helpers.IsAccessPointInArea(latitude.Value, longitude.Value, distance.Value, a))
                .WhereParamPresent(keyword is not null, a => Helpers.IsMatchingKeyword(keyword, a))
                .Paginate(page, pageSize);
        }

        public static async Task<IEnumerable<AccessPoint>> GetAllAccessPointsAdministration(
            this IQueryable<AccessPoint> accessPoints,
            DateTime? startingDate,
            DateTime? endingData,
            double? latitude,
            double? longitude,
            double? distance,
            string keyword,
            int? page,
            int? pageSize)
        {
            // Database handled query filtering
            var databaseResult = await accessPoints
                .WhereParamPresent(startingDate.HasValue, a => a.CreationTimestamp.Value > startingDate.Value)
                .WhereParamPresent(endingData.HasValue, a => a.CreationTimestamp.Value < endingData.Value)
                .AsNoTracking()
                .ToListAsync();

            // Server handled query filtering
            return databaseResult
                .WhereParamPresent(latitude.HasValue && longitude.HasValue && distance.HasValue, a => Helpers.IsAccessPointInArea(latitude.Value, longitude.Value, distance.Value, a))
                .WhereParamPresent(keyword is not null, a => Helpers.IsMatchingKeyword(keyword, a))
                .Paginate(page, pageSize);
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
                .Include(a => a.Adnnotations)
                .AsNoTracking()
                .SingleAsync(a => a.Id == id); 
        }

        public static async Task<IEnumerable<AccessPoint>> GetAllAccessPointsByRunId(this IQueryable<AccessPoint> accessPoints, Guid runId)
        {
            return await accessPoints
                .Where(a => a.RunIdentifier.Value.HasValue)
                .Where(a => a.RunIdentifier.Value.Value == runId)
                .Where(a => a.DisplayStatus.Value)
                .AsNoTracking()
                .ToListAsync();
        }

        public static async Task<IEnumerable<AccessPoint>> GetAllAccessPointsByRunIdAdministration(this IQueryable<AccessPoint> accessPoints, Guid runId)
        {
            return await accessPoints
                .Where(a => a.RunIdentifier.Value.HasValue)
                .Where(a => a.RunIdentifier.Value.Value == runId)
                .AsNoTracking()
                .ToListAsync();
        }

        public static async Task<IEnumerable<AccessPointStamp>> GetAllAccessPointStampsByRunId(this IQueryable<AccessPoint> accessPoints, Guid runId)
        {
            return await accessPoints
                .Include(a => a.Stamps)
                .Where(a => a.DisplayStatus.Value)
                .SelectMany(a => a.Stamps)
                .Where(a => a.RunIdentifier.Value.HasValue)
                .Where(a => a.RunIdentifier.Value == runId)
                .AsNoTracking()
                .ToListAsync(); 
        }

        public static async Task<IEnumerable<AccessPointStamp>> GetAllAccessPointStampsByRunIdAdministration(this IQueryable<AccessPoint> accessPoints, Guid runId)
        {
            return await accessPoints
                .Include(a => a.Stamps)
                .SelectMany(a => a.Stamps)
                .Where(a => a.RunIdentifier.Value.HasValue)
                .Where(a => a.RunIdentifier.Value == runId)
                .AsNoTracking()
                .ToListAsync();
        }

        public static async Task<IEnumerable<Guid>> GetAllAccessPointRunIds(this IQueryable<AccessPoint> accessPoints)
        {
            var accessPointRunIds = (await accessPoints
                .Where(a => a.DisplayStatus.Value)
                .Where(a => a.RunIdentifier.Value.HasValue)
                .OrderBy(a => a.CreationTimestamp.Value)
                .AsNoTracking()
                .ToListAsync())
                .DistinctBy(a => a.RunIdentifier.Value.Value)
                .Select(a => new Tuple<Guid, DateTime>(a.RunIdentifier.Value.Value, a.CreationTimestamp.Value));

            var accessPointStampRunIds = (await accessPoints
                .Include(a => a.Stamps)
                .Where(a => a.DisplayStatus.Value)
                .SelectMany(a => a.Stamps)
                .Where(s => s.RunIdentifier.Value.HasValue)
                .OrderBy(s => s.CreationTimestamp.Value)
                .AsNoTracking()
                .ToListAsync())
                .DistinctBy(s => s.RunIdentifier.Value.Value)
                .Select(s => new Tuple<Guid, DateTime>(s.RunIdentifier.Value.Value, s.CreationTimestamp.Value));

            return accessPointRunIds
                .Union(accessPointStampRunIds)
                .OrderBy(a => a.Item2)
                .DistinctBy(a => a.Item1)
                .Select(a => a.Item1);
        }

        public static async Task<IEnumerable<Guid>> GetAllAccessPointRunIdsAdministration(this IQueryable<AccessPoint> accessPoints)
        {
            var accessPointRunIds = (await accessPoints
                .Where(a => a.RunIdentifier.Value.HasValue)
                .OrderBy(a => a.CreationTimestamp.Value)
                .AsNoTracking()
                .ToListAsync())
                .DistinctBy(a => a.RunIdentifier.Value.Value)
                .Select(a => new Tuple<Guid, DateTime>(a.RunIdentifier.Value.Value, a.CreationTimestamp.Value));

            var accessPointStampRunIds = (await accessPoints
                .Include(a => a.Stamps)
                .SelectMany(a => a.Stamps)
                .Where(s => s.RunIdentifier.Value.HasValue)
                .OrderBy(s => s.CreationTimestamp.Value)
                .AsNoTracking()
                .ToListAsync())
                .DistinctBy(s => s.RunIdentifier.Value.Value)
                .Select(s => new Tuple<Guid, DateTime>(s.RunIdentifier.Value.Value, s.CreationTimestamp.Value));

            return accessPointRunIds
                .Union(accessPointStampRunIds)
                .OrderBy(a => a.Item2)
                .DistinctBy(a => a.Item1)
                .Select(a => a.Item1);
        }

        public static async Task<IEnumerable<AccessPointPacket>> GetAllAccessPointsAccessPointPackets(this IQueryable<AccessPoint> accessPoints, Guid id)
        {
            return await accessPoints
                .Include(a => a.Packets)
                .Where(a => a.Id == id)
                .SelectMany(a => a.Packets)
                .AsNoTracking()
                .ToListAsync();
        }

        public static async Task<AccessPointPacket> GetAccessPointsAccessPointPacketById(this IQueryable<AccessPoint> accessPoints, Guid id, Guid packetId)
        {
            return await accessPoints
                .Include(a => a.Packets)
                .Where(a => a.Id == id)
                .SelectMany(a => a.Packets)
                .AsNoTracking()
                .SingleAsync(a => a.Id == packetId);
        }

        public static async Task<AccessPoint> MatchAccessPointByAccessPointStampId(this IQueryable<AccessPoint> accessPoints, Guid stampId)
        {
            return await accessPoints
                .Include(a => a.Stamps)
                .Where(a => a.DisplayStatus.Value)
                .Where(a => a.Stamps.Any(s => s.Id == stampId))
                .AsNoTracking()
                .SingleAsync();
        }

        public static async Task<AccessPoint> MatchAccessPointByAccessPointStampIdAdministration(this IQueryable<AccessPoint> accessPoints, Guid stampId)
        {
            return await accessPoints
                .Include(a => a.Stamps)
                .Where(a => a.Stamps.Any(s => s.Id == stampId))
                .AsNoTracking()
                .SingleAsync();
        }

        public static async Task<AccessPoint> MatchAccessPointByAccessPointPacketId(this IQueryable<AccessPoint> accessPoints, Guid packetId)
        {
            return await accessPoints
                .Include(a => a.Packets)
                .Where(a => a.DisplayStatus.Value)
                .Where(a => a.Packets.Any(p => p.Id == packetId))
                .AsNoTracking()
                .SingleAsync();
        }

        public static async Task<AccessPoint> MatchAccessPointByAccessPointPacketIdAdministration(this IQueryable<AccessPoint> accessPoints, Guid packetId)
        {
            return await accessPoints
                .Include(a => a.Stamps)
                .Include(a => a.Packets)
                .Where(a => a.Packets.Any(p => p.Id == packetId))
                .AsNoTracking()
                .SingleAsync();
        }

        // TODO: Resolve problems related to this query
        // The main get endpoint can be used the same way, this endpoint were used
        // The filtering is happening now on the server side, which might be less efficient
        // We can remove this method (breaking change) or leave it as a alternative way
        //
        // For now the query will stay, but will be removed in the next braking-changes release
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
                .Select(a => new { Frequency = a.Key, Count = a.Count() })
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

            var encryptionCountMap = Constants.SecurityProtocols
                .Where(e => e.Value.Type == SecurityProtocolType.Framework)
                .OrderByDescending(e => e.Value.Priority)
                .ToDictionary(k => k.Value.Name, v => 0);

            encryptionCountMap.Add(_none, 0);

            var accessPointsEncryptions = await accessPoints
                .Where(a => a.DisplayStatus.Value)
                .Select(a => a.Security.SecurityStandards)
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

        private static class Helpers
        {
            public static bool IsAccessPointInArea(
                double latitude,
                double longitude,
                double distance,
                AccessPoint accessPoint)
            {
                const double _pi = 3.1415;
                double o1 = latitude * _pi / 180.0;
                double o2 = accessPoint.Positioning.HighSignalLatitude * _pi / 180.0;

                double so = (accessPoint.Positioning.HighSignalLatitude - latitude) * _pi / 180.0;
                double sl = (accessPoint.Positioning.HighSignalLongitude - longitude) * _pi / 180.0;

                double a = Math.Pow(Math.Sin(so / 2.0), 2.0) + Math.Cos(o1) * Math.Cos(o2) * Math.Pow(Math.Sin(sl / 2.0), 2.0);
                double c = 2.0 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1.0 - a));

                return Math.Round(6371e3 * c, 2) <= distance;
            }

            public static bool IsMatchingKeyword(string keyword, AccessPoint accessPoint)
            {
                string kw = keyword.Trim().ToLower();

                return accessPoint.Ssid.Value.ToLower().Contains(kw) ||
                    accessPoint.DeviceType.Value.ToLower().Contains(kw) ||
                    accessPoint.Security.RawSecurityPayload.ToLower().Contains(kw);
            }
        }
    }
}
