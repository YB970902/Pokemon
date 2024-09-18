using System;
using System.Collections;
using System.Collections.Generic;
using Define;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private readonly int KeyIsWalk = Animator.StringToHash("IsWalk");
    private readonly int KeyDirection = Animator.StringToHash("Direction");

    public void SetDirection(Character.Direction _direction)
    {
        animator.SetInteger(KeyDirection, (int)_direction);
    }

    public void SetMoveState(bool _isMove)
    {
        animator.SetBool(KeyIsWalk, _isMove);
    }
}
