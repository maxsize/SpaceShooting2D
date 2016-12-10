using UnityEngine;

[CreateAssetMenuAttribute(fileName = "Enemy", menuName = "Simple Enemy")]
[System.SerializableAttribute]
public class SimpleEnemyData : ScriptableObject
{
    public float Health = 1f;
    public float speed = 5f;
    public float score = 10f;
}