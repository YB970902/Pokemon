using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    /// <summary> 현재 내가 서 있는 타일의 인덱스 </summary>
    private Vector2Int tileIndex;
    /// <summary> 이동할 다음 타일 인덱스 </summary>
    private Vector2Int nextTileIndex;

    /// <summary> 이동중인지 여부 </summary>
    private bool isMove;

    private float elaspedTime;
    private const float durationTime = 0.5f;
    
    private void Start()
    {
        transform.position = Vector2.zero;
        tileIndex = TileManager.Instance.GetTileIndex(transform.position);
        isMove = false;
        elaspedTime = 0.0f;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow) && isMove == false)
        {
            nextTileIndex = tileIndex;
            ++nextTileIndex.y;
            isMove = true;
        }

        if (isMove)
        {
            var tileManager = TileManager.Instance;

            elaspedTime += Time.deltaTime;
            var t = elaspedTime / durationTime;
            transform.position = Vector2.Lerp(tileManager.GetPosition(tileIndex), tileManager.GetPosition(nextTileIndex), t);
            if (t >= 1.0f)
            {
                isMove = false;
                tileIndex = nextTileIndex;
                elaspedTime = 0.0f;
            }
        }
    }
}
