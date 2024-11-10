using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoSingleton<UIManager>
{
    [field: SerializeField]
    public CanvasScaler CommonScaler { get; private set; }
    [SerializeField] RectTransform rtSafeArea;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            GameObject prefab = Resources.Load<GameObject>("Prefabs/UI/Direction/UIFadeOut");
            var ui = Instantiate(prefab, rtSafeArea).GetComponent<UIFadeOut>();
            ui.Set(2f, 1.5f);
        }
    }
}
