using StackExchange.Redis;

namespace Music.Redis;

public interface IRedisService
{
    IDatabase getConnect();
}