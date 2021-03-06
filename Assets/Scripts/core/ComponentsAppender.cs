﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System.Reflection;
using System;
using UnityEngine.Assertions;

public class ComponentAppender {
	public const string JSON_PATH = "Assets/Data/json/";

	private static Mapping mapping;

	public static void Initialize(string levelName)
	{
		if (mapping == null)
			mapping = GetJson(levelName + ".json");
	}

	public static void Append (Scene level)
	{
		GameObject[] roots = level.GetRootGameObjects();
        for (int i = 0; i < roots.Length; i++)
        {
            var root = roots[i];
            var node = new Node(root.transform);

			RecursivelyAppend(node, mapping);
		}
	}

	public static void AppendOnPrefab(GameObject go)
	{
		var node = new Node(go.transform);
		FindMappingAndAppend(node, mapping);
	}

	private static void RecursivelyAppend(Node node, Mapping data)
	{
		FindMappingAndAppend(node, data);
		// Debug.Log(node.name);
		if (node.children != null && node.children.Count > 0)
		{
			for (int i = 0; i < node.children.Count; i++)
			{
				var child = node.GetChildAt(i);
				RecursivelyAppend(child, data);
			}
		}
	}

	private static void FindMappingAndAppend (Node node, Mapping data)
	{
		for (int i = 0; i < data.mappings.Length; i++)
		{
			var m = data.mappings[i];
			if (m.name == node.path)
			{
				DoAppend(node, m.components);
				break;
			}
		}
	}

	private static void DoAppend (Node node, ComponentData[] components)
	{
		GameObject gameObject = node.host.gameObject;
		// Type objType = gameObject.GetType();
		for (int i = 0; i < components.Length; i++)
		{
			string className = components[i].name;
			Type type = Type.GetType(className);
			var compIns = gameObject.AddComponent(type);
			SetParameters(compIns, components[i].parameters);
		}
	}

    private static void SetParameters(object compIns, KeyValue[] parameters)
    {
		if (parameters == null || parameters.Length <= 0) return;
		Type type = compIns.GetType();
		for (int i = 0; i < parameters.Length; i++)
		{
			var kv = parameters[i];
			var f = type.GetField(kv.name);
			if (f != null)
			{
				f.SetValue(compIns, kv.value);
			}
			var a = type.GetProperty(kv.name);
			if (a != null && a.CanWrite)
			{
				a.SetValue(compIns, kv.value, null);
			}
		}
    }

    private static Mapping GetJson (string name)
	{
		string jsonData = File.ReadAllText(JSON_PATH + name);
		return JsonUtility.FromJson<Mapping>(jsonData);
	}
}

[Serializable]
class MappingData
{
	public string name = "";
	public ComponentData[] components = null;
}

[Serializable]
class ComponentData
{
	public string name = "";
	public KeyValue[] parameters = null;
}

[Serializable]
class KeyValue
{
	public string name = "";
	public string value = "";
}

[Serializable]
class Mapping
{
	public MappingData[] mappings = null;
}