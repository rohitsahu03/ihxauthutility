using AuthUtility.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;

namespace AuthUtility.Common
{
    public class CachingHelper : ICachingHelper
    {
        private readonly IMemoryCache _memoryCache;
        private readonly ILogger _logger;
        public CachingHelper(IMemoryCache memoryCache, ILogger<CachingHelper> logger)
        {
            this._memoryCache = memoryCache;
            this._logger = logger;
        }

        public T GetCachedData<T>(string key)
        {
            if (string.IsNullOrEmpty(key))
                return default(T);
            try
            {
                return _memoryCache.Get<T>(key);
            }
            catch (Exception ex)
            {
                if (_logger != null)
                    _logger.LogError("Error in GetCachedRoles :" + ex.Message);
            }
            return default(T);
        }

        public void SetDataInCache<T>(string key, T Value)
        {
            try
            {
                if (_memoryCache.Get<T>(key) != null)
                    _memoryCache.Remove(key);

                _memoryCache.Set<T>(key, Value);
            }
            catch (Exception ex)
            {
                if (_logger != null)
                    _logger.LogError("Error in SetRolesInCache :" + ex.Message);
            }
        }

        public void RemoveCache(string key)
        {
            _memoryCache.Remove(key);
        }
    }
}