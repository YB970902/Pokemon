using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFadeOut : UIBase
{
    [SerializeField] Image imgFadeOut;
    
    private float elapsedTime;
    private float durationTime;
    private float directionTime;

    public override void Show()
    {
        base.Show();
        
        imgFadeOut.gameObject.SetActive(false);
    }

    public void Set(float _durationTime, float _directionTime)
    {
        imgFadeOut.gameObject.SetActive(true);
        imgFadeOut.fillAmount = 0f;
        elapsedTime = 0f;
        durationTime = _durationTime;
        directionTime = _directionTime;
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;
        imgFadeOut.fillAmount = elapsedTime / directionTime;
        
        if (elapsedTime >= durationTime)
        {
            Close();
        }
    }
}
