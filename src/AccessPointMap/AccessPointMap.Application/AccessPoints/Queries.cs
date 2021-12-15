﻿using AccessPointMap.Domain.AccessPoints;
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
    }
}