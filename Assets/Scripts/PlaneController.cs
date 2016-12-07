using UnityEngine;
using System.Collections;

public class PlaneController : MonoBehaviour {

	public float max_rotate = 20f;
	public float x_speed = 5f;
	public float y_speed = 5f;
	Transform plane;

	// Use this for initialization
	void Start () {
		plane = transform;
	}
	
	// Update is called once per frame
	void Update () {
		float axisX = Input.GetAxisRaw("Horizontal");
		float axisY = Input.GetAxisRaw("Vertical");
		Quaternion rotate = Quaternion.identity;
		Vector3 move = Vector3.zero;
		if (axisX != 0)
		{
			Vector3 dir = new Vector3(0, -axisX, 0);
			rotate = Quaternion.AngleAxis(max_rotate, dir);
			move.x = Input.GetAxis("Horizontal") * x_speed;
		}
		if (axisY != 0)
		{
			move.y = Input.GetAxis("Vertical") * y_speed;
		}
		Quaternion tempQua = Quaternion.Lerp(plane.rotation, rotate, Time.deltaTime * 5);
		plane.rotation = tempQua;
		plane.position += move * Time.deltaTime;
	}
}
