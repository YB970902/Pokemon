using System.Collections;
using System.Collections.Generic;
using Define;
using UnityEngine;

public class BattleModule : IModule
{
    public bool IsBattle { get; private set; }
    public Battle.BattleState CurrentState { get; private set; }

    private UIBattle uiBattle;

    private BoPokemon player;
    private BoPokemon enemy;
    
    public void Init()
    {
        IsBattle = false;
        uiBattle = null;
        CurrentState = Battle.BattleState.None;
    }

    public void BattleStart()
    {
        IsBattle = true;

        UIManager.Instance.Show(UI.UIBattle, out uiBattle);
        player = new BoPokemon(1);
        enemy = new BoPokemon(2);
        CurrentState = Battle.BattleState.Intro;
        PrintDesc(CurrentState);
    }

    public void BattleEnd()
    {
        IsBattle = false;
    }

    private void PrintDesc(Battle.BattleState _state)
    {
        switch (_state)
        {
            case Battle.BattleState.Intro:
                uiBattle.SetText(Locale.Instance.Localize("Battle.Intro", enemy.Name));
                break;
        }
    }

    public void Update()
    {
        if (IsBattle == false) return;
        
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            
        }
        
        if (Input.GetKeyDown(KeyCode.Z))
        {
            
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            
        }
    }
}
