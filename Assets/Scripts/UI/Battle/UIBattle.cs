using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBattle : UIBase
{
    [SerializeField] private Text txtMainText;

    [SerializeField] private GameObject objSkillUI;
    [SerializeField] private List<Text> txtSkills;
    [SerializeField] private List<GameObject> objArrows;

    public override void Show()
    {
        objSkillUI.SetActive(false);
        objArrows.ForEach(_ => _.SetActive(false));
    }

    public override bool BackKey()
    {
        return false;
    }

    public void SetText(string _text)
    {
        txtMainText.text = _text;
    }

    public void ShowMainText()
    {
        txtMainText.gameObject.SetActive(true);
    }

    public void HideMainText()
    {
        txtMainText.gameObject.SetActive(false);
    }

    /// <summary>
    /// 스킬을 고르는 UI를 보여준다.
    /// </summary>
    public void ShowSelectSkillUI(List<LDSkill> _ldSkills)
    {
        objSkillUI.SetActive(true);
        objArrows[0].SetActive(true);
        
        for(int i = 0; i < _ldSkills.Count; ++i)
        {
            LDSkill ldSkill = _ldSkills[i];
            txtSkills[i].text = Locale.Instance.Localize(ldSkill.Name);
        }
    }

    public void HideSelectSkillUI()
    {
        objSkillUI.SetActive(false);
        objArrows.ForEach(_ => _.SetActive(false));
    }

    /// <summary>
    /// UI에 화살표로 가리킬 스킬명의 인덱스를 보낸다.
    /// </summary>
    public void SetSkillIndex(int _index)
    {
        objArrows.ForEach((obj, index) => obj.SetActive(index == _index));
    }

    /// <summary>
    /// 스킬을 선택한다.
    /// </summary>
    public void SelectSkill(int _index)
    {
        objSkillUI.SetActive(false);
    }
}
