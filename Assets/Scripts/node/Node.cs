using UnityEngine;
using System.Collections.Generic;
using System;

public interface INode
{
    Node parent {get;}
    List<Node> children {get;}
    string name {get;}
    string path {get;}
    Transform host {get;}
    Node GetChildAt(int index);
}

public class Node : INode
{
    public Node parent
    {
        get { return _parent; }
        // set { _parent = value; }
    }

    public List<Node> children
    {
        get { return _children; }
    }

    public string name
    {
        get { return _host.name; }
    }

    public Transform host
    {
        get { return _host; }
    }

    public string path
    {
        get
        {
            if (_path == null)
            {
                _path = getPath();
            }
            return _path;
        }
    }

    public Node GetChildAt(int index)
    {
        return _children[index];
    }

    Transform _host;
    Node _parent;
    List<Node> _children;
    string _path;

    public Node(Transform host, Node parent = null)
    {
        this._host = host;
        this._parent = parent;
        init();
    }

    private void init()
    {
        _children = new List<Node>();

        for (int i = 0; i < _host.childCount; i++)
        {
            var trans = _host.GetChild(i);
            var node = new Node(trans, this);
            _children.Add(node);
        }

        _path = getPath();
    }

    string getPath()
    {
        string p = name;
        Node currentParent = _parent;
        while (currentParent != null)
        {
            p = currentParent.name + "." + p;
            currentParent = currentParent.parent;
        }
        return p;
    }

    override public string ToString()
    {
        JSONObject obj = new JSONObject();
        obj.name = name;
        List<string> list = new List<string>();
        for (int i = 0; i < children.Count; i++)
        {
            list.Add(children[i].ToString());
        }
        obj.children = list.ToArray();
        return JsonUtility.ToJson(obj);
    }
}

[Serializable]
class JSONObject
{
    public string name;
    public string[] children;
}