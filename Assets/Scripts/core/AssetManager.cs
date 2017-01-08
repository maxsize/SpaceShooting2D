using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

public class AssetManager
{
    internal static MonoBehaviour mono;

    private static Dictionary<string, ManifestLoader> manifestMap = new Dictionary<string, ManifestLoader>();

    public static void Initialize(MonoBehaviour mono)
    {
        AssetManager.mono = mono;
    }

    ///<summary>
    /// Load the main asset bundle to get manifest for all bundles
    ///</summary>
    private static void InitUri(string rootUri)
    {
        ManifestLoader loader = new ManifestLoader(typeof(AssetBundle));
        loader.Initialize(rootUri);
        manifestMap.Add(rootUri, loader);
    }

    public static Signal LoadBundle(string root, string bundleName)
    {
        ManifestLoader loader;
        manifestMap.TryGetValue(root, out loader);
        if (loader == null)
        {
            InitUri(root);
        }
        loader = manifestMap[root];
        loader.LoadBundle(bundleName);
        return loader;
    }

    public static AssetBundle GetBundle(string root, string bundleName)
    {
        ManifestLoader loader;
        manifestMap.TryGetValue(root, out loader);
        if (loader == null)
        {
            Debug.LogError("Bundle is not loaded yet!");
            return null;
        }
        return loader.GetBundle(bundleName);
    }
}

class ManifestLoader : Signal
{
    // public static string ASSET_LOADED = "assetLoaded";
    // public static string BUNDLE_LOADED = "bundleLoaded";

    private const string MAIN_BUNDLE_NAME = "AssetBundles";
    private AssetBundleManifest _manifestIns;
    private string root;
    private Dictionary<string, AssetBundle> bundleMap;
    private bool initialized = false;

    public ManifestLoader(Type ParamType) : base(ParamType)
    {
    }

    public AssetBundleManifest manifest
    {
        get { return _manifestIns; }
    }

    private MonoBehaviour mono
    {
        get { return AssetManager.mono; }
    }

    public void Initialize(string uri)
    {
        root = uri;
        bundleMap = new Dictionary<string, AssetBundle>();
        mono.StartCoroutine(LoadManifest());
    }

    public void LoadBundle(string bundleName)
    {
        mono.StartCoroutine(WaitAndLoad(bundleName));
    }

    public AssetBundle GetBundle(string bundleName)
    {
        AssetBundle bundle;
        bundleMap.TryGetValue(bundleName, out bundle);
        return bundle;
    }

    public void DisposeBundle(string bundleName, bool unloadAllLoaded = false)
    {
        AssetBundle bundle;
        bundleMap.TryGetValue(bundleName, out bundle);
        if (bundle)
        {
            bundle.Unload(unloadAllLoaded);
            bundleMap.Remove(bundleName);
            bundle = null;
        }
    }

    public void Dispose(bool unloadAllLoaded = false)
    {
        foreach (KeyValuePair<string, AssetBundle> item in bundleMap)
        {
            item.Value.Unload(unloadAllLoaded);
        }
        bundleMap.Clear();
        _manifestIns = null;
    }

    /// <summary>
    /// This function is called when the MonoBehaviour will be destroyed.
    /// </summary>
    void OnDestroy()
    {
        Dispose();
    }

    private IEnumerator WaitAndLoad(string bundleName)
    {
        yield return mono.StartCoroutine(WaitForInitialization());
        // load the target bundle and will not throw event unless all dependencies are all loaded
        yield return mono.StartCoroutine(LoadBundleOnly(bundleName, false));
        yield return mono.StartCoroutine(LoadDependencies(bundleName));
        AssetBundle bundle;
        bundleMap.TryGetValue(bundleName, out bundle);
        throwEvent(bundle);
        yield break;
    }

    private IEnumerator WaitForInitialization()
    {
        while (!initialized)
        {
            yield return null;
        }
        yield break;
    }

    private IEnumerator LoadDependencies(string bundleName)
    {
        var deps = manifest.GetAllDependencies(bundleName);
        while (deps.Length > 0)
        {
            var dep = ArrayUtils.RemoveAt(ref deps, 0);
            yield return mono.StartCoroutine(LoadBundleOnly(dep, false));
        }
        yield break;
    }

    private IEnumerator LoadManifest()
    {
        yield return mono.StartCoroutine(LoadBundleOnly(MAIN_BUNDLE_NAME, false));
        AssetBundle bundle = bundleMap[MAIN_BUNDLE_NAME];
        AssetBundleRequest request = bundle.LoadAssetAsync<AssetBundleManifest>("AssetBundleManifest");
        yield return request;

        if (request.asset == null)
        {
            Debug.LogError("Failed to load manifest");
            yield break;
        }

        _manifestIns = request.asset as AssetBundleManifest;
        initialized = true;
        Debug.Log(root + " initialized");
        yield break;
    }

    private IEnumerator LoadBundleOnly(string bundleName, bool throwEventAfterLoad = true)
    {
        AssetBundle bundle;
        bundleMap.TryGetValue(bundleName, out bundle);
        if (bundle != null)
        {
            // if bundle already loaded, end coroutine after 1 frame.
            yield return null;
            if (throwEventAfterLoad) throwEvent(bundleMap[bundleName]);
            yield break;
        }

        string fullPath = MultiLookUp.Acquire(Path.Combine(root, bundleName));
        if (fullPath == null)
        {
            throwEvent(new Exception("Failed to load bundle " + bundleName));
            yield break;
        }
        UnityWebRequest www = UnityWebRequest.GetAssetBundle(fullPath);
        
        yield return www.Send();

        if (www.isError)
        {
            Debug.LogError("Failed to load bundle");
            yield break;
        }

        DownloadHandlerAssetBundle handler = www.downloadHandler as DownloadHandlerAssetBundle;
        bundle = handler.assetBundle;
        bundleMap.Add(bundleName, bundle);
        www.Dispose();
        if (throwEventAfterLoad) throwEvent(bundleMap[bundleName]);
        Debug.Log("Bundle " + bundleName + " loaded.");
        yield break;
    }

    private void throwEvent(object data)
    {
        dispatch<object>(data);
    }
}