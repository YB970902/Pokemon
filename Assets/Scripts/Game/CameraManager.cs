using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CameraManager : MonoSingleton<CameraManager>
{
    private Camera camera;
    private Transform trPlayer;

    private Vector2Int canvasResolution;

    protected override void Init()
    {
        base.Init();
        camera = Camera.main;
    }

    private void Start()
    {
        Vector2 referenceResolution = UIManager.Instance.CommonScaler.referenceResolution;
        canvasResolution = new Vector2Int(Mathf.RoundToInt(referenceResolution.x), Mathf.RoundToInt(referenceResolution.y));
    }

    private void Update()
    {
        SetResolution();
    }

    public void SetPlayer(Transform _trPlayer)
    {
        trPlayer = _trPlayer;
    }

    private void SetResolution()
    {
        Vector2Int screenResolution = new Vector2Int(Screen.width, Screen.height);

        float canvasRatio = (float)canvasResolution.x / canvasResolution.y;
        float screenRatio = (float)screenResolution.x / screenResolution.y;

        if (canvasRatio < screenRatio)
        {
            // 실제 디바이스의 가로가 더 길다.
            Rect rc = camera.rect;
            rc.width = canvasRatio / screenRatio;
            rc.x = (1f - rc.width) * 0.5f;
            rc.height = 1f;
            rc.y = 0;
            camera.rect = rc;
            Screen.SetResolution(screenResolution.y, screenResolution.y, true);
        }
        else
        {
            // 실제 디바이스의 세로가 더 길다.
            Rect rc = camera.rect;
            rc.height = screenRatio / canvasRatio;
            rc.y = (1f - rc.height) * 0.5f;
            rc.width = 1f;
            rc.x = 0f;
            camera.rect = rc;
            Screen.SetResolution(screenResolution.x, screenResolution.x, true);
        }
    }

    private void LateUpdate()
    {
        var playerPos = trPlayer.position;
        playerPos.z = camera.transform.position.z;
        camera.transform.position = playerPos;
    }
}
