using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalTimer : MonoBehaviour {
	static float _elapsed;

	public static float TimeElapsed
	{
		get
		{
			return _elapsed;
		}
	}

	public static void SetTime(float time)
	{
		_elapsed = time;
	}

	// Use this for initialization
	/// <summary>
	/// Awake is called when the script instance is being loaded.
	/// </summary>
	void Awake()
	{
		_elapsed = 0;
	}

	/// <summary>
	/// Update is called every frame, if the MonoBehaviour is enabled.
	/// </summary>
	void Update()
	{
		_elapsed += Time.deltaTime;
	}
}
