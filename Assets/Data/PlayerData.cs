using UnityEngine;

[CreateAssetMenuAttribute(fileName = "Player", menuName = "Player")]
[System.SerializableAttribute]
public class PlayerData : ScriptableObject
{
    public float Health = 5f;
    public float X_Speed = 12f;
    public float Y_Speed = 12f;
    public float Max_Rotate = 25f;
}