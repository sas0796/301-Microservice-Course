using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace MT.OnlineRestaurant.BusinessLayer
{
    public class CacheManager : ICache
    {
        private readonly IConfiguration _configuration;
        private ConnectionMultiplexer _connectionMultiplexer;

        public CacheManager(IConfiguration configuration)
        {
            _configuration = configuration;
        }



        private ConnectionMultiplexer Connection
        {
            get
            {
                if (_connectionMultiplexer == null)
                {
                    _connectionMultiplexer = ConnectionMultiplexer.Connect(_configuration.GetConnectionString("RedisCacheConnectionString"));
                }


                return _connectionMultiplexer;
            }
        }

        public IDatabase Database
        {
            get
            {
                return Connection.GetDatabase();
            }
        }

        public void SetInCache<T>(string key, T data)
        {
            string json = JsonConvert.SerializeObject(data);
            Database.StringSet(key, json);
        }

        public T GetFromCache<T>(string key)
        {
            string json = Database.StringGet(key);
            if (!string.IsNullOrEmpty(json))
            {
                return JsonConvert.DeserializeObject<T>(json);
            }
            return default(T);
        }

        public void InValidateCache(string key)
        {
            Database.KeyDelete(key);
        }

    }
}
   
