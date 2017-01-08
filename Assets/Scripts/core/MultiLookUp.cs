using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class MultiLookUp
{
    private static List<string> rootURIs = new List<string>();
    private static Dictionary<string, LookUp> dic = new Dictionary<string, LookUp>();
    private static int Count = 0;

    public static void AddLookUp(string uri)
    {
        ArrayUtils.AddUnique<string>(rootURIs, uri);
        Count = rootURIs.Count;
    }

    public static LookUp Acquire(string relativePath)
    {
        LookUp lookup;
        if (dic.TryGetValue(relativePath, out lookup))
        {
            return lookup;
        }
        lookup = CreateLookUp(relativePath);
        dic.Add(relativePath, lookup);
        return lookup;
    }

    private static LookUp CreateLookUp(string path)
    {
        var list = new List<string>();
        for (int i = 0; i < Count; i++)
        {
            list.Add(Path.Combine(rootURIs[i], path));
        }
        var lu = new LookUp(list.ToArray());
        return lu;
    }
}

public class LookUp
{
    string[] paths;
    int index = 0;

    public LookUp(string[] paths)
    {
        this.paths = paths;
    }

    public bool Next()
    {
        if (index < paths.Length - 1)
        {
            index++;
            return true;
        }
        index = -1;
        return false;
    }

    public string Current
    {
        get
        {
            if (index < 0)
                return null;
            return paths[index];
        }
    }
}