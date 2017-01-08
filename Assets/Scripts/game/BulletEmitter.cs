using UnityEngine;
using System.Collections;

public class BulletEmitter : MonoBehaviour
{
    PlayerEmitterVO playerEmitter;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        playerEmitter = Metadata.Instance.playerEmitter;
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
            FireRateVO rate = playerEmitter.fireRates[0];
            GameObject newBullet = getBullet();
            if (newBullet)
            {
                newBullet.transform.position = transform.position;
                newBullet.transform.rotation = transform.rotation;
                newBullet.SetActive(true);
                // newBullet.transform.parent = transform.parent;
            }
            yield return new WaitForSeconds(rate.fireRate);
        }
    }

    GameObject getBullet()
    {
        return ObjectPool.current.GetObject(playerEmitter.bullet);
        // return Instantiate(bullet, transform.position, transform.rotation) as GameObject;
    }
}