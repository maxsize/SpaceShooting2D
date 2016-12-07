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
    /// Sent when an incoming collider makes contact with this object's
    /// collider (2D physics only).
    /// </summary>
    /// <param name="other">The Collision2D data associated with this collision.</param>
    void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("Hitted " + other);
        IExplodable explodable = other.gameObject.GetComponent<IExplodable>();
        explodable.DealDamage(damage);
        gameObject.SetActive(false);    // once hitted, should recycle myself
    }

    void Recycle()
    {
        ObjectPool.current.PoolObject(gameObject);
        // Destroy(gameObject);
    }
}