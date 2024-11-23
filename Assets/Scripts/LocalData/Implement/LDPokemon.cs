using System.Collections;
using System.Collections.Generic;

public class LDPokemon : LocalDataBase
{
    public string Name { get; set; }
    public int StatusID { get; set; }
    public List<int> SkillIDList { get; set; }
}
