using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBattle : UIBase
{
    [SerializeField] private Text txtMainText;
    
    public void SetText(string _text)
    {
        txtMainText.text = _text;
    }
}
