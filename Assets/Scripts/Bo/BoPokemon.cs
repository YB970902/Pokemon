using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoPokemon
{
    public LDPokemon LDPokemon { get; private set; }
    public BoStatus BoStatus { get; private set; }
    public List<LDSkill> LDSkills { get; private set; }

    public string Name => Locale.Instance.Localize(LDPokemon.Name);

    public BoPokemon(int _id)
    {
        LDPokemon = LocalDataManager.Instance.Pokemon[_id];
        BoStatus = new BoStatus(LocalDataManager.Instance.Status[LDPokemon.StatusID]);
        LDSkills = LDPokemon.SkillIDList.ConvertAll(_ => LocalDataManager.Instance.Skill[_]);
    }

    public int GetSkillDamage(int _skillIndex, int _atk)
    {
        return Mathf.RoundToInt(LDSkills[_skillIndex].Atk * _atk);
    }
}
