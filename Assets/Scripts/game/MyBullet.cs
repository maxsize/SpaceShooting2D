using UnityEngine;

public class MyBullet : MonoBehaviour
{
    Rigidbody2D rigi;
    PlayerEmitterVO emitter;
    FireRateVO rate;
    
    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    void OnEnable()
    {
        emitter = Metadata.Instance.playerEmitter;
        rate = emitter.fireRates[0];

        rigi = GetComponent<Rigidbody2D>();
        rigi.velocity = transform.up.normalized * rate.speed;

        Invoke("Recycle", emitter.lifeTime);
    }

    /// <summary>
    /// This function is called when the behaviour becomes disabled or inactive.
    /// </summary>
    void OnDisable()
    {
        CancelInvoke("Recycle");
    }

    /// <summary>
    /// Sent when another object enters a trigger collider attached to this
    /// object (2D physics only).
    /// </summary>
    /// <param name="other">The other Collider2D involved in this collision.</param>
    void OnTriggerEnter2D(Collider2D other)
    {
        IExplodable explodable = other.gameObject.GetComponent<IExplodable>();
        if (explodable != null)
        {
            explodable.DealDamage(rate.damage);
            gameObject.SetActive(false);    // once hitted, should recycle myself
        }
    }

    void Recycle()
    {
        ObjectPool.current.PoolObject(gameObject);
        // Destroy(gameObject);
    }
}