using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiKeyDictionary<T1, T2, T3> : Dictionary<T1, Dictionary<T2, T3>>
{
    new public Dictionary<T2, T3> this[T1 key]
    {
        get
        {
            if (!ContainsKey(key))
                Add(key, new Dictionary<T2, T3>());

            Dictionary<T2, T3> returnObj;
            TryGetValue(key, out returnObj);

            return returnObj;
        }
    }

    public bool ContainsKey(T1 key1, T2 key2)
    {
        Dictionary<T2, T3> returnObj;
        TryGetValue(key1, out returnObj);
        if (returnObj == null)
            return false;

        return returnObj.ContainsKey(key2);
    }

    public bool TryGetValue(T1 key1, T2 key2, out T3 v)
    {
        Dictionary<T2, T3> dic;
        if (!TryGetValue(key1, out dic))
        {
            v = default(T3);
            return false;
        }

        return dic.TryGetValue(key2, out v);
    }

    public void Add(T1 key1, T2 key2, T3 v)
    {
        Dictionary<T2, T3> dic;
        if (TryGetValue(key1, out dic))
        {
            if (!dic.ContainsKey(key2))
            {
                dic.Add(key2, v);
            }
            else
            {
                Debug.LogError(" 存在 相同 的键值！！！");
            }
        }
        else
        {
            dic = new Dictionary<T2, T3>();
            dic.Add(key2, v);
            Add(key1, dic);
        }
    }

    public void Remove(T1 key1, T2 key2)
    {
        Dictionary<T2, T3> dic;
        if (TryGetValue(key1, out dic))
        {
            dic.Remove(key2);
            if (dic.Count <= 0)
            {
                this.Remove(key1);
            }
        }
    }

}