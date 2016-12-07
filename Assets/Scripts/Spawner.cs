using UnityEngine;
using System.Collections;
using System;

public class Spawner : MonoBehaviour {

	public GameObject Template;
	public int SpawnAmount = 2;
	public float SpawnInterval = 0.8f;

	Vector3 cachedPosition;
	Quaternion cachedRotation;

	// Use this for initialization
	void Start () {
		cachedPosition = Template.transform.position;
		cachedRotation = Template.transform.rotation;
		StartCoroutine("Spawn");
	}

	IEnumerator Spawn()
	{
		for (int i = 0; i < SpawnAmount; i++)
		{
			yield return new WaitForSeconds(SpawnInterval);
			GameObject copy = Instantiate(Template, cachedPosition, cachedRotation) as GameObject;
			copy.SetActive(true);
		}
	}
}
