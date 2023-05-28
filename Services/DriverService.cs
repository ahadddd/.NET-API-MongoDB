using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Diagnostics.CodeAnalysis;
using System.Web.Mvc;
using WebAPIMongo.Configurations;

namespace WebAPIMongo.Services
{
    public class DriverService
    {
        private readonly IMongoCollection<Driver> _driverCollection;
        public DriverService(IOptions<DbSettings> dbSettings)
        {
            var mongoClient = new MongoClient(dbSettings.Value.ConnectionString);
            var mongoDb = mongoClient.GetDatabase(dbSettings.Value.DatabaseName);

            _driverCollection = mongoDb.GetCollection<Driver>(dbSettings.Value.CollectionName);
        }

        public async Task<List<Driver>> GetDrivers()
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

        public async Task<Driver> GetDriverWithId(int number)
        {
            return await _driverCollection.Find(d => d.Number == number).FirstOrDefaultAsync();
        }

        
    }
}
