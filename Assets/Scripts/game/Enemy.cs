using System;
using UnityEngine;

public class Enemy : MonoBehaviour, IExplodable
{
    public string enemyVOName;
    public string explosion;

    float _health;
    EnemyVO data;
    
    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        data = ArrayUtils.GetObjectByPrimaryKey<EnemyVO>(ref Metadata.Instance.enemies, "name", enemyVOName);
        _health = data.health;
    }

    float IExplodable.Health
    {
        get
        {
            return _health;
        }
    }

    void IExplodable.DealDamage(float damage)
    {
        _health -= damage;
        if (_health < 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Explode();
        gameObject.SetActive(false);
        Destroy(gameObject);
    }

    private void Explode()
    {
        // Instantiate(this, transform.position, Quaternion.identity);
        var explode = ObjectPool.current.GetObject(explosion);
        explode.transform.position = transform.position;
        explode.transform.rotation = Quaternion.identity;
    }
}