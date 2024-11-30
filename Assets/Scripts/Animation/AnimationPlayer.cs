using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 애니메이션 재생을 위한 플레이어.
/// 애니메이션의 정보를 가지고있다.
/// </summary>
public class AnimationPlayer
{
    /// <summary>
    /// 애니메이션의 정보를 가지고 있는 데이터 클래스.
    /// 외부에서 보일 필요가 없으므로 클래스 내에 private으로 정의.
    /// </summary>
    private class AnimationData
    {
        /// <summary> 애니메이션의 키 </summary>
        public string Key { get; private set; }
        /// <summary> 애니메이션의 플레이 시간 </summary>
        public float Time { get; private set; }
        /// <summary> 애니메이션의 플레이 시간만큼 코루틴에서 대기하도록 하는 객체 </summary>
        public WaitForSeconds WFS { get; private set; }

        public AnimationData(Animation _anim, string _key)
        {
            Key = _key;
            AnimationClip clip = _anim.GetClip(_key);
            Time =clip.length; 
            WFS = new WaitForSeconds(Time);
        }
    }
    
    private Animation anim;

    private Dictionary<string, AnimationData> animDataDict;
    
    public AnimationPlayer(Animation _animation)
    {
        anim = _animation;
        animDataDict = new Dictionary<string, AnimationData>();
    }

    /// <summary>
    /// 키를 등록한다.
    /// </summary>
    public void AddKey(string _key)
    {
        if (animDataDict.ContainsKey(_key))
        {
            Debug.LogWarning($"이미 정의된 키 입니다. key : {_key}");
            return;
        }
        animDataDict[_key] = new AnimationData(anim, _key);
    }

    /// <summary>
    /// 코루틴에 사용될 객체를 반환한다.
    /// </summary>
    public WaitForSeconds GetWaitForSeconds(string _key)
    {
        if (animDataDict.ContainsKey(_key) == false) return null;
        return animDataDict[_key].WFS;
    }
}
