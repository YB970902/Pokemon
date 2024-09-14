using System;
using System.Collections;
using System.Collections.Generic;
using Define;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private PlayerMovement movement;

    private readonly int KeyIsWalk = Animator.StringToHash("IsWalk");
    private readonly int KeyDirection = Animator.StringToHash("Direction");

    private bool isMove;
    private Define.Character.Direction direction;
    
    private void Start()
    {
        isMove = false;
        direction = Character.Direction.Down;

        movement.OnChangeDirection += SetDirection;
        movement.OnChangeMoveState += SetMoveState;
    }

    private void SetDirection(Character.Direction _direction)
    {
        animator.SetInteger(KeyDirection, (int)_direction);
    }

    private void SetMoveState(bool _isMove)
    {
        animator.SetBool(KeyIsWalk, _isMove);
    }
}
