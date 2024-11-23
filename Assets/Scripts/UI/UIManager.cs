using System;
using System.Collections;
using System.Collections.Generic;
using Define;
using UnityEngine;
using UnityEngine.UI;

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
            // 페이드 아웃 끝나고, 포켓몬 등장하는 로직. + UI등장
            void OnFadeOutEnd()
            {
                GameManager.Instance.BattleModule.BattleStart();
            }
            
            Show(UI.UIFadeOut, out UIFadeOut ui);
            ui.Set(2f, 1.5f, OnFadeOutEnd);
        }
    }
}
