using System;
using System.Collections;
using System.Collections.Generic;
using Define;
using DefineExtension;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    /// <summary> 현재 내가 서 있는 타일의 인덱스 </summary>
    public Vector2Int TileIndex { get; private set; }
    /// <summary> 이동할 다음 타일 인덱스 </summary>
    private Vector2Int nextTileIndex;
    
    /// <summary> 이동중인지 여부 </summary>
    public bool IsMove { get; private set; }
    public Character.Direction Direction { get; private set; }

    private float elapsedTime;
    private const float durationTime = 0.5f;

    public Action<bool> OnChangeMoveState { get; set; }
    public Action<Character.Direction> OnChangeDirection { get; set; }
    
    private void Start()
    {
        TileIndex = TileManager.Instance.GetTileIndex(transform.position);
        IsMove = false;
        Direction = Character.Direction.Down;
        elapsedTime = 0.0f;
    }

    public void SetMoveDirection(Character.Direction _direction)
    {
        OnChangeDirection?.Invoke(_direction);
        Direction = _direction;
        nextTileIndex = TileIndex;
        nextTileIndex += _direction.GetTileDirection();

        IsMove = true;
        OnChangeMoveState?.Invoke(true);
    }

    public void SetRotation(Character.Direction _direction)
    {
        nextTileIndex = TileIndex + _direction.GetTileDirection();
        OnChangeDirection?.Invoke(_direction);
    }

    void Update()
    {
        if (IsMove)
        {
            var tileManager = TileManager.Instance;

            elapsedTime += Time.deltaTime;
            var t = elapsedTime / durationTime;
            transform.position = Vector2.Lerp(tileManager.GetPosition(TileIndex), tileManager.GetPosition(nextTileIndex), t);
            if (t >= 1.0f)
            {
                OnChangeMoveState?.Invoke(false);
                IsMove = false;
                TileIndex = nextTileIndex;
                elapsedTime = 0.0f;
            }
        }
    }
}
