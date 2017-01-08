using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class MultiLookUp
{
    private static List<string> lookup = new List<string>();
    private static int Count = 0;

    public static void AddLookUp(string uri)
    {
        ArrayUtils.AddUnique<string>(lookup, uri);
        Count = lookup.Count;
    }

    public static bool RemoveLookUp(string uri)
    {
        bool founded = lookup.Remove(uri);
        Count = lookup.Count;
        return founded;
    }

    public static string Acquire(string relativePath)
    {
        string obsolutePath;
        for (int i = 0; i < Count; i++)
        {
            obsolutePath = Path.Combine(lookup[i], relativePath);
            if (File.Exists(obsolutePath))
            {
                return obsolutePath;
            }
        }
        return null;
    }
}