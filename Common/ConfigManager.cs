using Microsoft.Extensions.Configuration;

namespace AuthUtility.Common
{
    public class ConfigManager
    {
        private readonly IConfiguration _configuration;
        public ConfigManager(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        private string GetKey(string key)
        {

            var response = _configuration["AppSettings:" + key];
            if (response != null)
                return response;

            return string.Empty;
        }
        private string GetConnectionString(string key)
        {

            var response = _configuration["ConnectionString:" + key];
            if (response != null)
                return response;

            return string.Empty;
        }

        public string MediAuthUrl
        {
            get
            {
                return IsStaging ? GetKey("AuthenticationUrlStaging") : GetKey("AuthenticationUrl");
            }
        }
        public string ConnectionStringAuth
        {
            get
            {
                return IsStaging ? GetConnectionString("MediAuthStaging") : GetConnectionString("MediAuth");
            }
        }
        //public string KeyToSyncToElastic
        //{
        //    get
        //    {
        //        return GetKey("KeyToSyncToElastic");
        //    }
        //}
        public string UtilityUrl
        {
            get
            {
                return GetKey("UtilityUrl");
            }
        }
        //public string MediMarketAuthenticationKey
        //{
        //    get
        //    {
        //        return GetKey("IsStaging") == "1" ? GetKey("MediMarketAuthenticationKeyStaging") : GetKey("MediMarketAuthenticationKey");
        //    }
        //}
        //public string ProcheckAuthenticationKey
        //{
        //    get
        //    {
        //        return GetKey("IsStaging") == "1" ? GetKey("ProcheckAuthenticationKeyStaging") : GetKey("ProcheckAuthenticationKey");
        //    }
        //}

        //public List<string> AppBasedFields
        //{
        //    get
        //    {
        //        return GetKey("AppBasedFields").Split(",").ToList();
        //    }
        //}

        public bool IsStaging
        {
            get
            {
                bool.TryParse(GetKey("IsStaging"), out bool result);
                return result;
            }
        }

        //public string ElasticUrl
        //{
        //    get
        //    {
        //        return GetKey("ElasticUrl");
        //    }
        //}
        public string IHXSupremeUrl
        {
            get
            {
                return IsStaging ? GetKey("IHXSupremeUrlStaging") : GetKey("IHXSupremeUrl");
            }
        }

        public string AuthUtilityAuthenticationKey
        {
            get
            {
                return IsStaging ? GetKey("AuthUtilityAuthenticationKeyStaging") : GetKey("AuthUtilityAuthenticationKeyProduction");
            }
        }
    }
}