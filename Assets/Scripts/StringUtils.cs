using UnityEngine;
using System;

public class StringUtils
{
    public static char[] SASH_SPLITER = {'/'};
    public static char[] DOT_SPLITER = {'.'};
    public static string GetFileName(string path)
    {
        var arr = path.Split(SASH_SPLITER);
        return arr[arr.Length - 1];
    }

    public static string GetExtention(string path)
    {
        string fileName = GetFileName(path);
        var arr = fileName.Split(DOT_SPLITER);
        return arr[arr.Length];
    }

    public static string GetFileNameWithoutExt(string path)
    {
        string fullName = GetFileName(path);
        var arr = fullName.Split(DOT_SPLITER);
        return arr[0];
    }
}