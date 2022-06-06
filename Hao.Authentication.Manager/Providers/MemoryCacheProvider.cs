using Microsoft.Extensions.Caching.Memory;

namespace Hao.Authentication.Manager.Providers
{
    public class MemoryCacheProvider : ICacheProvider
    {
        private readonly IMemoryCache memoryCache;

        public MemoryCacheProvider(IMemoryCache memoryCache)
        {
            this.memoryCache = memoryCache;
        }

        public void Save(string key, object value, int hours = 2, int minutes = 0)
        {
            if (minutes <= 0) { minutes = 0; }
            if (hours <= 0 && minutes <= 0) { hours = 1; }
            var span = new TimeSpan(0, hours, minutes, 0);

            memoryCache.Set(key, value, span);
        }

        public T TryGetValue<T>(string key)
        {
            memoryCache.TryGetValue<T>(key, out T value);
            return value;
        }

        public bool Exist<T>(string key)
        {
            return memoryCache.TryGetValue<T>(key, out T value);
        }

        public void Remove(string key)
        {
            memoryCache.Remove(key);
        }
    }
}
