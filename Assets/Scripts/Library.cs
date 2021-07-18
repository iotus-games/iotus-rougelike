using System.Collections.Generic;

public class Library
{
    public static void RenameKey<TKey, TValue>(IDictionary<TKey, TValue> dic, TKey fromKey, TKey toKey)
    {
        var value = dic[fromKey];
        dic.Remove(fromKey);
        dic[toKey] = value;
    }   
}
