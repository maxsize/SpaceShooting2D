using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
    }

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        /*AssetBundle.LoadFromFile("Assets/AssetBundles/sprites");
        var bundle = AssetBundle.LoadFromFile("Assets/AssetBundles/level1");
        
        SceneManager.LoadScene("level1", LoadSceneMode.Additive);*/
        // StartCoroutine(BundleLoader.LoadBundle("http://localhost:8080/AssetBundles/AssetBundles"));
        AssetManager.Initialize (this);
        var signal = AssetManager.LoadBundle("http://localhost:8080/AssetBundles/", "level1");
        signal.Add<object>(OnBundleLoaded);
    }

    void OnBundleLoaded(object value)
    {
        AssetBundle bundle = value as AssetBundle;
        Debug.Log("Bundle loaded, requesting asset");
        SceneManager.LoadScene("level1", LoadSceneMode.Additive);
    }
}