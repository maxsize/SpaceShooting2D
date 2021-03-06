using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine.Events;

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
        ManifestLoader loader = new ManifestLoader();
        loader.Initialize(rootUri);
        manifestMap.Add(rootUri, loader);
    }

    public static ManifestLoader LoadBundle(string root, string bundleName)
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
            Debug.LogError("Bundle " + bundleName + " is not loaded yet!");
            return null;
        }
        return loader.GetBundle(bundleName);
    }
}

public class ManifestLoader
{
    public ManifestLoaderEvent<AssetBundle> OnSuccess;
    public ManifestLoaderEvent<Exception> OnFail;

    private const string MAIN_BUNDLE_NAME = "AssetBundles";
    private AssetBundleManifest _manifestIns;
    private string root;
    private Dictionary<string, AssetBundle> bundleMap;
    private bool initialized = false;

    public ManifestLoader()
    {
        OnSuccess = new ManifestLoaderEvent<AssetBundle>();
        OnFail = new ManifestLoaderEvent<Exception>();
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
        yield return mono.StartCoroutine(LoadBundleOnly(bundleName));
        yield return mono.StartCoroutine(LoadDependencies(bundleName));
        AssetBundle bundle;
        bundleMap.TryGetValue(bundleName, out bundle);
        OnSuccess.Invoke(bundle);
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
            yield return mono.StartCoroutine(LoadBundleOnly(dep));
        }
        yield break;
    }

    private IEnumerator LoadManifest()
    {
        yield return mono.StartCoroutine(LoadBundleOnly(MAIN_BUNDLE_NAME));
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

    private IEnumerator LoadBundleOnly(string bundleName)
    {
        AssetBundle bundle;
        bundleMap.TryGetValue(bundleName, out bundle);
        if (bundle != null)
        {
            // if bundle already loaded, end coroutine after 1 frame.
            yield return null;
            yield break;
        }

        LookUp lu = MultiLookUp.Acquire(Path.Combine(root, bundleName));
        while (lu.Current != null)
        {
            UnityWebRequest www = UnityWebRequest.GetAssetBundle(lu.Current);
        
            yield return www.Send();

            if (www.isError || www.responseCode != (long)System.Net.HttpStatusCode.OK)
            {
                www.Dispose();
                Debug.Log("Failed to load bundle on " + lu.Current + ", looking for next lookup");
                bool hasNext = lu.Next();      // try next lookup
                if (!hasNext)
                {
                    OnFail.Invoke(new Exception($"Fail to load bundle {bundleName}"));
                    yield break;    // exit coroutine
                }
            }
            else
            {
                DownloadHandlerAssetBundle handler = www.downloadHandler as DownloadHandlerAssetBundle;
                bundle = handler.assetBundle;
                bundleMap.Add(bundleName, bundle);
                www.Dispose();
                Debug.Log("Bundle " + bundleName + " loaded.");
                yield break;
            }
        }

        OnFail.Invoke(new Exception($"Fail to load bundle {bundleName}"));
        yield break;
    }
}