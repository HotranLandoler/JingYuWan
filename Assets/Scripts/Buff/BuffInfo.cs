using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Buff", menuName = "Buff")]
public class BuffInfo : ScriptableObject
{
    /// <summary>
    /// Buff Id
    /// </summary>
    public int Id;

    /// <summary>
    /// 图标
    /// </summary>
    public Sprite Icon;

    /// <summary>
    /// Buff名称
    /// </summary>
    public string Name;

    /// <summary>
    /// Buff持续时间
    /// </summary>
    public int Duration;

    /// <summary>
    /// 最大层数
    /// </summary>
    public int MaxLevel = 1;

    public ControlType controlType = ControlType.None;

    public BuffEffect[] buffEffects;

    public BuffConverter[] converters;

    public void OnAdded(Character character) 
    {
        
    }

    public void OnTick(Character character, int level) 
    {
        foreach (var effect in buffEffects)
        {
            if (effect.EffectType == BuffEffect.Type.Damage)
                character.CurrentHealth -= effect.Val1*level;
            //effect.EffectType switch
            //{
            //    BuffEffect.Type.Damage => Debug.Log(""),
            //    _ => null
            //};
        }
        
    }

    public void OnRemoved(Character character) { }

    public void OnBeHurt(DamageInfo info) { }

    

    [System.Serializable]
    public struct BuffEffect
    {
        public enum Type
        {
            Damage,
            /// <summary>
            /// 免控
            /// </summary>
            UnControllable,
            /// <summary>
            /// 霸体免推拉
            /// </summary>
            UnControllable_Strong,
            AddCritic,
            AddCriticDamage,
            AddHpPercent,
            ReduceDamage,
            AddDodge,
            ReduceMagicDamage,
            /// <summary>
            /// 缴械
            /// </summary>
            Disarm,
            /// <summary>
            /// 封内
            /// </summary>
            NoMagic,
            /// <summary>
            /// 沉默
            /// </summary>
            Silent,
        }

        [SerializeField]
        private Type type;
        public Type EffectType => type;

        [SerializeField]
        private float val1;
        public float Val1 => val1;
    }

    [System.Serializable]
    public struct BuffConverter
    {
        /// <summary>
        /// 转换为BUFF
        /// </summary>
        public BuffInfo convertTarget;
        /// <summary>
        /// 达到指定level时转换
        /// </summary>
        public int convertLevel;
    }
}

/// <summary>
/// 控制等级
/// </summary>
public enum ControlType
{
    None,
    /// <summary>
    /// 减速
    /// </summary>
    Slow,
    /// <summary>
    /// 锁足
    /// </summary>
    Stuck,
    /// <summary>
    /// 定身
    /// </summary>
    Freeze,
    /// <summary>
    /// 眩晕
    /// </summary>
    Daze,
    /// <summary>
    /// 击倒
    /// </summary>
    Down,
}