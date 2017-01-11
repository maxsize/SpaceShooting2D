//This is a modification of the original code found here: http://forum.unity3d.com/threads/simple-reusable-object-pool-help-limit-your-instantiations.76851/
//Remember kids, credit where credit is due!
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectPool : MonoBehaviour
{
    private static ObjectPool _current;         //A public static reference to itself (make's it visible to other objects without a reference)
    public PoolSettings[] settings;
    // public GameObject[] prefabs;				//Collection of prefabs to be poooled
    // public List<GameObject>[] pooledObjects;	//The actual collection of pooled objects
    // public int[] amountToBuffer;				//The amount to pool of each object. This is optional
    // public int defaultBufferAmount = 10;		//Default pooled amount if no amount abaove is supplied
    // public bool canGrow = true;					//Whether or not the pool can grow. Should be off for final builds

    // GameObject containerObject;					//A parent object for pooled objects to be nested under. Keeps the hierarchy clean

    public static ObjectPool current
    {
        get
        {
            if (_current == null)
            {
                _current = FindObjectOfType(typeof(ObjectPool)) as ObjectPool;
                Debug.Log("pool" + _current);
            }
            return _current;
        }
    }

	public void AddSetting(PoolSettings setting)
	{
		ArrayUtils.Push<PoolSettings>(ref settings, setting);
	}

    public void Initialize()
    {
        //For each prefab to be pooled...
        for (var i = 0; i < settings.Length; i++)
        {
            var setting = settings[i];
            int bufferAmount = setting.amountToBuffer;
            //...loop the correct number of times and in each iteration...
            for (int j = 0; j < bufferAmount; j++)
            {
                //...create the object...
                GameObject obj = (GameObject)Instantiate(setting.prefab);
                //...give it a name...
                obj.name = setting.prefab.name;
                //...and add it to the pool.
                PoolObject(obj);
            }
        }
    }

    public GameObject GetObject(string typeName)
    {
        //Loop through the collection of prefabs...
        for (int i = 0; i < settings.Length; i++)
        {
            //...to find the one of the correct type
            PoolSettings setting = settings[i];
            GameObject prefab = setting.prefab;
            if (prefab.name == typeName)
            {
                //If there are any left in the pool...
                if (setting.pooledObjects.Count > 0)
                {
                    //...get it...
                    GameObject pooledObject = setting.pooledObjects[0];
                    //...remove it from the pool...
                    setting.pooledObjects.RemoveAt(0);
                    return pooledObject;
                }
                //Otherwise, if the pool is allowed to grow...
                else if (setting.canGrow)
                {
                    //...write it to the log (so we know to adjust our values...
                    Debug.Log("pool grew when requesting: " + typeName + ". consider expanding default size.");
                    //...create a new one...
                    GameObject obj = Instantiate(prefab) as GameObject;
                    //...give it a name...
                    obj.name = prefab.name;
                    //...and return it.
                    return obj;
                }
                //If we found the correct collection but it is empty and can't grow, break out of the loop
                break;
            }
        }

        return null;
    }

    public void PoolObject(GameObject obj)
    {
        //Find the correct pool for the object to go in to
        for (int i = 0; i < settings.Length; i++)
        {
            PoolSettings setting = settings[i];
            if (setting.prefab.name == obj.name)
            {
                //Deactivate it...
                obj.SetActive(false);
                //..parent it to the container...
                // obj.transform.parent = containerObject.transform;
                //...and add it back to the pool
                setting.pooledObjects.Add(obj);
                return;
            }
        }
    }

}

[System.Serializable]
public class PoolSettings
{
    public GameObject prefab;
    public int amountToBuffer = 10;
    public bool canGrow;
    internal List<GameObject> pooledObjects;

    public static PoolSettings Create(GameObject prefab, int amountToBuffer, bool canGrow = false)
    {
        PoolSettings setting = new PoolSettings();
        setting.prefab = prefab;
        setting.amountToBuffer = amountToBuffer;
        setting.canGrow = canGrow;
        setting.pooledObjects = new List<GameObject>();
        return setting;
    }
}