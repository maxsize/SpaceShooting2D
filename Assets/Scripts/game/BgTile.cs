using UnityEngine;
using System.Collections;
using max.events;
using System;

public class BgTile : EventDispatcher
{
    public const string EVENT_UPDATE = "update";

    int _index;
    string _spriteName;
    AssetRequestor requestor;
    public string spriteName
    {
        get { return _spriteName; }
        set { SetSprite(value); }
    }

    public int index
    {
        get { return _index; }
        set { _index = value; }
    }

    void SetSprite(string spriteName)
    {
        this._spriteName = spriteName;
        CreateRenderer();
        LoadSprite();
    }

    void LoadSprite()
    {
        AssetBundle bundle = AssetManager.GetBundle("AssetBundles", "sprites");
        requestor = AssetRequestor.Create(bundle);
        requestor.RequestSubAssetAsync("assets/sprites/bg.png");
        /*requestor.OnLoad.Add<object[]>(
            OnLoaded
        );*/
        requestor.AddListener(OnLoaded);
    }

    void OnLoaded(object[] allAssets)
    {
        requestor.RemoveListener(OnLoaded);
        Debug.Log(string.Format("sprite loaded{0}", _spriteName));
        SpriteRenderer render = gameObject.GetComponent<SpriteRenderer>();
        var sprite = ArrayUtils.GetObjectByPrimaryKey(ref allAssets, "name", _spriteName);
        render.sprite = sprite as Sprite;

        transform.localScale = Vector3.one;
        var width = render.sprite.bounds.size.x;
        var height = render.sprite.bounds.size.y;
        var screenHeight = Camera.main.orthographicSize * 2.0;
        var screenWidth = screenHeight / Screen.height * Screen.width;
        var scale = transform.localScale;
        scale.x = (float) screenWidth / width;
        scale.y = (float) screenHeight / height;
        transform.localScale = scale;
        Dispatch(new EEvent(EVENT_UPDATE, false, render.sprite.rect.size)); 
    }

    void CreateRenderer()
    {
        SpriteRenderer renderer = gameObject.GetComponent<SpriteRenderer>();
        if (renderer == null)
            gameObject.AddComponent<SpriteRenderer>();
    }

    public void Dispose()
    {
        SpriteRenderer renderer = gameObject.GetComponent<SpriteRenderer>();
        renderer.sprite = null;
        requestor = null;
    }
}