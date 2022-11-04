using CarPark.DataAccess.Abstract;
using CarPark.DataAccess.Repository;
using CarPark.DataAccess.Settings;
using CarPark.Entities.Concrete;
using Microsoft.Extensions.Options;

namespace CarPark.DataAccess.Concrete
{
    public class CityDataAccess : MongoRepositoryBase<City>, ICityDataAccess
    {
        public CityDataAccess(IOptions<MongoConnectionSetting> settings) : base(settings)
        {
        }
    }
}
