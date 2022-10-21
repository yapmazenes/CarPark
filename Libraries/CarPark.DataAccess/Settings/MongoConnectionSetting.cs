using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarPark.DataAccess.Settings
{
    public class MongoConnectionSetting
    {
        public string ConnectionString { get; set; }
        public string Database { get; set; }
    }
}
