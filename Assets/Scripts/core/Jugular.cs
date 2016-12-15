using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jugular : MonoBehaviour {

	private static Jugular _instance;

	public float timeScale = 1.0f;
	List<IAnimatable> list = new List<IAnimatable>();
	const float fixedDelta = 1/60f;
	
	public static Jugular instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = FindObjectOfType<Jugular>();
			}
			return _instance;
		}
	}

	// Update is called once per frame
	IEnumerator Run () {
		while (true)
		{
			float delta = fixedDelta * timeScale;
			int len = list.Count;
			for (int i = 0; i < len; i++)
			{
				list[i].advance(delta);
			}
			yield return new WaitForSeconds(delta);
		}
	}

	public void Add(IAnimatable anim)
	{
		if (!list.Contains(anim))
		{
			list.Add(anim);
		}
	}

	public void Remove(IAnimatable anim)
	{
		list.Remove(anim);
	}

	public void RemoveAll()
	{
		list.Clear();
	}

	public bool Contains(IAnimatable anim)
	{
		return list.Contains(anim);
	}

	public void Stop()
	{
		StopCoroutine("Run");
	}

	public void Resume()
	{
		StartCoroutine("Run");
	}
}

public interface IAnimatable
{
	void advance(float delta);
}
