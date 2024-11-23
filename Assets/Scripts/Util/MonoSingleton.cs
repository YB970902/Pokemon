using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : Component
{
    public static bool IsInit { get; private set; } = false;
    
    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                // 이미 있으면 사용한다.
                instance = FindAnyObjectByType<T>();
                
                if (instance == null)
                {
                    instance = new GameObject().AddComponent<T>();
                }
            }

            return instance;
        }
    }

    protected virtual void Awake()
    {
        if (IsInit)
        {
            Destroy(gameObject);
            return;
        }
        
        DontDestroyOnLoad(gameObject);

        IsInit = true;
        Init();
    }

    /// <summary>
    /// 인스턴스가 생성된 후 1회 호출되는 초기화 함수
    /// </summary>
    protected virtual void Init()
    {
        
    }
}
