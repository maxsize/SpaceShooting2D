using System;
using UnityEngine;

public class Enemy : MonoBehaviour, IExplodable
{
    public SimpleEnemyData data;
    public GameObject explosion;

    float _health;
    
    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    void OnEnable()
    {
        _health = data.Health;
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
        GameObject anim = Instantiate(explosion, transform.position, Quaternion.identity);
    }
}