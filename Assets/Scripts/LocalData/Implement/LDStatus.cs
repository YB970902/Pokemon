using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LDStatus : LocalDataBase
{
    public int Atk { get; set; }
    public float Def { get; set; }
    public string Name { get; set; }
    public List<int> Param { get; set; }
    public StatusType StatusType { get; set; }
}
