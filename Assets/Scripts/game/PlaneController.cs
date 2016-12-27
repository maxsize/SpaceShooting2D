using UnityEngine;
using System.Collections;
using System;

public class PlaneController : MonoBehaviour, IExplodable {

	public PlayerData playerData;
	Transform plane;
	float health;

    float IExplodable.Health
    {
        get
        {
			return health;
        }
    }

    void IExplodable.DealDamage(float damage)
    {
		health -= damage;
		if (health < 0)
		{
			Destroy(gameObject);
		}
    }

    // Use this for initialization
    void Start () {
		plane = transform;
		health = playerData.Health;

		Debug.Log(gameObject.name);
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
			rotate = Quaternion.AngleAxis(playerData.Max_Rotate, dir);
			move.x = Input.GetAxis("Horizontal") * playerData.X_Speed;
		}
		if (axisY != 0)
		{
			move.y = Input.GetAxis("Vertical") * playerData.Y_Speed;
		}
		Quaternion tempQua = Quaternion.Lerp(plane.rotation, rotate, Time.deltaTime * 5);
		plane.rotation = tempQua;
		plane.position += move * Time.deltaTime;
	}
}
