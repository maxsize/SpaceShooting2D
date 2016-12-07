using UnityEngine;
using System.Collections;

public class Waypoint : MonoBehaviour {

	public float radius = 0.1f;
	public Color color = Color.red;
	/// <summary>
	/// Callback to draw gizmos that are pickable and always drawn.
	/// </summary>
	void OnDrawGizmos()
	{
		Gizmos.color = color;
		Gizmos.DrawSphere(transform.position, radius);
	}
}
