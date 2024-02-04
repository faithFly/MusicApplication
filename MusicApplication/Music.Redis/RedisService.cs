using Microsoft.Extensions.Configuration;
using StackExchange.Redis;

namespace Music.Redis;

public class RedisService 
{
    private readonly string connectionString;
    private ConnectionMultiplexer connection;

    public RedisService(string connectionString)
    {
        this.connectionString = connectionString;
    }

    public IDatabase GetDatabase()
    {
        if (connection == null || !connection.IsConnected)
        {
            Connect();
        }

        return connection.GetDatabase();
    }

    private void Connect()
    {
        connection = ConnectionMultiplexer.Connect(connectionString);

        // 在这里可以添加连接成功后的其他逻辑

        // 注册事件处理程序，以在连接断开时重新连接
        connection.ConnectionFailed += (sender, args) =>
        {
            Console.WriteLine($"Connection failed: {args.Exception}");
        };

        connection.ConnectionRestored += (sender, args) =>
        {
            Console.WriteLine("Connection restored");
        };
    }
}