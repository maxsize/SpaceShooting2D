using UnityEngine;

public class RecycleAnim : MonoBehaviour
{
    public void DestroyGameObject()
    {
        ObjectPool.current.PoolObject(gameObject);
    }
}