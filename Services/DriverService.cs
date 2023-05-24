﻿using Microsoft.Extensions.Options;
using MongoDB.Driver;
using WebAPIMongo.Configurations;

namespace WebAPIMongo.Services
{
    public class DriverService
    {
        private readonly IMongoCollection<Driver> _driverCollection;
        public DriverService(IOptions<DbSettings> dbSetttings)
        {
            var mongoClient = new MongoClient(dbSetttings.Value.ConnectionString);
            var mongoDb = mongoClient.GetDatabase(dbSetttings.Value.DatabaseName);

            var _driverCollection = mongoDb.GetCollection<Driver>(dbSetttings.Value.CollectionName);
        }

        public async Task<List<Driver>> GetDriversAsync()
        {
            return await _driverCollection.Find(driver => true).ToListAsync();
        }

        public async Task<Driver> GetDriver (int number)
        {
            return await _driverCollection.Find(driver => driver.Number == number).FirstOrDefaultAsync();
        }

        public async Task CreateDriver(Driver driver)
        {
            await _driverCollection.InsertOneAsync(driver);
        }

        public async Task UpdateDriver(Driver driver)
        {
            await _driverCollection.ReplaceOneAsync(d => d.Number == driver.Number, driver);
        }

        public async Task DeleteDriver(int number)
        {
            await _driverCollection.DeleteOneAsync(d => d.Number == number);
        }
    }
}