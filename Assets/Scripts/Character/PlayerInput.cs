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
    [SerializeField] private PlayerController playerController;

    void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            playerController.Move(Character.Direction.Up);
        }
        
        if (Input.GetKey(KeyCode.DownArrow))
        {
            playerController.Move(Character.Direction.Down);
        }
        
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            playerController.Move(Character.Direction.Left);
        }
        
        if (Input.GetKey(KeyCode.RightArrow))
        {
            playerController.Move(Character.Direction.Right);
        }
    }
}
