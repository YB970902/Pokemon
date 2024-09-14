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
    private PlayerMovement playerMovement; 
    
    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            playerMovement?.SetMoveDirection(Character.Direction.Up);
        }
        
        if (Input.GetKey(KeyCode.DownArrow))
        {
            playerMovement?.SetMoveDirection(Character.Direction.Down);
        }
        
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            playerMovement?.SetMoveDirection(Character.Direction.Left);
        }
        
        if (Input.GetKey(KeyCode.RightArrow))
        {
            playerMovement?.SetMoveDirection(Character.Direction.Right);
        }
    }
}
