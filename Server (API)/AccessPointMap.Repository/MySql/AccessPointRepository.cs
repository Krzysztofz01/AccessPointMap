﻿using AccessPointMap.Domain;
using AccessPointMap.Repository.MySql.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccessPointMap.Repository.MySql
{
    public class AccessPointRepository: Repository<AccessPoint>, IAccessPointRepository
    {
        public AccessPointRepository(AccessPointMapMySqlDbContext context) : base(context)
        {
        }

        public async Task<int> AllInsecureRecordsCount()
        {
            return await entities
                .Where(x => !x.IsSecure)
                .CountAsync();
        }

        public async Task<int> AllRecordsCount()
        {
            return await entities
                .CountAsync();
        }

        public IEnumerable<AccessPoint> GetAllIntegration()
        {
            return entities
                .AsNoTracking();
        }

        public IEnumerable<AccessPoint> GetAllMaster()
        {
            return entities
                .Include(x => x.UserAdded)
                .Include(x => x.UserModified)
                .Where(x => x.MasterGroup);
        }

        public IEnumerable<AccessPoint> GetAllNoBrand()
        {
            return entities
                .Where(x => string.IsNullOrEmpty(x.Manufacturer) && x.MasterGroup);
        }

        public IEnumerable<AccessPoint> GetAllPublic()
        {
            return entities
                .Include(x => x.UserAdded)
                .Include(x => x.UserModified)
                .Where(x => x.MasterGroup && x.Display);
        }

        public IEnumerable<AccessPoint> GetAllQueue()
        {
            return entities
                .Include(x => x.UserAdded)
                .Include(x => x.UserModified)
                .Where(x => !x.MasterGroup);
        }

        public async Task<AccessPoint> GetByBssidMaster(string bssid)
        {
            return await entities
                .Include(x => x.UserAdded)
                .Include(x => x.UserModified)
                .SingleOrDefaultAsync(x => x.Bssid == bssid && x.MasterGroup);
        }

        public async Task<AccessPoint> GetByIdGlobal(long accessPointId)
        {
            return await entities
                .Include(x => x.UserAdded)
                .Include(x => x.UserModified)
                .SingleOrDefaultAsync(x => x.Id == accessPointId);
        }

        public async Task<AccessPoint> GetByIdMaster(long accessPointId)
        {
            return await entities
                .Include(x => x.UserAdded)
                .Include(x => x.UserModified)
                .Where(x => x.MasterGroup)
                .SingleOrDefaultAsync(x => x.Id == accessPointId);
        }

        public async Task<AccessPoint> GetByIdPublic(long accessPointId)
        {
            return await entities
                .Include(x => x.UserAdded)
                .Include(x => x.UserModified)
                .Where(x => x.MasterGroup && x.Display)
                .SingleOrDefaultAsync(x => x.Id == accessPointId);
        }

        public async Task<AccessPoint> GetByIdQueue(long accessPointId)
        {
            return await entities
                .Include(x => x.UserAdded)
                .Include(x => x.UserModified)
                .Where(x => !x.MasterGroup)
                .SingleOrDefaultAsync(x => x.Id == accessPointId);
        }

        public IEnumerable<AccessPoint> SearchBySsid(string ssid)
        {
            string cleanedSsid = ssid.ToLower().Trim();

            return entities
                .Include(x => x.UserAdded)
                .Include(x => x.UserModified)
                .Where(x => x.MasterGroup && x.Display)
                .Where(x => x.Ssid.ToLower().Contains(cleanedSsid))
                .Take(10);
        }

        public IEnumerable<AccessPoint> TopAreaAccessPointsSorted()
        {
            return entities
                .OrderByDescending(x => x.SignalArea)
                .Take(5);
        }

        public IEnumerable<Tuple<string, int>> TopOccuringBrandsSorted()
        {
            return entities
                .Where(x => !string.IsNullOrEmpty(x.Manufacturer))
                .GroupBy(x => x.Manufacturer)
                .OrderByDescending(x => x.Count())
                .Take(5)
                .Select(x => new Tuple<string, int>(x.Key, x.Count()));
        }

        public IEnumerable<Tuple<double, int>> TopOccuringFrequencies()
        {
            return entities
                .GroupBy(x => x.Frequency)
                .OrderByDescending(x => x.Count())
                .Take(5)
                .Select(x => new Tuple<double, int>(x.Key, x.Count()));
        }

        public IEnumerable<Tuple<string, int>> TopOccuringSecurityTypes(IEnumerable<string> securityTypes)
        {
            //Probably not the most efficient way to do this task
            //
            // Complexity
            // n - records
            // m - security types
            // O(m + n * m) => O( m(n + 1) )
            var securityCount = new Dictionary<string, int>();

            foreach (var type in securityTypes) securityCount.Add(type, 0);

            securityCount.Add("None", 0);

            foreach (var ap in entities)
            {
                bool hasSecurity = false;

                foreach (var type in securityTypes)
                {
                    if (ap.FullSecurityData.Contains(type) && securityCount.ContainsKey(type))
                    {
                        securityCount[type]++;
                        hasSecurity = true;
                    }

                    if (!hasSecurity) securityCount["None"]++;
                }
            }

            return securityCount
                .Take(5)
                .Select(x => new Tuple<string, int>(x.Key, x.Value))
                .OrderByDescending(x => x.Item2);
        }

        public IEnumerable<AccessPoint> UserAddedAccessPoints(long userId)
        {
            return entities
                .Include(x => x.UserAdded)
                .Include(x => x.UserModified)
                .Where(x => x.UserAddedId == userId)
                .Where(x => x.Display && x.MasterGroup);
        }

        public IEnumerable<AccessPoint> UserModifiedAccessPoints(long userId)
        {
            return entities
                .Include(x => x.UserAdded)
                .Include(x => x.UserModified)
                .Where(x => x.UserModifiedId == userId)
                .Where(x => x.Display && x.MasterGroup);
        }
    }
}