﻿using System.Collections.Generic;

namespace Grillisoft.Configuration
{
    public static class IValueStoreExtensions
    {
        public static string Get(this IValueStore store, string key)
        {
            return store.TryGetValue(key, out var ret) ? ret : throw new KeyNotFoundException($"Key '{key}' not found");
        }

        public static int GetInt32(this IValueStore store, string key)
        {
            return int.Parse(store.Get(key));
        }

        public static string Get(this IValueStore store, string key, string defaultValue)
        {
            return store.TryGetValue(key, out var ret) ? ret : defaultValue;
        }

        public static int GetInt32(this IValueStore store, string key, int defaultValue)
        {
            if (store.TryGetValue(key, out var value) && int.TryParse(value, out var ret))
                return ret;

            return  defaultValue;
        }
    }
}