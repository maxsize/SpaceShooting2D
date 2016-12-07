using UnityEngine;
using System.Collections;

public class WaypointGizmos : MonoBehaviour {
	public float radius = 1;
	public Color color = Color.green;
	/// <summary>
	/// Callback to draw gizmos that are pickable and always drawn.
	/// </summary>
	void OnDrawGizmos()
	{
		Gizmos.color = color;
		Gizmos.DrawSphere(transform.position, radius);
	}
}
