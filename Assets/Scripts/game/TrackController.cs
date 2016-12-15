using UnityEngine;
using System.Collections;
using System;

public class TrackController : MonoBehaviour {

	public GameObject WaypointsContainer;
	public float turnSpeed = 4;
	public float speed = 5;
	public float treashold = 0.1f;
	// Use this for initialization
	int currentWaypoint = 0;
	Rigidbody2D craft;
	Transform[] track;
	void Start () {
		track = GetWaypoints();
		craft = GetComponent<Rigidbody2D>();
		craft.velocity = (track[0].position - craft.transform.position).normalized * speed;
	}

    private Transform[] GetWaypoints()
    {
		Transform[] waypoints = WaypointsContainer.GetComponentsInChildren<Transform>();
		ArrayUtils.RemoveAt(ref waypoints, 0);
		return waypoints;
    }

    // Update is called once per frame
    void Update () {
		Transform target = track[currentWaypoint];
		Vector3 moveDir = target.position - craft.transform.position;
		if (moveDir.magnitude < treashold)
		{
			currentWaypoint++;
		}
		else
		{
			moveDir = moveDir.normalized * speed;
			craft.velocity = Vector3.Lerp(craft.velocity, moveDir, turnSpeed * Time.deltaTime);
			craft.transform.rotation = Quaternion.FromToRotation(Vector3.up, moveDir);
		}

		if (currentWaypoint == track.Length)
		{
			Destroy(gameObject);
		}
	}
}
