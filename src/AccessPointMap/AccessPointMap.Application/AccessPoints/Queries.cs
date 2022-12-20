using AccessPointMap.Application.Core;
using AccessPointMap.Application.Extensions;
using AccessPointMap.Domain.AccessPoints;
using AccessPointMap.Domain.AccessPoints.AccessPointPackets;
using AccessPointMap.Domain.AccessPoints.AccessPointStamps;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace AccessPointMap.Application.AccessPoints
{
    public static class Queries
    {
        public static async Task<Result<IEnumerable<AccessPoint>>> GetAllAccessPoints(
            this IAccessPointRepository accessPointRepository,
            DateTime? startingDate,
            DateTime? endingData,
            double? latitude,
            double? longitude,
            double? distance,
            string keyword,
            int? page,
            int? pageSize,
            CancellationToken cancellationToken = default)
        {
            // NOTE: Filtering performed on the database-site
            var databaseResult = await accessPointRepository.Entities
                .Where(a => a.DisplayStatus.Value)
                .WhereParamPresent(startingDate.HasValue, a => a.CreationTimestamp.Value > startingDate.Value)
                .WhereParamPresent(endingData.HasValue, a => a.CreationTimestamp.Value < endingData.Value)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();

            // NOTE: Filtering performed on the server-site
            return databaseResult
                .WhereParamPresent(latitude.HasValue && longitude.HasValue && distance.HasValue, a => Helpers.IsAccessPointInArea(latitude.Value, longitude.Value, distance.Value, a))
                .WhereParamPresent(keyword is not null, a => Helpers.IsMatchingKeyword(keyword, a))
                .Paginate(page, pageSize)
                .ToResultObject();
        }

        public static async Task<Result<IEnumerable<AccessPoint>>> GetAllAccessPointsAdministration(
            this IAccessPointRepository accessPointRepository,
            DateTime? startingDate,
            DateTime? endingData,
            double? latitude,
            double? longitude,
            double? distance,
            string keyword,
            int? page,
            int? pageSize,
            CancellationToken cancellationToken = default)
        {
            // NOTE: Filtering performed on the database-site
            var databaseResult = await accessPointRepository.Entities
                .WhereParamPresent(startingDate.HasValue, a => a.CreationTimestamp.Value > startingDate.Value)
                .WhereParamPresent(endingData.HasValue, a => a.CreationTimestamp.Value < endingData.Value)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();

            // NOTE: Filtering performed on the server-site
            return databaseResult
                .WhereParamPresent(latitude.HasValue && longitude.HasValue && distance.HasValue, a => Helpers.IsAccessPointInArea(latitude.Value, longitude.Value, distance.Value, a))
                .WhereParamPresent(keyword is not null, a => Helpers.IsMatchingKeyword(keyword, a))
                .Paginate(page, pageSize)
                .ToResultObject();
        }

        public static async Task<Result<AccessPoint>> GetAccessPointById(
            this IAccessPointRepository accessPointRepository,
            Guid id,
            CancellationToken cancellationToken = default)
        {
            return await accessPointRepository.Entities
                .Include(a => a.Stamps)
                .Where(a => a.DisplayStatus.Value)
                .AsNoTracking()
                .SingleOrDefaultAsync(a => a.Id == id, cancellationToken)
                .ToResultObjectAsync();
        }

        public static async Task<Result<AccessPoint>> GetAccessPointByIdAdministration(
            this IAccessPointRepository accessPointRepository,
            Guid id,
            CancellationToken cancellationToken = default)
        {
            return await accessPointRepository.Entities
                .Include(a => a.Stamps)
                .Include(a => a.Adnnotations)
                .AsNoTracking()
                .SingleOrDefaultAsync(a => a.Id == id, cancellationToken)
                .ToResultObjectAsync();
        }

        public static async Task<Result<IEnumerable<AccessPoint>>> GetAllAccessPointsByRunId(
            this IAccessPointRepository accessPointRepository,
            Guid runId,
            CancellationToken cancellationToken = default)
        {
            return await accessPointRepository.Entities
                .Where(a => a.RunIdentifier.Value.HasValue)
                .Where(a => a.RunIdentifier.Value.Value == runId)
                .Where(a => a.DisplayStatus.Value)
                .AsNoTracking()
                .ToListAsync(cancellationToken)
                .ToResultObjectAsync();
        }

        public static async Task<Result<IEnumerable<AccessPoint>>> GetAllAccessPointsByRunIdAdministration(
            this IAccessPointRepository accessPointRepository,
            Guid runId,
            CancellationToken cancellationToken = default)
        {
            return await accessPointRepository.Entities
                .Where(a => a.RunIdentifier.Value.HasValue)
                .Where(a => a.RunIdentifier.Value.Value == runId)
                .AsNoTracking()
                .ToListAsync(cancellationToken)
                .ToResultObjectAsync();
        }

        public static async Task<Result<IEnumerable<AccessPointStamp>>> GetAllAccessPointStampsByRunId(
            this IAccessPointRepository accessPointRepository,
            Guid runId,
            CancellationToken cancellationToken = default)
        {
            return await accessPointRepository.Entities
                .Include(a => a.Stamps)
                .Where(a => a.DisplayStatus.Value)
                .SelectMany(a => a.Stamps)
                .Where(a => a.RunIdentifier.Value.HasValue)
                .Where(a => a.RunIdentifier.Value == runId)
                .AsNoTracking()
                .ToListAsync(cancellationToken)
                .ToResultObjectAsync();
        }

        public static async Task<Result<IEnumerable<AccessPointStamp>>> GetAllAccessPointStampsByRunIdAdministration(
            this IAccessPointRepository accessPointRepository,
            Guid runId,
            CancellationToken cancellationToken = default)
        {
            return await accessPointRepository.Entities
                .Include(a => a.Stamps)
                .SelectMany(a => a.Stamps)
                .Where(a => a.RunIdentifier.Value.HasValue)
                .Where(a => a.RunIdentifier.Value == runId)
                .AsNoTracking()
                .ToListAsync(cancellationToken)
                .ToResultObjectAsync();
        }

        public static async Task<Result<IEnumerable<string>>> GetAllAccessPointRunIds(
            this IAccessPointRepository accessPointRepository,
            CancellationToken cancellationToken = default)
        {
            var accessPointRunIds = (await accessPointRepository.Entities
                .Where(a => a.DisplayStatus.Value)
                .Where(a => a.RunIdentifier.Value.HasValue)
                .OrderBy(a => a.CreationTimestamp.Value)
                .AsNoTracking()
                .ToListAsync(cancellationToken))
                .DistinctBy(a => a.RunIdentifier.Value.Value)
                .Select(a => new Tuple<Guid, DateTime>(a.RunIdentifier.Value.Value, a.CreationTimestamp.Value));

            var accessPointStampRunIds = (await accessPointRepository.Entities
                .Include(a => a.Stamps)
                .Where(a => a.DisplayStatus.Value)
                .SelectMany(a => a.Stamps)
                .Where(s => s.RunIdentifier.Value.HasValue)
                .OrderBy(s => s.CreationTimestamp.Value)
                .AsNoTracking()
                .ToListAsync(cancellationToken))
                .DistinctBy(s => s.RunIdentifier.Value.Value)
                .Select(s => new Tuple<Guid, DateTime>(s.RunIdentifier.Value.Value, s.CreationTimestamp.Value));

            return accessPointRunIds
                .Union(accessPointStampRunIds)
                .OrderBy(a => a.Item2)
                .DistinctBy(a => a.Item1)
                .Select(a => a.Item1.ToString())
                .ToList()
                .ToResultObject();
        }

        public static async Task<Result<IEnumerable<string>>> GetAllAccessPointRunIdsAdministration(
            this IAccessPointRepository accessPointRepository,
            CancellationToken cancellationToken = default)
        {
            var accessPointRunIds = (await accessPointRepository.Entities
                .Where(a => a.RunIdentifier.Value.HasValue)
                .OrderBy(a => a.CreationTimestamp.Value)
                .AsNoTracking()
                .ToListAsync(cancellationToken))
                .DistinctBy(a => a.RunIdentifier.Value.Value)
                .Select(a => new Tuple<Guid, DateTime>(a.RunIdentifier.Value.Value, a.CreationTimestamp.Value));

            var accessPointStampRunIds = (await accessPointRepository.Entities
                .Include(a => a.Stamps)
                .SelectMany(a => a.Stamps)
                .Where(s => s.RunIdentifier.Value.HasValue)
                .OrderBy(s => s.CreationTimestamp.Value)
                .AsNoTracking()
                .ToListAsync(cancellationToken))
                .DistinctBy(s => s.RunIdentifier.Value.Value)
                .Select(s => new Tuple<Guid, DateTime>(s.RunIdentifier.Value.Value, s.CreationTimestamp.Value));

            return accessPointRunIds
                .Union(accessPointStampRunIds)
                .OrderBy(a => a.Item2)
                .DistinctBy(a => a.Item1)
                .Select(a => a.Item1.ToString())
                .ToList()
                .ToResultObject();
        }

        public static async Task<Result<IEnumerable<AccessPointPacket>>> GetAllAccessPointsAccessPointPackets(
            this IAccessPointRepository accessPointRepository,
            Guid id,
            CancellationToken cancellationToken = default)
        {
            return await accessPointRepository.Entities
                .Include(a => a.Packets)
                .Where(a => a.Id == id)
                .SelectMany(a => a.Packets)
                .AsNoTracking()
                .ToListAsync(cancellationToken)
                .ToResultObjectAsync();
        }

        public static async Task<Result<AccessPointPacket>> GetAccessPointsAccessPointPacketById(
            this IAccessPointRepository accessPointRepository,
            Guid id,
            Guid packetId,
            CancellationToken cancellationToken = default)
        {
            return await accessPointRepository.Entities
                .Include(a => a.Packets)
                .Where(a => a.Id == id)
                .SelectMany(a => a.Packets)
                .AsNoTracking()
                .SingleOrDefaultAsync(a => a.Id == packetId, cancellationToken)
                .ToResultObjectAsync();
        }

        public static async Task<Result<AccessPoint>> MatchAccessPointByAccessPointStampId(
            this IAccessPointRepository accessPointRepository,
            Guid stampId,
            CancellationToken cancellationToken = default)
        {
            return await accessPointRepository.Entities
                .Include(a => a.Stamps)
                .Where(a => a.DisplayStatus.Value)
                .Where(a => a.Stamps.Any(s => s.Id == stampId))
                .AsNoTracking()
                .SingleOrDefaultAsync(cancellationToken)
                .ToResultObjectAsync();
        }

        public static async Task<Result<AccessPoint>> MatchAccessPointByAccessPointStampIdAdministration(
            this IAccessPointRepository accessPointRepository,
            Guid stampId,
            CancellationToken cancellationToken = default)
        {
            return await accessPointRepository.Entities
                .Include(a => a.Stamps)
                .Where(a => a.Stamps.Any(s => s.Id == stampId))
                .AsNoTracking()
                .SingleOrDefaultAsync(cancellationToken)
                .ToResultObjectAsync();
        }

        public static async Task<Result<AccessPoint>> MatchAccessPointByAccessPointPacketId(
            this IAccessPointRepository accessPointRepository,
            Guid packetId,
            CancellationToken cancellationToken = default)
        {
            return await accessPointRepository.Entities
                .Include(a => a.Packets)
                .Where(a => a.DisplayStatus.Value)
                .Where(a => a.Packets.Any(p => p.Id == packetId))
                .AsNoTracking()
                .SingleOrDefaultAsync(cancellationToken)
                .ToResultObjectAsync();
        }

        public static async Task<Result<AccessPoint>> MatchAccessPointByAccessPointPacketIdAdministration(
            this IAccessPointRepository accessPointRepository,
            Guid packetId,
            CancellationToken cancellationToken = default)
        {
            return await accessPointRepository.Entities
                .Include(a => a.Stamps)
                .Include(a => a.Packets)
                .Where(a => a.Packets.Any(p => p.Id == packetId))
                .AsNoTracking()
                .SingleOrDefaultAsync(cancellationToken)
                .ToResultObjectAsync();
        }

        // TODO: Resolve problems related to this query
        // The main get endpoint can be used the same way, this endpoint were used
        // The filtering is happening now on the server side, which might be less efficient
        // We can remove this method (breaking change) or leave it as a alternative way
        //
        // For now the query will stay, but will be removed in the next braking-changes release
        [Obsolete("This query will be removed in the future release. Use the GetAllAccessPoints() method instead.")]
        public static async Task<Result<IEnumerable<AccessPoint>>> SearchByKeyword(
            this IAccessPointRepository accessPointRepository,
            string keyword,
            CancellationToken cancellationToken = default)
        {
            string kw = keyword.Trim().ToLower();

            return await accessPointRepository.Entities
                .Where(a =>
                    a.Ssid.Value.ToLower().Contains(kw) ||
                    a.DeviceType.Value.ToLower().Contains(kw) ||
                    a.Security.RawSecurityPayload.ToLower().Contains(kw))
                .AsNoTracking()
                .ToListAsync(cancellationToken)
                .ToResultObjectAsync();
        }

        public static async Task<Result<IEnumerable<AccessPoint>>> GetAccessPointsWithGreatestSignalRange(
            this IAccessPointRepository accessPointRepository,
            int limit,
            CancellationToken cancellationToken = default)
        {
            var greatestSignalRangeQuery = accessPointRepository.Entities
                .Where(x => x.DisplayStatus.Value)
                .OrderByDescending(a => a.Positioning.SignalArea)
                .AsNoTracking();

            if (limit == default)
            {
                return await greatestSignalRangeQuery
                    .ToListAsync(cancellationToken)
                    .ToResultObjectAsync();
            }
            else
            {
                return await greatestSignalRangeQuery
                    .Take(limit)
                    .ToListAsync(cancellationToken)
                    .ToResultObjectAsync();
            }
        }

        public static async Task<Result<IEnumerable<object>>> GetMostCommonUsedFrequency(
            this IAccessPointRepository accessPointRepository,
            int limit,
            CancellationToken cancellationToken = default)
        {
            // TODO: This query can return frequency as strings in the future to avoid "0 as other" hacking
            // const string _other = "Other";
            const double _other = 0;

            var mostCommonFrequencyQuery = accessPointRepository.Entities
                .Where(a => a.DisplayStatus.Value)
                .Where(a => a.Frequency.Value != default)
                .GroupBy(a => a.Frequency.Value)
                .OrderByDescending(a => a.Count())
                .Select(a => new { Frequency = a.Key, Count = a.Count() })
                .OrderByDescending(a => a.Count)
                .AsNoTracking();

            if (limit == default)
            {
                var queryResult = await mostCommonFrequencyQuery
                    .ToListAsync(cancellationToken);

                return queryResult.Count == 0
                    ? Result.Failure<IEnumerable<object>>(NotFoundError.Default)
                    : Result.Success<IEnumerable<object>>(queryResult);
            }
            else if (limit == 1)
            {
                var queryResult = await mostCommonFrequencyQuery
                    .Take(1)
                    .ToListAsync(cancellationToken);

                return queryResult.Count == 0
                    ? Result.Failure<IEnumerable<object>>(NotFoundError.Default)
                    : Result.Success<IEnumerable<object>>(queryResult);
            }
            else
            {
                var frequenciesAboveLimit = await mostCommonFrequencyQuery
                    .Take(limit - 1)
                    .ToListAsync(cancellationToken);

                var frequenciesUnderLimit = await mostCommonFrequencyQuery
                    .Skip(limit - 1)
                    .SumAsync(a => a.Count, cancellationToken);

                if (frequenciesUnderLimit > 0)
                {
                    frequenciesAboveLimit.Add(new { Frequency = _other, Count = frequenciesUnderLimit });
                }

                return frequenciesAboveLimit.Count == 0
                    ? Result.Failure<IEnumerable<object>>(NotFoundError.Default)
                    : Result.Success<IEnumerable<object>>(frequenciesAboveLimit);
            }
        }

        public static async Task<Result<IEnumerable<object>>> GetMostCommonUsedManufacturer(
            this IAccessPointRepository accessPointRepository,
            int limit,
            CancellationToken cancellationToken = default)
        {
            var mostCommonManufacturerQuery = accessPointRepository.Entities
                .Where(a => a.DisplayStatus.Value)
                .Where(a => !string.IsNullOrEmpty(a.Manufacturer.Value))
                .GroupBy(a => a.Manufacturer.Value)
                .OrderByDescending(a => a.Count())
                .Select(a => new { Manufacturer = a.Key, Count = a.Count() })
                .AsNoTracking();

            if (limit == default)
            {
                var queryResult = await mostCommonManufacturerQuery
                    .ToListAsync(cancellationToken);

                return queryResult.Count == 0
                    ? Result.Failure<IEnumerable<object>>(NotFoundError.Default)
                    : Result.Success<IEnumerable<object>>(queryResult);
            }
            else
            {
                var queryResult = await mostCommonManufacturerQuery
                    .Take(limit)
                    .ToListAsync(cancellationToken);

                return queryResult.Count == 0
                    ? Result.Failure<IEnumerable<object>>(NotFoundError.Default)
                    : Result.Success<IEnumerable<object>>(queryResult);
            }
        }

        public static async Task<Result<IEnumerable<object>>> GetMostCommonUsedEncryptionTypes(
            this IAccessPointRepository accessPointRepository,
            int limit,
            CancellationToken cancellationToken = default)
        {
            const string _none = "None";
            const string _other = "Other";

            var encryptionCountMap = Constants.SecurityProtocols
                .Where(e => e.Value.Type == SecurityProtocolType.Framework)
                .OrderByDescending(e => e.Value.Priority)
                .ToDictionary(k => k.Value.Name, v => 0);

            encryptionCountMap.Add(_none, 0);

            var accessPointsEncryptions = await accessPointRepository.Entities
                .Where(a => a.DisplayStatus.Value)
                .Select(a => a.Security.SecurityStandards)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            foreach (var serializedEncryption in accessPointsEncryptions)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var encryption = JsonSerializer.Deserialize<string[]>(serializedEncryption).FirstOrDefault();

                if (encryption is null) encryptionCountMap[_none]++;
                if (encryption is not null) encryptionCountMap[encryption]++;
            }

            var sortedEncryptionCountMap = encryptionCountMap
                .Select(e => new { Encryption = e.Key, Count = e.Value })
                .OrderByDescending(e => e.Count);

            if (limit == default)
            {
                return (!sortedEncryptionCountMap.Any())
                    ? Result.Failure<IEnumerable<object>>(NotFoundError.Default)
                    : Result.Success<IEnumerable<object>>(sortedEncryptionCountMap);
            }
            else if (limit == 1)
            {
                var queryResult = sortedEncryptionCountMap
                    .Take(1);

                return (!queryResult.Any())
                    ? Result.Failure<IEnumerable<object>>(NotFoundError.Default)
                    : Result.Success<IEnumerable<object>>(sortedEncryptionCountMap);
            }
            else
            {
                var encryptionsAboveLimit = sortedEncryptionCountMap
                    .Take(limit - 1).ToList();

                var encryptionsUnderLimitCount = sortedEncryptionCountMap
                    .Skip(limit - 1)
                    .Sum(e => e.Count);

                if (encryptionsUnderLimitCount > 0)
                {
                    encryptionsAboveLimit.Add(new { Encryption = _other, Count = encryptionsUnderLimitCount });
                }

                return (encryptionsAboveLimit.Count == 0)
                    ? Result.Failure<IEnumerable<object>>(NotFoundError.Default)
                    : Result.Success<IEnumerable<object>>(encryptionsAboveLimit);
            }
        }

        public static async Task<Result<IEnumerable<AccessPoint>>> GetAccessPointsForCsvExport(
            this IAccessPointRepository accessPointRepository,
            CancellationToken cancellationToken = default)
        {
            return await accessPointRepository.Entities
                .Where(a => a.DisplayStatus.Value)
                .ToListAsync(cancellationToken)
                .ToResultObjectAsync();
        }

        public static async Task<Result<IEnumerable<string>>> GetAccessPointIdsWithoutManufacturerSpecified(
            this IAccessPointRepository accessPointRepository,
            CancellationToken cancellationToken = default)
        {
            return await accessPointRepository.Entities
                .Where(a => a.Manufacturer.Value == string.Empty)
                .Select(a => a.Id.ToString())
                .ToListAsync(cancellationToken)
                .ToResultObjectAsync();
        }

        public static async Task<Result<IEnumerable<AccessPoint>>> GetAccessPointsForPresenceCheckJob(
            this IAccessPointRepository accessPointRepository,
            CancellationToken cancellationToken = default)
        {
            return await accessPointRepository.Entities
                .Include(a => a.Stamps)
                .ToListAsync(cancellationToken)
                .ToResultObjectAsync();
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
