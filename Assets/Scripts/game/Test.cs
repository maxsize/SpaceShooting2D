using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class Test : MonoBehaviour
{
    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        // AssemblyUtils.GetAllFiles("cs", "Assets/Scripts/", true);
        /*var scene = SceneManager.GetActiveScene();
        GameObject[] roots = scene.GetRootGameObjects();
        for (int i = 0; i < roots.Length; i++)
        {
            var root = roots[i];
            var node = new Node(root.transform);
            Debug.Log(node.ToString());
        }*/
        //ComponentAppender.Append(SceneManager.GetActiveScene());
    }
}