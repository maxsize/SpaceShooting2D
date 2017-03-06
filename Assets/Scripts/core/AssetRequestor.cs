using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetRequestor
{
    private static Dictionary<AssetBundle, AssetRequestor> pool;
    private static MonoBehaviour runner;

    AssetBundle bundle;
    List<string> requestQueue;
    List<string> subRequestQueue;
    Dictionary<string, object[]> cache;   // cache of loaded assets, use name as key, [asset, allAssets]
    bool isBusy;
    public delegate void OnRequest(object asset);
    public delegate void OnSubRequest(object[] assets);
    OnRequest requestDelegate;
    OnSubRequest subRequestDelegate;

    public static void Initialize(MonoBehaviour runner)
    {
        AssetRequestor.runner = runner;
    }
    
    public static AssetRequestor Create(AssetBundle bundle)
    {
        if (pool == null) pool = new Dictionary<AssetBundle, AssetRequestor>();
        AssetRequestor requestor;
        if (pool.TryGetValue(bundle, out requestor)) return requestor;
        else
        {
            requestor = new AssetRequestor();
            requestor.bundle = bundle;
            pool.Add(bundle, requestor);
            return requestor;
        }
    }

    public static void Release(AssetBundle bundle, bool disposeLoadedAssets = false)
    {
        if (pool == null) return;
        AssetRequestor requestor;
        if (pool.TryGetValue(bundle, out requestor))
        {
            requestor.Dispose(disposeLoadedAssets);
            pool.Remove(bundle);
        }
    }

    public AssetRequestor()
    {
        requestQueue = new List<string>();
        subRequestQueue = new List<string>();
        cache = new Dictionary<string, object[]>();
    }

    /// <summary>
    /// Every loaded asset will be cached inside for furture use.
    /// It's your responsibility to dispose the requestor instance by calling AssetRequestor.Release()
    /// <para>
    /// assetName
    /// </para>
    /// </summary>
    public void RequestAssetAsync(string assetName)
    {
        requestQueue.Add(assetName);
        if (!isBusy) runner.StartCoroutine(Next());
    }

    public void RequestSubAssetAsync(string assetName)
    {
        subRequestQueue.Add(assetName);
        if (!isBusy) runner.StartCoroutine(Next());
    }

    public void AddListener(OnRequest listener)
    {
        if (requestDelegate == null) requestDelegate = new OnRequest(listener);
        else requestDelegate += listener;
    }

    public void AddListener(OnSubRequest listener)
    {
        if (subRequestDelegate == null) subRequestDelegate = new OnSubRequest(listener);
        else subRequestDelegate += listener;
    }

    public void RemoveListener(OnRequest listener)
    {
        if (requestDelegate != null) requestDelegate -= listener;
    }

    public void RemoveListener(OnSubRequest listener)
    {
        if (subRequestDelegate != null) subRequestDelegate -= listener;
    }

    IEnumerator Next()
    {
        while (requestQueue.Count + subRequestQueue.Count > 0)
        {
            isBusy = true;
            string assetName;
            AssetBundleRequest request;
            bool loadSingle = true;
            if (requestQueue.Count > 0)
            {
                assetName = requestQueue[0];
                requestQueue.RemoveAt(0);
            }
            else
            {
                loadSingle = false;
                assetName = subRequestQueue[0];
                subRequestQueue.RemoveAt(0);
            }
            
            var loaded = GetCache(assetName);
            if (loaded != null)
            {
                yield return null;
                if (loadSingle)
                    invokeCallback(requestDelegate, loaded[0]);
                else
                    invokeCallback(subRequestDelegate, loaded[1] as object[]);
            }
            else
            {
                request = loadSingle ? bundle.LoadAssetAsync(assetName) : bundle.LoadAssetWithSubAssetsAsync(assetName);
                yield return request;
                if (loadSingle) invokeCallback(requestDelegate, request.asset);
                else invokeCallback(subRequestDelegate, request.allAssets);
                cache[assetName] = new object[2]{request.asset, request.allAssets};
            }
            
        }
        isBusy = false;
        yield break;
    }

    void invokeCallback(Delegate dele, object value)
    {
        if (dele != null)
        {
            dele.DynamicInvoke(value);
        }
    }

    void invokeCallback(OnSubRequest dele, object[] value)
    {
        if (dele != null)
        {
            dele(value);
        }
    }

    object[] GetCache(string assetName)
    {
        object[] cached;
        if (cache.TryGetValue(assetName, out cached))
        {
            return cached;
        }
        return default(object[]);
    }

    void Dispose(bool disposeLoadedAssets)
    {
        bundle.Unload(disposeLoadedAssets);
        bundle = null;
    }
}
