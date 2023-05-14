using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

[ApiController]
[Route("redis-data")]
public class RedisController : ControllerBase
{
    private readonly IDatabase _db;

    public RedisController(IConnectionMultiplexer redis)
    {
        _db = redis.GetDatabase();
    }

    [HttpGet]
    public IActionResult GetAllData()
    {
        var server = _db.Multiplexer.GetEndPoints().First();
        var keys = _db.Multiplexer.GetServer(server).Keys();

        var keyValueList = new List<KeyValuePair<string, string>>();

        foreach (var key in keys)
        {
            var value = _db.StringGet(key);
            keyValueList.Add(new KeyValuePair<string, string>(key.ToString(), value.ToString()));
        }

        return Ok(keyValueList);

    }
}