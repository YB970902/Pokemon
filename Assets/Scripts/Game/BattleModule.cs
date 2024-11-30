using System;
using System.Collections;
using System.Collections.Generic;
using Define;
using UnityEngine;
using Camera = Define.Camera;
using Random = UnityEngine.Random;

public class BattleModule : MonoBehaviour, IModule
{
    [SerializeField] SpriteRenderer sprPlayerPokemon;
    [SerializeField] SpriteRenderer sprEnemyPokemon;
    [SerializeField] Animation animBattle;

    private AnimationPlayer animPlayer;

    // 애니메이션 키값들
    private const string AnimKeyBattleIntro = "BattleIntro";
    private const string AnimKeyPlayerAttack = "PlayerAttack";
    private const string AnimKeyEnemyAttack = "EnemyAttack";
    private const string AnimKeyPlayerDamaged = "PlayerDamaged";
    private const string AnimKeyEnemyDamaged = "EnemyDamaged";

    private List<string> attackToTargetAnimStream = new List<string>() { AnimKeyPlayerAttack, AnimKeyEnemyDamaged };
    private List<string> damagedAnimStream = new List<string>() { AnimKeyEnemyAttack, AnimKeyPlayerDamaged };
    
    public bool IsBattle { get; private set; }
    public Battle.BattleState CurrentState { get; private set; }

    private UIBattle uiBattle;

    private BoPokemon player;
    private BoPokemon enemy;

    /// <summary> 선택한 스킬의 인덱스 </summary>
    private int selectedSkillIndex;
    /// <summary> 적이 사용한 스킬의 인덱스 </summary>
    private int enemySkillIndex;

    private WaitForSeconds WFSDelay = new WaitForSeconds(1.5f);
    
    public void Init()
    {
        IsBattle = false;
        uiBattle = null;
        CurrentState = Battle.BattleState.None;

        animPlayer = new AnimationPlayer(animBattle);
        animPlayer.AddKey(AnimKeyBattleIntro);
        animPlayer.AddKey(AnimKeyPlayerAttack);
        animPlayer.AddKey(AnimKeyEnemyAttack);
        animPlayer.AddKey(AnimKeyPlayerDamaged);
        animPlayer.AddKey(AnimKeyEnemyDamaged);
    }

    public void BattleStart()
    {
        IsBattle = true;
        selectedSkillIndex = 0;

        CameraManager.Instance.SetCameraMode(Camera.CameraMode.Battle);
        UIManager.Instance.Show(UI.UIBattle, out uiBattle);
        animBattle.Stop();
        player = new BoPokemon(1);
        enemy = new BoPokemon(2);
        ChangeState(Battle.BattleState.Intro);
    }
    
    public void BattleEnd()
    {
        CameraManager.Instance.SetCameraMode(Camera.CameraMode.FollowPlayer);
        IsBattle = false;
        enemy.BoStatus.HealFull();
        player.BoStatus.HealFull();
        uiBattle.Close();
    }

    private void SelectSkill()
    {
        ChangeState(Battle.BattleState.SelectSkill);
    }

    private void AttackToTarget()
    {
        ChangeState(Battle.BattleState.Attack);
    }

    private void DamagedByEnemy()
    {
        ChangeState(Battle.BattleState.Damaged);
    }

    private void PlayerDead()
    {
        ChangeState(Battle.BattleState.PlayerDead);
    }

    private void EnemyDead()
    {
        ChangeState(Battle.BattleState.EnemyDead);
    }

    private void ChangeState(Battle.BattleState _state)
    {
        CurrentState = _state;
        
        switch (CurrentState)
        {
            case Battle.BattleState.Intro:
                animBattle.Play(AnimKeyBattleIntro);
                PrintDesc(CurrentState);
                StartCoroutine(CoBattleAnimation(animPlayer.GetWaitForSeconds(AnimKeyBattleIntro), SelectSkill));
                break;
            case Battle.BattleState.SelectSkill:
                uiBattle.HideMainText();
                uiBattle.ShowSelectSkillUI(player.LDSkills);
                break;
            case Battle.BattleState.Attack:
                StartCoroutine(CoBattleAnimationChain(attackToTargetAnimStream, AttackProcess));
                break;
            case Battle.BattleState.Damaged:
                StartCoroutine(CoBattleAnimationChain(damagedAnimStream, DamagedProcess));
                break;
            case Battle.BattleState.PlayerDead:
                Delay(PlayerDeadProcess);
                break;
            case Battle.BattleState.EnemyDead:
                Delay(EnemyDeadProcess);
                break;
        }
    }

    private void AttackProcess()
    {
        enemy.BoStatus.Damaged(player.GetSkillDamage(selectedSkillIndex, player.BoStatus.Atk));
        uiBattle.ShowMainText();
        PrintDesc(Battle.BattleState.Attack);
        if (enemy.BoStatus.IsDead)
        {
            EnemyDead();
        }
        else
        {
            Delay(DamagedByEnemy);
        }
    }

    private void DamagedProcess()
    {
        enemySkillIndex = Random.Range(0, 4);
        player.BoStatus.Damaged(enemy.GetSkillDamage(enemySkillIndex, enemy.BoStatus.Atk));
        PrintDesc(Battle.BattleState.Damaged);
        if (player.BoStatus.IsDead)
        {
            PlayerDead();
        }
        else
        {
            Delay(SelectSkill);
        }
    }

    private void PlayerDeadProcess()
    {
        PrintDesc(Battle.BattleState.PlayerDead);
        Delay(BattleEnd);
    }
    
    private void EnemyDeadProcess()
    {
        PrintDesc(Battle.BattleState.EnemyDead);
        Delay(BattleEnd);
    }

    private IEnumerator CoBattleAnimation(WaitForSeconds _wfs, Action _callback)
    {
        yield return _wfs;
        _callback?.Invoke();
    }
    
    private IEnumerator CoBattleAnimationChain(List<string> _animKeys, Action _callback)
    {
        int index = 0;
        
        while (index < _animKeys.Count)
        {
            string animKey = _animKeys[index++];
            animBattle.Play(animKey);
            yield return animPlayer.GetWaitForSeconds(animKey);
        }
        
        _callback?.Invoke();
    }

    private void Delay(Action _callback)
    {
        StartCoroutine(CoDelay());
        
        IEnumerator CoDelay()
        {
            yield return WFSDelay;
            _callback?.Invoke();
        }
    }

    private void PrintDesc(Battle.BattleState _state)
    {
        switch (_state)
        {
            case Battle.BattleState.Intro:
                uiBattle.SetText(Locale.Instance.Localize("Battle.Intro", enemy.Name));
                break;
            case Battle.BattleState.Attack:
            {
                string skillName = Locale.Instance.Localize(player.LDSkills[selectedSkillIndex].Name);
                int damageAmount = enemy.BoStatus.DamageAmount(player.GetSkillDamage(selectedSkillIndex, player.BoStatus.Atk));
                /* {0} : 공격 포켓몬 이름, {1} : 스킬명, {2} : 적 이름, {3} : 적이 받은 피해량 */
                uiBattle.SetText(Locale.Instance.Localize("Battle.Attack", player.Name, skillName, enemy.Name, damageAmount));
            }
                break;
            case Battle.BattleState.Damaged:
            {
                string skillName = Locale.Instance.Localize(enemy.LDSkills[enemySkillIndex].Name);
                int damageAmount = player.BoStatus.DamageAmount(enemy.GetSkillDamage(enemySkillIndex, enemy.BoStatus.Atk));
                uiBattle.SetText(Locale.Instance.Localize("Battle.Attack", enemy.Name, skillName, player.Name, damageAmount));
            }
                break;
            case Battle.BattleState.PlayerDead:
                uiBattle.SetText(Locale.Instance.Localize("Battle.PlayerDead", player.Name));
                break;
            case Battle.BattleState.EnemyDead:
                uiBattle.SetText(Locale.Instance.Localize("Battle.EnemyDead", enemy.Name));
                break;
        }
    }

    private void Update()
    {
        if (IsBattle == false) return;
        if (CurrentState != Battle.BattleState.SelectSkill) return;
        
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            selectedSkillIndex += 2;
            selectedSkillIndex %= 4;
            uiBattle.SetSkillIndex(selectedSkillIndex);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            selectedSkillIndex -= 1;
            if (selectedSkillIndex < 0) selectedSkillIndex += 4;
            uiBattle.SetSkillIndex(selectedSkillIndex);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            selectedSkillIndex += 1;
            selectedSkillIndex %= 4;
            uiBattle.SetSkillIndex(selectedSkillIndex);
        }
        
        if (Input.GetKeyDown(KeyCode.Z))
        {
            uiBattle.SelectSkill(selectedSkillIndex);
            uiBattle.HideSelectSkillUI();
            AttackToTarget();
        }
    }
}
