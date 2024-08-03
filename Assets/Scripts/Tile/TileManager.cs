using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TileManager
{
    #region Singleton
    private static TileManager instance;

    public static TileManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new TileManager();
            }

            return instance;
        }
    }
    #endregion

    /// <summary> 맵의 크기 </summary>
    private Vector2Int mapSize = new Vector2Int(5, 5);

    /// <summary> 타일의 크기 </summary>
    private float tileSize = 1f;

    public void Init()
    {
        
    }

    public Vector2 GetPosition(Vector2Int _tileIndex)
    {
        return new Vector2(_tileIndex.x * tileSize, _tileIndex.y * tileSize);
    }

    public Vector2Int GetTileIndex(Vector2 _position)
    {
        return new Vector2Int((int)(_position.x / tileSize), (int)(_position.y / tileSize));
    }
}
