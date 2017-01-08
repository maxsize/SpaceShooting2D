using UnityEngine;

public class Move : MonoBehaviour
{
    public string speed = "5";

    Rigidbody2D rigid;
    int speedInt;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        int.TryParse(speed, out speedInt);
        rigid = GetComponent<Rigidbody2D>();
        rigid.velocity = Vector2.up * speedInt;
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        rigid.velocity = rigid.transform.up * speedInt;
    }
}