using UnityEngine;
using System.Collections;

public class PulseBulletEmitter : MonoBehaviour
{
    public string emitterName;

    PulseEmitterVO emitter;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        emitter = ArrayUtils.GetObjectByPrimaryKey<PulseEmitterVO>(ref Metadata.Instance.pulseEmitters, "name", emitterName);
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
            for (int i = 0; i < emitter.numBulletEachPulse; i++)
            {
                GameObject b = getBullet();
                if (b)
                {
                    b.transform.position = transform.position;
                    b.transform.rotation = transform.rotation;
                    b.SetActive(true);
                }
                yield return new WaitForSeconds(emitter.fireRates[0].fireRate);
            }
            yield return new WaitForSeconds(emitter.fireGap);
        }
    }

    GameObject getBullet()
    {
        return ObjectPool.current.GetObject(emitter.bullet) as GameObject;
    }
}