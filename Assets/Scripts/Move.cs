using UnityEngine;

public class Move : MonoBehaviour
{
    public float speed = 5;

    Rigidbody2D rigid;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        rigid.velocity = Vector2.up * speed;
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        rigid.velocity = rigid.transform.up * speed;
    }
}