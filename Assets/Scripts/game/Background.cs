using UnityEngine;
using System.Collections;
using max.events;

public class Background : MonoBehaviour
{
    public string backgroundName;

    BackgroundVO vo;
    BgTile[] tiles;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        vo = ArrayUtils.GetObjectByPrimaryKey<BackgroundVO>(ref Metadata.Instance.backgrounds, "name", backgroundName);
        CreateTiles();
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        Vector3.MoveTowards(transform.position, Vector3.up, vo.scrollSpeed);
        CheckBounds();
    }

    void CreateTiles()
    {
        tiles = new BgTile[3];
        for (int i = 0; i < 3; i++)
        {
            GameObject go = new GameObject();
            BgTile tile = go.AddComponent<BgTile>();
            tile.spriteName = vo.bgs[i];
            tile.index = i;
            tile.Add(BgTile.EVENT_UPDATE, OnTileUpdate);
            go.transform.parent = transform;
            tiles[i] = tile;
        }
    }

    void OnTileUpdate(EEvent e)
    {
        Vector2 size = (Vector2) e.data;
        BgTile tile = (BgTile) e.target;
    }

    void CheckBounds()
    {
        BgTile tile = ArrayUtils.GetObjectByPrimaryKey<BgTile>(ref tiles, "index", "0");
        SpriteRenderer render = tile.gameObject.GetComponent<SpriteRenderer>();
        if (render == null || render.sprite == null) return;
        // Vector3 worldSize = render.sprite.bounds.size;
        // Debug.Log(string.Format("Tile screen size {0}, Screen.w {1}, Screen.h {2}", worldSize, Screen.width, Screen.height));
    }
}