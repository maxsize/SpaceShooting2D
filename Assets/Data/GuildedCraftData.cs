using UnityEngine;

[CreateAssetMenuAttribute(fileName = "GuildedCraft", menuName = "Guilded Craft")]
[System.SerializableAttribute]
public class GuildedCraftData : ScriptableObject
{
    public float TurnSpeed = 4f;
    public float Treshold = 0.2f;
}