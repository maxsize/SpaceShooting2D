using UnityEngine;
using System.Collections;

public class BulletEmitter : MonoBehaviour
{
    public float fireRate = 0.1f;
    public string bullet;   // bullet prefab

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        StartCoroutine("fire");
    }

    /// <summary>
    /// This function is called when the behaviour becomes disabled or inactive.
    /// </summary>
    void OnDisable()
    {
        StopCoroutine("fire");
    }

    IEnumerator fire()
    {
        while(true)
        {
            GameObject newBullet = getBullet();
            if (newBullet)
            {
                newBullet.transform.position = transform.position;
                newBullet.transform.rotation = transform.rotation;
                newBullet.SetActive(true);
                // newBullet.transform.parent = transform.parent;
            }
            yield return new WaitForSeconds(fireRate);
        }
    }

    GameObject getBullet()
    {
        return ObjectPool.current.GetObject(bullet);
        // return Instantiate(bullet, transform.position, transform.rotation) as GameObject;
    }
}