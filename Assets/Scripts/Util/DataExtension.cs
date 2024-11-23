using System.Collections;
using System.Collections.Generic;
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
}
