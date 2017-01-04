using UnityEngine;
using System.Collections;

public class PulseBulletEmitter : MonoBehaviour
{
    public float fireRate = 0.1f;
    public float fireGap = 1.5f;
    public int numBulletEachPulse = 10;
    public string bullet;

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
        while (true)
        {
            for (int i = 0; i < numBulletEachPulse; i++)
            {
                GameObject b = getBullet();
                if (b)
                {
                    b.transform.position = transform.position;
                    b.transform.rotation = transform.rotation;
                    b.SetActive(true);
                }
                yield return new WaitForSeconds(fireRate);
            }
            yield return new WaitForSeconds(fireGap);
        }
    }

    GameObject getBullet()
    {
        return ObjectPool.current.GetObject(bullet) as GameObject;
    }
}