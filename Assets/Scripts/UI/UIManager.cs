using System;
using System.Collections;
using System.Collections.Generic;
using Define;
using UnityEngine;
using UnityEngine.UI;

/*
 *  1. UI 캐싱 (했음)
 *      - 한번 사용한 UI는 재사용될 가능성이 높음.
 *      - 사용후에 지우지 않고, 따로 보관했다가 필요할때 꺼내쓰면 좋음.
 *  2. UI 기본 기능
 *      - UI가 완전히 로드된 후 호출되는 함수 (했음)
 *      - 닫기 (했음)
 *      - UI의 타입 구분 (화면을 꽉 채우는 UI, 화면위로 표시되는 UI)
 */

public class UIManager : MonoSingleton<UIManager>
{
    [field: SerializeField]
    public CanvasScaler CommonScaler { get; private set; }
    [SerializeField] RectTransform rtSafeArea;
    [SerializeField] RectTransform rtCachedArea;

    private Dictionary<string, UIBase> cachedDict;
    private List<UIBase> uiList;

    private UIBase Peek => uiList.Count == 0 ? null : uiList[^1];

    protected override void Init()
    {
        base.Init();
        cachedDict = new Dictionary<string, UIBase>();
        uiList = new List<UIBase>();
    }

    public void Show<T>(string _address, out T _uiBase) where T : UIBase
    {
        if (cachedDict.ContainsKey(_address))
        {
            _uiBase = cachedDict[_address] as T;
            _uiBase.transform.SetParent(rtSafeArea, true);
            _uiBase.gameObject.SetActive(true);
        }
        else
        {
            var prefab = Resources.Load<GameObject>(_address);
            _uiBase = Instantiate(prefab, rtSafeArea).GetComponent<T>();
            cachedDict[_address] = _uiBase;
        }
        
        // 공통코드
        if (uiList.Count > 0 && _uiBase.UIType == UI.UIType.FullScreen)
        {
            foreach (UIBase ui in uiList)
            {
                ui.gameObject.SetActive(false);
            }
        }
        
        _uiBase.Show();
        uiList.Add(_uiBase);
    }

    public bool Close(UIBase _ui)
    {
        // 최상단에 있는 UI가 아니라면 닫지 않는다.
        if (_ui != Peek)
        {
            Debug.LogError($"잘못된 UI를 닫으려고 함. {_ui.name}");
            return false;
        }

        uiList.RemoveAt(uiList.Count - 1);
        _ui.gameObject.SetActive(false);
        _ui.transform.SetParent(rtCachedArea, true);

        if (_ui.UIType == UI.UIType.FullScreen)
        {
            for (int i = uiList.Count - 1; i >= 0; --i)
            {
                var ui = uiList[i];
                ui.gameObject.SetActive(true);
                if (ui.UIType == UI.UIType.FullScreen) break;
            }
        }

        return true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (uiList.Count == 0) return;
            
            if (Peek.BackKey())
            {
                Peek.Close();
            }
        }
        
        if (Input.GetKeyDown(KeyCode.Q))
        {
            UIFadeOut ui;
            Show("Prefabs/UI/Direction/UIFadeOut", out ui);
            ui.Set(2f, 1.5f);
        }
    }
}
