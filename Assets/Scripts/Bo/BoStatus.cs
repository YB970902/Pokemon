using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 스테이터스 관리.
/// </summary>
public class BoStatus
{
    public int Atk { get; private set; }
    public int Def { get; private set; }
    public int MaxHp { get; private set; }
    public int CurrentHp { get; private set; }

    public bool IsDead => CurrentHp <= 0;
    //public bool IsDead { get { return CurrentHp <= 0; } }

    public BoStatus(LDStatus _ldStatus)
    {
        Atk = _ldStatus.Atk;
        Def = _ldStatus.Def;
        CurrentHp = _ldStatus.Hp;
        MaxHp = _ldStatus.Hp;
    }

    public void Damaged(int _atk)
    {
        CurrentHp -= DamageAmount(_atk);
    }

    public int DamageAmount(int _atk)
    {
        _atk -= Def;
        return Mathf.Max(_atk, 0);
    }

    public void Heal(int _hp)
    {
        CurrentHp += _hp;
        CurrentHp = Mathf.Min(CurrentHp, MaxHp);
    }

    public void HealFull()
    {
        CurrentHp = MaxHp;
    }
}
