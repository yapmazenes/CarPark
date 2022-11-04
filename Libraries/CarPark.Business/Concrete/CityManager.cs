using CarPark.Business.Abstract;
using CarPark.Core.Models;
using CarPark.DataAccess.Abstract;
using CarPark.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarPark.Business.Concrete
{
    public class CityManager : ICityService
    {
        private readonly ICityDataAccess _cityDataAccess;

        public CityManager(ICityDataAccess cityDataAccess)
        {
            _cityDataAccess = cityDataAccess;
        }

        public async Task<GetManyResult<City>> GetAllCitiesAsync()
        {
            return await _cityDataAccess.GetAllAsync();
        }
    }
}
