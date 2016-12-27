using UnityEngine;
using System.Collections.Generic;
using System;

public interface INode
{
    INode parent {get;set;}
    List<INode> children {get;}
    string name {get;}
    string path {get;}
    Transform host {get;}
}

public class Node : INode
{
    public INode parent
    {
        get { return _parent; }
        set { _parent = value; }
    }

    public List<INode> children
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

    Transform _host;
    INode _parent;
    List<INode> _children;
    string _path;

    public Node(Transform host)
    {
        this._host = host;
        init();
    }

    private void init()
    {
        _children = new List<INode>();

        foreach (Transform trans in _host)
        {
            var node = new Node(trans);
            node.parent = this;
            _children.Add(node);
        }
    }

    string getPath()
    {
        string p = name;
        while (parent != null)
        {
            p = parent.name + "." + p;
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