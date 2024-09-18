using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Define;
using DefineExtension;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerMovement movement;
    [SerializeField] private PlayerAnimation animation;

    private void Start()
    {
        movement.OnChangeDirection += animation.SetDirection;
        movement.OnChangeMoveState += animation.SetMoveState;
        movement.OnChangeMoveState += _isMove =>
        {
            if (_isMove == false) OnMoveEnd();
        };
    }

    public void Move(Character.Direction _direction)
    {
        if (movement.IsMove) return;

        movement.SetRotation(_direction);
        
        var nextTileIndex = movement.TileIndex + _direction.GetTileDirection();
        if (TileManager.Instance.IsObstacle(nextTileIndex)) return;
        
        movement.SetMoveDirection(_direction);
    }

    private void OnMoveEnd()
    {
        if (TileManager.Instance.IsBush(movement.TileIndex) == false) return;
        
        // 전투 로직
    }
}
