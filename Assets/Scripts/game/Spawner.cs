using UnityEngine;
using System.Collections;
using System;

public class Spawner : MonoBehaviour {

	public string templateName;
	public int spawnAmount = 2;
	public float spawnInterval = 0.8f;

	Vector3 cachedPosition;
	Quaternion cachedRotation;
	GameObject template;

	// Use this for initialization
	void Start () {
		template = Utils.Query(transform, templateName).gameObject;
		cachedPosition = template.transform.position;
		cachedRotation = template.transform.rotation;
		StartCoroutine("Spawn");
	}

	IEnumerator Spawn()
	{
		for (int i = 0; i < spawnAmount; i++)
		{
			yield return new WaitForSeconds(spawnInterval);
			GameObject copy = Instantiate(template, cachedPosition, cachedRotation) as GameObject;
			copy.transform.parent = template.transform.parent;
			copy.SetActive(true);
		}
		yield break;
	}
}
