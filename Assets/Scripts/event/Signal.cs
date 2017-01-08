using System.Collections.Generic;
using UnityEngine.Assertions;
using System;
using UnityEngine;

public class Signal {

	List<Delegate> list = new List<Delegate>();
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
			list[i].DynamicInvoke();
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
				list[i].DynamicInvoke(value);
			}
		}
		else
		{
			throw new Exception("Type " + value.GetType().Name + " not matching " + ParamType.Name);
		}
	}

	void AddListener<T>(T listener)
	{
		list.Add(listener as Delegate);
	}
}
