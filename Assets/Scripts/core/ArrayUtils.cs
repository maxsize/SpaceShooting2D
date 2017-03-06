using System;
using UnityEngine.Assertions;
using UnityEngine;
using System.Collections.Generic;
using System.Reflection;

public class ArrayUtils
{
    ///<summary>
    /// Remove item from specified index and return it.
    /// If index out of range will return null.
    ///</summary>
    public static T RemoveAt<T>(ref T[] arr, int index)
    {
        AssertRange(ref arr, index);
        var item = arr[index];
        for (int a = index; a < arr.Length - 1; a++)
        {
            // moving elements downwards, to fill the gap at [index]
            arr[a] = arr[a + 1];
        }
        // finally, let's decrement Array's size by one
        Array.Resize(ref arr, arr.Length - 1);
        return item;
    }

    ///<summary>
    /// Add item into specified array in specified index
    ///</summary>
    public static void AddAt<T> (ref T[] arr, T item, int index)
    {
        AssertRange(ref arr, index);
        Array.Resize(ref arr, arr.Length + 1);
        T current = item;
        for (int i = index; i < arr.Length; i++)
        {
            var temp = arr[index];
            arr[index] = current;
            current = temp;
        }
    }

    public static void Push<T> (ref T[] arr, T item)
    {
        AddAt(ref arr, item, arr.Length);
    }

    public static void Unshift<T> (ref T[] arr, T item)
    {
        AddAt(ref arr, item, 0);
    }

    ///<summary>
    /// Find object by specified key and value, make sure item in the array contains specified key, otherwise it will throw error.
    ///</summary>
    public static T GetObjectByPrimaryKey<T>(ref T[] arr, string key, string value)
    {
        try
        {
            for (int i = 0; i < arr.Length; i++)
            {
                var current = arr[i];
                FieldInfo field = current.GetType().GetField(key);
                if (field != null && field.GetValue(current).ToString() == value)
                {
                    return current;
                }
                PropertyInfo property = current.GetType().GetProperty(key);
                if (property != null && property.GetValue(current, null).ToString() == value)
                {
                    return current;
                }
            }
        }
        catch(Exception e)
        {
            Debug.LogError(e.Message);
        }
        return default(T);
    }

    public static string Join<T>( ref T[] arr, string seperator = ", ")
    {
        string result = "";
        int minusOne = arr.Length - 1;
        for (int i = 0; i < arr.Length; i++)
        {
            result += arr[i].ToString();
            if (i < minusOne)
            {
                result += seperator;
            }
        }
        return result;
    }

    public static void AddUnique<T>(List<T> list, T toAdd)
    {
        var count = list.Count;
        bool isFound = false;
        for (int i = 0; i < count; i++)
        {
            if (list[i].Equals(toAdd))
            {
                isFound = true;
                break;
            }
        }
        if (!isFound)
        {
            list.Add(toAdd);
        }
    }

    private static void AssertRange<T> (ref T[] arr, int index)
    {
        Assert.IsTrue (index >= 0 && index <= arr.Length, "Index " + index + " out of range.");
    }
}