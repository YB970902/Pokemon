using System.Collections;
using System.Collections.Generic;

public class BoPokemon
{
    public LDPokemon LDPokemon { get; private set; }
    public LDStatus LDStatus { get; private set; }
    public List<LDSkill> LDSkills { get; private set; }

    public string Name => Locale.Instance.Localize(LDPokemon.Name);

    public BoPokemon(int _id)
    {
        LDPokemon = LocalDataManager.Instance.Pokemon[_id];
        LDStatus = LocalDataManager.Instance.Status[LDPokemon.StatusID];
        LDSkills = LDPokemon.SkillIDList.ConvertAll(_ => LocalDataManager.Instance.Skill[_]);
    }
}
