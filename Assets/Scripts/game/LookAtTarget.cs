using UnityEngine;

public class LookAtTarget : MonoBehaviour
{
    public string TargetTag;

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        GameObject lookTarget;
        lookTarget = GameObject.FindWithTag(TargetTag);

        if (lookTarget)
        {
            Vector2 forward = lookTarget.transform.position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(forward, transform.TransformDirection(Vector3.back));
            transform.rotation = new Quaternion(0, 0, rotation.z, rotation.w);
        }
    }
}