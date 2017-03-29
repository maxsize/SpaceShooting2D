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
        MultiLookUp.AddLookUp("http://localhost:8080/");
        Metadata.Initialize();
        AssetManager.Initialize(this);
        AssetRequestor.Initialize(this);
        var loader = AssetManager.LoadBundle("AssetBundles", "level1");
        loader.OnSuccess.AddListener(OnBundleLoaded);
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

        AssetBundle bundle = AssetManager.GetBundle("AssetBundles", "prefabs");
        
        StartCoroutine(LoadAndAppend(bundle));
    }

    private IEnumerator LoadAndAppend(AssetBundle bundle)
    {
        yield return StartCoroutine(LoadScene());
        Scene level1 = SceneManager.GetSceneByName("level1");

        ComponentAppender.Initialize(level1.name);
        var MyBullet = bundle.LoadAsset("assets/prefabs/mybulletprefab.prefab") as GameObject;
        var PulseBullet = bundle.LoadAsset("assets/prefabs/pulsebulletprefab.prefab") as GameObject;
        var Explotion = bundle.LoadAsset("assets/prefabs/explosionprefab.prefab") as GameObject;
        ComponentAppender.AppendOnPrefab(MyBullet);
        ComponentAppender.AppendOnPrefab(PulseBullet);
        ComponentAppender.AppendOnPrefab(Explotion);

        ObjectPool.current.AddSetting(PoolSettings.Create(MyBullet, 20, true));
        ObjectPool.current.AddSetting(PoolSettings.Create(PulseBullet, 10, true));
        ObjectPool.current.AddSetting(PoolSettings.Create(Explotion, 3, true));
        ObjectPool.current.Initialize();    // pre cache

        ComponentAppender.Append(level1);

        LoadBg();
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

    void LoadBg()
    {
        GameObject go = new GameObject();
        var bg = go.AddComponent<Background>();
        bg.backgroundName = "level1";
    }
}