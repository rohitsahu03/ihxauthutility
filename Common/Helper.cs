using AuthUtility.Constants;
using AuthUtility.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace AuthUtility.Common
{
    public class Helper
    {
        private Helper()
        {

        }
        private static Helper _helper;
        public static Helper Instance
        {
            get
            {
                if (_helper == null)
                    _helper = new Helper();

                return _helper;
            }
        }

        public string GetAppId(string LoginType)
        {
            string appId = null;
            switch (LoginType)
            {
                case "MA_Role":
                    appId = UtilityConstants.ProcheckAppId;
                    break;

                case "DC_Role":
                case "Insurer_Role":
                case "Corp_Role":
                    appId = UtilityConstants.MediMarketAppId;
                    break;

                default:
                    break;
            }
            return appId;
        }

        public Dictionary<string, string> JObjectToDictionary(JObject JsonObject)
        {
            if (JsonObject == null)
                return null;

            try
            {
                var value = JsonObject.ToObject<Dictionary<string, object>>()
                                   .Where(x => x.Value != null)
                                   .ToDictionary(x => x.Key, x => x.Value);
                Dictionary<string, string> response = new Dictionary<string, string>();
                foreach (var item in value)
                {
                    try
                    {
                        response.Add(item.Key, item.Value.ToString());
                    }
                    catch
                    {
                        response.Add(item.Key, JsonConvert.SerializeObject(item.Value));
                    }
                }
                return response;
            }
            catch
            {
                return null;
            }
        }
        public string GetAuthenticationKey(string AppId, ConfigManager Config)
        {
            string result = null;

            switch (AppId)
            {
                //case UtilityConstants.ProcheckAppId:
                //    result = Config.ProcheckAuthenticationKey;
                //    break;

                //case UtilityConstants.MediMarketAppId:
                //    result = Config.MediMarketAuthenticationKey;
                //    break;

                default:
                    break;
            }

            return result;
        }

        public int GetActionPerformedBy(HttpContext Context)
        {
            var session = Context.Session.Get<UserContext>();
            return session != null ? session.UserId : default(int);
        }
        public DateTime ParseStringToDateTime(string Date)
        {
            DateTime result = default(DateTime);
            try
            {
                result = DateTime.Parse(Date.Trim());
            }
            catch
            {
            }

            if (result != default(DateTime))
                return result;

            string[] formats = new string[]
            {
                "dd/MM/yyyy hh:mm:ss",
                "yyyy/MM/dd hh:mm:ss",
                "yyyy-MM-dd hh:mm:ss",
                "yyyy-dd-MM hh:mm:ss",
                "dd-MM-yyyy hh:mm:ss",
                "MM/dd/yyyy hh:mm:ss",
                "dd/MM/yyyy H:mm:ss",
                "yyyy/MM/dd H:mm:ss",
                "yyyy-MM-dd H:mm:ss",
                "yyyy-dd-MM H:mm:ss",
                "dd-MM-yyyy H:mm:ss",
                "MM/dd/yyyy H:mm:ss",
                "dd/MM/yyyy",
                "yyyy/MM/dd",
                "yyyy-MM-dd",
                "yyyy-dd-MM",
                "dd-MM-yyyy",
                "MM/dd/yyyy",
                "dd/MM/yyyy h:mm:ss tt",
                "yyyy/MM/dd h:mm:ss tt",
                "yyyy-MM-dd h:mm:ss tt",
                "yyyy-dd-MM h:mm:ss tt",
                "dd-MM-yyyy h:mm:ss tt",
                "MM/dd/yyyy h:mm:ss tt",
                "M/dd/yyyy h:mm:ss tt"
            };

            try
            {
                result = DateTime.ParseExact(Date.Trim(), formats, CultureInfo.InvariantCulture);
            }
            catch
            {
            }
            return result;
        }

        public string GetTrimmedValue(string Value)
        {
            return !string.IsNullOrEmpty(Value) ? Value.Trim() : default(string);
        }
    }
}
