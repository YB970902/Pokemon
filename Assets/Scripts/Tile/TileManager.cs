using System.Collections;
using System.Collections.Generic;
using Define;
using UnityEngine;

/// <summary>
/// 맵에 생성되어 있는 타일을 관리하는 타일 매니저.
/// Vector2Int 타입으로 타일의 인덱스를 관리한다.
/// 좌표가 (0, 0) 인 경우 좌측 하단을 의미하고, x축이 증가하면 오른쪽으로 움직이고 y축이 증가하면 위로 움직인다.
/// </summary>
public class TileManager : MonoSingleton<TileManager>
{
    /// <summary> 맵의 크기 </summary>
    private Vector2Int mapSize = new Vector2Int();

    private List<Map.TileType> tileTypeList;
    private List<GameObject> mapObjects;

    /// <summary> 플레이어 소환 인덱스 </summary>
    public Vector2Int PlayerSpawnIndex { get; private set; }

    /// <summary> 타일의 크기 </summary>
    private float tileSize = 1f;

    private const int ZeroNumber = '0';

    protected override void Init()
    {
        base.Init();
        
        tileTypeList = new List<Map.TileType>();
        mapObjects = new List<GameObject>();
    }

    public void LoadMap(string _mapPath)
    {
        TextAsset textAsset = Resources.Load<TextAsset>(_mapPath);
        if (textAsset == null)
        {
            Debug.LogError($"맵 데이터가 없습니다.{_mapPath}");
        };
        
        string[] lines = textAsset.text.Split(System.Environment.NewLine);
        mapSize.x = lines[1].Length;
        mapSize.y = lines.Length - 1;

        tileTypeList.Clear();

        var firstLine = lines[0].Split(' ');
        PlayerSpawnIndex = new Vector2Int(int.Parse(firstLine[0]), int.Parse(firstLine[1]));

        for (int y = lines.Length - 1; y >= 1; --y)
        {
            var line = lines[y];
            for (int x = 0 ; x < line.Length; ++x)
            {
                var index = (mapSize.y - y) * line.Length + x;
                var tileID = line[x] - ZeroNumber;
                CreateTile(index, tileID);
                tileTypeList.Add((Map.TileType)tileID);
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
        
        string spriteName = ((Map.TileType)_tileID).ToString();

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
        return new Vector2Int(_index % mapSize.x, _index / mapSize.x);
    }

    public Vector2Int GetTileIndex(Vector2 _position)
    {
        return new Vector2Int((int)(_position.x / tileSize), (int)(_position.y / tileSize));
    }

    public Map.TileType GetTileType(int _index)
    {
        return tileTypeList[_index];
    }
    
    public Map.TileType GetTileType(Vector2Int _tileIndex)
    {
        return tileTypeList[_tileIndex.x + _tileIndex.y * mapSize.x];
    }

    public bool IsObstacle(Vector2Int _tileIndex)
    {
        return GetTileType(_tileIndex) == Map.TileType.Tree;
    }

    public bool IsBush(Vector2Int _tileIndex)
    {
        return GetTileType(_tileIndex) == Map.TileType.Bush;
    }
}