using AuthUtility.Constants;
using LinqToDB.Configuration;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace AuthUtility.MediBuddyDBFactory
{
    public class DBSettings : ILinqToDBSettings
    {
        private IConfiguration _configuration = null;
        public DBSettings(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public IEnumerable<IDataProviderSettings> DataProviders => Enumerable.Empty<IDataProviderSettings>();
        public string DefaultConfiguration => "SqlServer";
        public string DefaultDataProvider => "SqlServer";
        public IEnumerable<IConnectionStringSettings> ConnectionStrings
        {
            get
            {
                yield return
                    new ConnectionStringSettings
                    {
                        Name = DBConstants.MediAuth,
                        ProviderName = "SqlServer",
                        ConnectionString = bool.Parse(_configuration["AppSettings:IsStaging"]) == true ? _configuration["ConnectionString:MediAuthStaging"] : _configuration["ConnectionString:MediAuth"]
                    };
            }
        }
    }
    public class ConnectionStringSettings : IConnectionStringSettings
    {
        public string ConnectionString { get; set; }
        public string Name { get; set; }
        public string ProviderName { get; set; }
        public bool IsGlobal => false;
    }
}
