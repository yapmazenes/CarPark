using CarPark.DataAccess.Abstract;
using CarPark.DataAccess.Context;
using CarPark.DataAccess.Repository;
using CarPark.DataAccess.Settings;
using CarPark.Entities.Concrete;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarPark.DataAccess.Concrete
{
    public class PersonelDataAccess : MongoRepositoryBase<Personel>, IPersonelDataAccess
    {
        private readonly MongoDbContext _mongoDbContext;
        private readonly IMongoCollection<Personel> _collection;

        public PersonelDataAccess(IOptions<MongoConnectionSetting> settings) : base(settings)
        {
            _mongoDbContext = new MongoDbContext(settings);
            _collection = _mongoDbContext.GetCollection<Personel>();
        }
    }
}
