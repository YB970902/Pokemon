using System;
using System.Collections;
using System.Collections.Generic;
using Define;
using UnityEngine;

/// <summary>
/// 사용자의 입력을 받고 적절한 함수를 호출한다.
/// </summary>
public class PlayerInput : MonoBehaviour
{
    private Movement movement; 
    
    private void Start()
    {
        movement = GetComponent<Movement>();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            movement?.SetMoveDirection(Character.Direction.Up);
        }
        
        if (Input.GetKey(KeyCode.DownArrow))
        {
            movement?.SetMoveDirection(Character.Direction.Down);
        }
        
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            movement?.SetMoveDirection(Character.Direction.Left);
        }
        
        if (Input.GetKey(KeyCode.RightArrow))
        {
            movement?.SetMoveDirection(Character.Direction.Right);
        }
    }
}
