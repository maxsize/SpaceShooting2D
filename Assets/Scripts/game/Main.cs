using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

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
        MultiLookUp.AddLookUp("http://localhost:8080/");
        Metadata.Initialize();
        AssetManager.Initialize(this);
        var signal = AssetManager.LoadBundle("http://localhost:8080/AssetBundles/", "level1");
        signal.Add<object>(OnBundleLoaded);
    }

    void OnBundleLoaded(object value)
    {
        //assets/prefabs/emitterprefab.prefab
        //assets/prefabs/explosionprefab.prefab
        //assets/prefabs/guildedcraftprefab.prefab
        //assets/prefabs/mybulletprefab.prefab
        //assets/prefabs/player.prefab
        //assets/prefabs/pulsebulletprefab.prefab
        //assets/prefabs/pulseemitterprefab.prefab
        //assets/prefabs/waypoint.prefab

        AssetBundle bundle = AssetManager.GetBundle("http://localhost:8080/AssetBundles/", "prefabs");
        Debug.Log("Bundle loaded, requesting asset");
        
        StartCoroutine(LoadAndAppend(bundle));
    }

    private IEnumerator LoadAndAppend(AssetBundle bundle)
    {
        yield return StartCoroutine(LoadScene());
        Scene level1 = SceneManager.GetSceneByName("level1");

        ComponentAppender.Initialize(level1.name);
        var MyBullet = bundle.LoadAsset("assets/prefabs/mybulletprefab.prefab") as GameObject;
        var PulseBullet = bundle.LoadAsset("assets/prefabs/pulsebulletprefab.prefab") as GameObject;
        ComponentAppender.AppendOnPrefab(MyBullet);
        ComponentAppender.AppendOnPrefab(PulseBullet);

        Debug.Log("Asset loaded " + MyBullet);
        GameObject[] pools = ObjectPool.current.prefabs;
        ArrayUtils.Push(ref pools, MyBullet);
        ArrayUtils.Push(ref pools, PulseBullet);

        ComponentAppender.Append(level1);
    }

    private IEnumerator LoadScene()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync("level1", LoadSceneMode.Additive);
        while (operation == null)
        {
            yield return null;
        }
        while (!operation.isDone)
        {
            yield return null;
        }
        Debug.Log("Scene loaded");
        yield break;
    }
}