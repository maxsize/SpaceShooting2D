using System;
using UnityEngine.Assertions;

public class ArrayUtils
{
    ///<summary>
    /// Remove item from specified index and return it.
    /// If index out of range will return null.
    ///</summary>
    public static T RemoveAt<T>(ref T[] arr, int index)
    {
        Assert.IsTrue (index >= 0 && index <= arr.Length, "Index " + index + " out of range.");
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
}