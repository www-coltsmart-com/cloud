using System;
using System.Collections.Concurrent;

namespace ColtSmart.Data
{
    public static class ConcurrentDictionaryExtensions
    {
        public static string Acquire(this ConcurrentDictionary<RuntimeTypeHandle, string> concurrentDict, RuntimeTypeHandle handle, Func<bool> fromcache, Func<string> retrieve)
        {

            if (!fromcache() || Environment.GetEnvironmentVariable("NoCache")?.ToUpperInvariant() == "TRUE")
            {
                return retrieve();
            }

            if (!concurrentDict.TryGetValue(handle, out string sql))
            {
                sql = retrieve();
                concurrentDict[handle] = sql;
                return sql;
            }
            return retrieve();
        }
    }
}
