using System.Collections.Generic;
using UnityEngine.Assertions;
using System;
using UnityEngine;

public class Signal {

	public float id = UnityEngine.Random.value;
	List<Delegate> list = new List<Delegate>();
	List<Delegate> addOnces = new List<Delegate>();
	Type ParamType;
	public Signal()
	{
	}

	public Signal(Type ParamType)
	{
		this.ParamType = ParamType;
	}

	public void Add(Action listener)
	{
		AddListener(listener);
	}

	public void Add<T>(Action<T> listener)
	{
		AddListener(listener);
	}

	public void AddOnce(Action listener)
	{
		addOnces.Add(listener);
		Add(listener);
	}

	public void AddOnce<T>(Action<T> listener)
	{
		Debug.Log(string.Format("add once ---------- {0}", listener.ToString()));
		
		addOnces.Add(listener);
		Add<T>(listener);
	}

	public void Remove(Action listener)
	{
		list.Remove(listener);
	}

	public void Remove<T>(Action<T> listener)
	{
		list.Remove(listener);
	}

	public void RemoveAll()
	{
		list.Clear();
	}

	public void dispatch()
	{
		for (int i = 0; i < list.Count; i++)
		{
			Delegate dele = list[i];
			dele.DynamicInvoke();
			bool ao = IsAddOnce(dele);
			if (ao)
			{
				list.RemoveAt(i);
				addOnces.Remove(dele);
				i--;
			}
		}
	}

	public void dispatch<T>(T value)
	{
		Assert.IsNotNull(ParamType);
		
		#if Debug
			Debug.Log(value.GetType().Name + ", " + ParamType.Name);
		#endif

		bool valid = value == null || value.GetType() == ParamType;
		if (valid)
		{
			for (int i = 0; i < list.Count; i++)
			{
				Delegate dele = list[i];
				dele.DynamicInvoke(value);
				bool ao = IsAddOnce(dele);
				if (ao)
				{
					list.RemoveAt(i);
					addOnces.Remove(dele);
					i--;
				}
			}
		}
		else
		{
			throw new Exception("Type " + value.GetType().Name + " not matching " + ParamType.Name);
		}
	}

	bool IsAddOnce(Delegate dele)
	{
		return addOnces.Contains(dele);
	}

	void AddListener<T>(T listener)
	{
		list.Add(listener as Delegate);
	}
}
