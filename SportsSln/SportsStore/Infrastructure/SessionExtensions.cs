using System.Text.Json;

namespace SportsStore.Infrastructure;

public static class SessionExtensions
{
    public static void SetJson<T>(this ISession session, string key, T obj)
    {
        session.SetString(key, JsonSerializer.Serialize<T>(obj));
    }

    public static T? GetJson<T>(this ISession session, string key)
    {
        var sessionData = session.GetString(key);
        return sessionData is null ? default(T) : JsonSerializer.Deserialize<T>(sessionData);
    }
}