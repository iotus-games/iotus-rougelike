using System.Collections.Generic;
using UnityEngine;

public class Library
{
    public static void RenameKey<TKey, TValue>(IDictionary<TKey, TValue> dic, TKey fromKey, TKey toKey)
    {
        var value = dic[fromKey];
        dic.Remove(fromKey);
        dic[toKey] = value;
    }

    public static TValue GetOrCreate<TKey, TValue>(IDictionary<TKey, TValue> dict, TKey key, TValue init)
    {
        if (!dict.TryGetValue(key, out TValue val))
        {
            val = init;
            dict.Add(key, val);
        }

        return val;
    }

    public static TValue GetOrCreate<TKey, TValue>(IDictionary<TKey, TValue> dict, TKey key)
        where TValue : new()
    {
        return GetOrCreate(dict, key, new TValue());
    }

    public static T GetOrAddComponent<T>(GameObject obj) where T : Component
    {
        var c = obj.GetComponent<T>();
        return c == null ? obj.AddComponent<T>() : c;
    }
}