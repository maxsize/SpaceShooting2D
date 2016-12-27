using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System.Reflection;
using System;
using UnityEngine.Assertions;

public class ComponentAppender {
	private const string JSON_PATH = "Assets/Data/json/";
	public static void Append (Scene level)
	{
		Mapping mapping = GetJson(level.name + ".json");
		GameObject[] roots = level.GetRootGameObjects();
        for (int i = 0; i < roots.Length; i++)
        {
            var root = roots[i];
            var node = new Node(root.transform);

			RecursivelyAppend(node, mapping);
		}
	}

	private static void RecursivelyAppend(Node node, Mapping data)
	{
		FindMappingAndAppend(node, data);
		for (int i = 0; i < node.children.Count; i++)
		{
			RecursivelyAppend(node.children[i] as Node, data);
		}
	}

	private static void FindMappingAndAppend (Node node, Mapping data)
	{
		bool found = false;
		for (int i = 0; i < data.mappings.Length; i++)
		{
			var m = data.mappings[i];
			if (m.name == node.path)
			{
				found = true;
				DoAppend(node, m.components);
				break;
			}
		}
		Assert.IsTrue(found, "No mapping was found for node " + node.path);
	}

	private static void DoAppend (Node node, string[] components)
	{
		GameObject gameObject = node.host.gameObject;
		MethodInfo method = gameObject.GetType().GetMethod("AddComponent");
		for (int i = 0; i < components.Length; i++)
		{
			string className = components[i];
			Type type = Type.GetType(className);
			method.MakeGenericMethod(type);
			method.Invoke(gameObject, null);
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
	public string name;
	public string[] components;
}

[Serializable]
class Mapping
{
	public MappingData[] mappings;
}