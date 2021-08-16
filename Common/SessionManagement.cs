using AuthUtility.Constants;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace AuthUtility.Common {
    public static class SessionManagement {

        public static void Set<T> (this ISession session, T value) {
            session.SetString (UtilityConstants.UserSession, JsonConvert.SerializeObject (value));
        }

        public static T Get<T> (this ISession session) {
            var value = session.GetString (UtilityConstants.UserSession);

            return value == null ? default (T) :
                JsonConvert.DeserializeObject<T> (value);
        }

        public static void Clean (this ISession session) {
            session.Remove (UtilityConstants.UserSession);
        }
    }

}