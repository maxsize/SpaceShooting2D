using UnityEngine;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.IO;

public class AssemblyUtils
{
    public static string[] GetAllFiles(string extention, string root, bool recursively = false)
    {
        DirectoryInfo scriptDir = new DirectoryInfo(root);
        string[] arr = GetFilesInDirectory(scriptDir, extention, recursively).ToArray();
        Debug.Log(ArrayUtils.Join(ref arr));
        return arr;
    }

    public static List<string> GetFilesInDirectory(DirectoryInfo dir, string extention, bool recursively = false)
    {
        List<string> all = new List<string>();
        FileInfo[] files = dir.GetFiles("*." + extention);
        for (int i = 0; i < files.Length; i++)
        {
            FileInfo file = files[i];
            all.Add(file.Name);
        }

        if (recursively)
        {
            DirectoryInfo[] dirs = dir.GetDirectories();
            for (int j = 0; j < dirs.Length; j++)
            {
                List<string> childFiles = GetFilesInDirectory(dirs[j], extention, recursively);
                all.AddRange(childFiles);
            }
        }
        return all;
    }

    public static List<string> GetAllDerviedTypes<T>(AppDomain domain, bool useFullName = false)
    {
        List<string> all = new List<string>();
        Assembly[] assem = domain.GetAssemblies();
        for (int i = 0; i < assem.Length; i++)
        {
            var types = assem[i].GetTypes();
            for (int j = 0; j < types.Length; j++)
            {
                var type = types[j];
                if (type.IsSubclassOf(typeof(T)))
                {
                    all.Add(useFullName ? type.FullName:type.Name);
                }
            }
        }
        return all;
    }
}