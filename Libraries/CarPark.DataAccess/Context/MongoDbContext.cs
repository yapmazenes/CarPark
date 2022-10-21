using CarPark.DataAccess.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace CarPark.DataAccess.Context
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(IOptions<MongoConnection> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            _database = client.GetDatabase(settings.Value.Database);
        }

        public IMongoCollection<TEntity> GetCollection<TEntity>() => _database.GetCollection<TEntity>(typeof(TEntity).Name.Trim());

        public IMongoDatabase GetDatabase() => _database;
    }
}
