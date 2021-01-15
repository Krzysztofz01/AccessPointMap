using AccessPointMapWebApi.Models;
using AccessPointMapWebApi.Settings;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccessPointMapWebApi.Repositories
{
    public interface ILogsRepository
    {
        public Task Create(string message);
        public Task Clear();
        public Task<IEnumerable<Log>> GetLogs();
        public Task<IEnumerable<Log>> GetAllLogs();
    }

    public class LogsRepository : ILogsRepository
    {
        private readonly IMongoCollection<Log> context;

        public LogsRepository(ILogsDatabaseSettings logsDatabaseSettings)
        {
            var client = new MongoClient(logsDatabaseSettings.ConnectionString);
            var database = client.GetDatabase(logsDatabaseSettings.DatabaseName);
            context = database.GetCollection<Log>(logsDatabaseSettings.LogCollectionName);
        }

        public async Task Clear()
        {
            await context.DeleteManyAsync(x => x.Date < DateTime.Now.AddDays(-7));
        }

        public async Task Create(string message)
        {
            var log = new Log();
            log.Date = DateTime.Now;
            log.Message = message;
            await context.InsertOneAsync(log);
        }

        public async Task<IEnumerable<Log>> GetAllLogs()
        {
            return await context.Find(x => true).ToListAsync();
        }

        public async Task<IEnumerable<Log>> GetLogs()
        {
            return await context.Find(x => x.Date >= DateTime.Now.AddDays(-7)).ToListAsync();
        }
    }
}
