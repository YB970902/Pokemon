using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    #region Singleton

    private static TileManager instance;

    public static TileManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameObject().AddComponent<TileManager>();
            }

            return instance;
        }
    }

    #endregion

    /// <summary> 맵의 크기 </summary>
    private Vector2Int mapSize = new Vector2Int();

    private const string MapPath = "MapData/MapData";

    private List<int> mapDatas;
    private List<GameObject> mapObjects;

    /// <summary> 타일의 크기 </summary>
    private float tileSize = 1f;

    private void Awake()
    {
        mapDatas = new List<int>();
        mapObjects = new List<GameObject>();
        LoadMap(MapPath);
    }

    void LoadMap(string _mapPath)
    {
        TextAsset textAsset = Resources.Load<TextAsset>(_mapPath);
        if (textAsset == null)
        {
            Debug.LogError($"맵 데이터가 없습니다.{_mapPath}");
        };
        
        string[] lines = textAsset.text.Split("\n");
        mapSize.x = lines[0].Length;
        mapSize.y = lines.Length;

        mapDatas.Clear();

        for (int i = 0; i < lines.Length; ++i)
        {
            var line = lines[i];
            for (int j = 0 ; j < line.Length; ++j)
            {
                var tileID = int.Parse(line[j].ToString());
                CreateTile(i * line.Length + j, tileID);
                mapDatas.Add(tileID);
            }
        }
    }

    private void CreateTile(int _index, int _tileID)
    {
        Sprite[] tileSprites = Resources.LoadAll<Sprite>("Tile/Tile");
        if (tileSprites == null)
        {
            Debug.LogError($"타일이 없습니다 tilePath : {tileSprites}");
            return;
        }
        
        string spriteName = string.Empty;
        
        switch (_tileID)
        {
            case 0:
                spriteName = "Ground";
                break;
            case 1:
                spriteName = "Tree";
                break;
            case 2:
                spriteName = "Bush";
                break;
            default:
                Debug.LogError($"타일이 없습니다 tileID : {_tileID}");
                return;
        }

        Sprite sprite = null;
        foreach (var tile in tileSprites)
        {
            if (tile.name == spriteName)
            {
                sprite = tile;
                break;
            }
        }

        if (sprite == null)
        {
            Debug.LogError($"스프라이트가 없음. spriteName : {spriteName}");
            return;
        }
        
        var spriteRenderer = new GameObject().AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprite;
        spriteRenderer.transform.position = GetPosition(GetTileIndex(_index));
    }

    public Vector2 GetPosition(Vector2Int _tileIndex)
    {
        return new Vector2(_tileIndex.x * tileSize, _tileIndex.y * tileSize);
    }

    public Vector2Int GetTileIndex(int _index)
    {
        return new Vector2Int(_index % mapSize.x, mapSize.y - _index / mapSize.x);
    }

    public Vector2Int GetTileIndex(Vector2 _position)
    {
        return new Vector2Int((int)(_position.x / tileSize), (int)(_position.y / tileSize));
    }
}