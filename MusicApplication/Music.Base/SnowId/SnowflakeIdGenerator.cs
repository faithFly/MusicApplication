using Snowflake.Core;

namespace Music.Base;

public static class SnowflakeIdGenerator
{
    public static long getSnowId()
    {
        var worker = new IdWorker(1, 1);
        return worker.NextId();
    }
}