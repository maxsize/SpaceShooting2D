using UnityEngine;
using System.Collections;

public class CraftSpawner : MonoBehaviour {

	public Transform SpawnPosition;
	public GameObject Template;
	public int SpawnAmount = 5;

	int spawnCount = 0;
	Vector3 cachedPosition;
	Quaternion cachedRotation;
	// Use this for initialization
	void Start () {
		cachedPosition = SpawnPosition.position;
		cachedRotation = Template.transform.rotation;
		StartCoroutine(spawn());
	}

	IEnumerator spawn()
	{
		if (spawnCount >= SpawnAmount)
		{
			yield return null;
		}
		spawnCount++;
		Instantiate(Template, cachedPosition, cachedRotation);
		yield return new WaitForSeconds(0.6f);
	}
}
