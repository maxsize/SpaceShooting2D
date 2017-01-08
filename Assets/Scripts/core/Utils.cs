using System;
using UnityEngine;

public class Utils
{
    ///<summary>
    /// Recursivly search through children of specified transform for specified name of child.
    ///</summary>
    public static Transform Query(Transform root, string nameToQuery)
    {
        var result = QueryInChildren(root, nameToQuery);
        if (result != null) return result;
        int count = root.childCount;
        for (int i = 0; i < count; i++)
        {
            result = Query(root.GetChild(i), nameToQuery);
            if (result != null) break;
        }
        return result;
    }

    private static Transform QueryInChildren(Transform root, string nameToQuery)
    {
        int count = root.childCount;
        Transform result = null;
        for (int i = 0; i < count; i++)
        {
            var current = root.GetChild(i);
            if (current.name == nameToQuery)
            {
                result = current;
                break;
            }
        }
        return result;
    }
}