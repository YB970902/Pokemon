using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Define;

public class UIBase : MonoBehaviour
{
    [field: SerializeField]
    public UI.UIType UIType { get; set; }

    protected System.Action cbClose;

    /// <summary>
    /// UI가 켜지는 시점
    /// </summary>
    public virtual void Show()
    {
        
    }

    public virtual bool BackKey()
    {
        return true;
    }

    public void Close()
    {
        if (UIManager.Instance.Close(this))
        {
            cbClose?.Invoke();
            cbClose = null;
        }
    }
}
