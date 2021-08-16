using System.Collections.Generic;

namespace AuthUtility.Interfaces
{
    public interface ICachingHelper
    {        
        T GetCachedData<T>(string key);
        void SetDataInCache<T>(string key, T Value);
        void RemoveCache(string key);
    }
}
