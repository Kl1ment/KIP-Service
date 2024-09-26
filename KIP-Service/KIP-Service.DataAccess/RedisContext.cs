using Microsoft.Extensions.Configuration;
using StackExchange.Redis;

namespace KIP_Service.DataAccess
{
    public class RedisContext
    {
        public ConnectionMultiplexer Connection { get; }

        public RedisContext(IConfiguration configuration)
        {
            Connection = ConnectionMultiplexer.Connect(configuration.GetConnectionString("Redis"));
        }
    }
}
