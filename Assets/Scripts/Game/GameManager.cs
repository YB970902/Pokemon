using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    public PlayerController Player { get; private set; }
    
    private void Start()
    {
        var tileManager = TileManager.Instance;
        
        tileManager.LoadMap("MapData/MapData");

        // SpawnPlayer
        var prefab = Resources.Load<GameObject>("Prefabs/Player");
        Player = Instantiate(prefab, tileManager.GetPosition(tileManager.PlayerSpawnIndex), Quaternion.identity).GetComponent<PlayerController>();
        
        CameraManager.Instance.SetPlayer(Player.transform);
    }
}