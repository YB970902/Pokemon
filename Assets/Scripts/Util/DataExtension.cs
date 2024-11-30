using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class DataExtension
{
    public static bool IsNullOrEmpty<T>(this IList<T> _list)
    {
        return _list == null || _list.Count == 0;
    }

    public static bool IsNullOrEmpty(this string _str)
    {
        return string.IsNullOrEmpty(_str);
    }
    
    public static void ForEach<T>(this IList<T> _list, System.Action<T, int> _cb)
    {
        for (int i = 0; i < _list.Count; ++i)
        {
            _cb(_list[i], i);
        }
    }
}
