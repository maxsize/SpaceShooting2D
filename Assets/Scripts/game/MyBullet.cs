using UnityEngine;

public class MyBullet : MonoBehaviour
{
    public float lifeTime = 1f;
    public float speed = 10f;
    public float damage = 1f;
    Rigidbody2D rigi;
    
    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    void OnEnable()
    {
        rigi = GetComponent<Rigidbody2D>();
        rigi.velocity = transform.up.normalized * speed;

        Invoke("Recycle", lifeTime);
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
            explodable.DealDamage(damage);
            gameObject.SetActive(false);    // once hitted, should recycle myself
        }
    }

    void Recycle()
    {
        ObjectPool.current.PoolObject(gameObject);
        // Destroy(gameObject);
    }
}