using CarPark.Business.Abstract;
using CarPark.DataAccess.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarPark.Business.Concrete
{
    public class PersonelManager : IPersonelService
    {
        private readonly IPersonelDataAccess _personelDataAccess;

        public PersonelManager(IPersonelDataAccess personelDataAccess)
        {
            _personelDataAccess = personelDataAccess;
        }
    }
}
