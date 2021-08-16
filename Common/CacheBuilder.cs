using AuthUtility.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Timers;

namespace AuthUtility.Common
{
    public class CacheBuilder
    {
        private static Timer aTimer = null;
        private static IServiceProvider _serviceProvider = null;
        public static void Init(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _serviceProvider.GetService<ICacheProvider>().RefreshCache();
            aTimer = new Timer(1000 * 60 * 1440); //minutes in milliseconds
            aTimer.Elapsed += new ElapsedEventHandler(RefreshCache);
            aTimer.AutoReset = true;
            aTimer.Start();
        }
        public static void RefreshCache(object source, ElapsedEventArgs e)
        {
            _serviceProvider.GetService<ICacheProvider>().RefreshCache();
        }
    }
}
