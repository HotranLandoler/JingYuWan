using JYW.UI;
using JYW.UI.ToolTip;
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

    public UncontrolType uncontrolType = UncontrolType.None;

    /// <summary>
    /// 无法移动
    /// </summary>
    public bool lockMove = false;

    public CardData.Type type;

    public BuffEffect[] buffEffects;

    public BuffEffect[] EffectsOnTakeDamage;

    public bool RemoveOnTakeDamage = false;

    public bool RemoveOnUseSkill = false;

    [SerializeReference]
    [SerializeReferenceButton]
    public List<Effect> EffectsOnStartRound;

    public BuffConverter[] converters;

    public TipInfo[] toolTips;

    public void OnAdded(Character character) 
    {
        foreach (var effect in buffEffects)
        {
            switch (effect.EffectType)
            {
                case BuffEffect.Type.AddCritic:
                    character.Critic.AddValueMod(effect.Val1);
                    break;
                case BuffEffect.Type.AddCriticDamage:
                    character.CriticDamage.AddValueMod(effect.Val1);
                    break;
                case BuffEffect.Type.AddEnergyRecover:
                    character.EnergyRecover.AddValueMod(effect.Val1);
                    break;
                case BuffEffect.Type.AddDodge:
                    character.DodgeChance.AddValueMod(effect.Val1);
                    break;
                case BuffEffect.Type.AddSkipChant:
                    character.SkipChantChance++;
                    break;
                case BuffEffect.Type.Invisible:
                    character.ToggleInvisible(true);
                    break;
                case BuffEffect.Type.ReduceDamage:
                    character.DamageRedu.AddValueMod(effect.Val1);
                    break;
                case BuffEffect.Type.ReduceMagicDamage:
                    character.MagicDamageRedu.AddValueMod(effect.Val1);
                    break;
                case BuffEffect.Type.AddAtk:
                    character.Atk.AddValueMod(effect.Val1);
                    break;
            }
        }
    }

    public void OnTick(Character character, int level) 
    {
        foreach (var effect in buffEffects)
        {         
            if (effect.EffectType == BuffEffect.Type.Damage)
            {
                character.TakeDamage(CombatManager.CalcuDamage(effect.Val1 * level, null, 
                    character, type, DamageTag.Dot));               
            }
        }       
    }

    public void OnRemoved(Character character) 
    {
        foreach (var effect in buffEffects)
        {
            switch (effect.EffectType)
            {
                case BuffEffect.Type.AddCritic:
                    character.Critic.AddValueMod(-1f * effect.Val1);
                    break;
                case BuffEffect.Type.AddCriticDamage:
                    character.CriticDamage.AddValueMod(-1f * effect.Val1);
                    break;
                case BuffEffect.Type.AddEnergyRecover:
                    character.EnergyRecover.AddValueMod(-1f * effect.Val1);
                    break;
                case BuffEffect.Type.AddDodge:
                    character.DodgeChance.AddValueMod(-1f * effect.Val1);
                    break;
                case BuffEffect.Type.Invisible:
                    character.ToggleInvisible(false);
                    break;
                case BuffEffect.Type.ReduceDamage:
                    character.DamageRedu.AddValueMod(-1f * effect.Val1);
                    break;
                case BuffEffect.Type.ReduceMagicDamage:
                    character.MagicDamageRedu.AddValueMod(-1f * effect.Val1);
                    break;
                case BuffEffect.Type.AddAtk:
                    character.Atk.AddValueMod(-1f * effect.Val1);
                    break;
            }
        }
    }

    public void OnStartRound(Character character)
    {
        foreach (var effect in EffectsOnStartRound)
        {
            effect.Perform(character, null, null);
        }
    }

    public void OnBeHurt(Character character)
    {
        foreach (var effect in EffectsOnTakeDamage)
        {
            switch (effect.EffectType)
            {
                case BuffEffect.Type.AddHpPercent:
                    character.AddHealth(effect.Val1);
                    break;
                case BuffEffect.Type.ReduceLevel:
                    character.Buffs.ReduceBuffLevel(this);
                    break;
                default:
                    break;
            }
        }
    }

    

    [System.Serializable]
    public struct BuffEffect
    {
        public enum Type
        {
            Damage,
            
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
            AddEnergyRecover,
            AddSkipChant,
            Invisible,
            ReduceLevel,
            AddAtk,
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

public enum UncontrolType
{
    None,
    /// <summary>
    /// 免控
    /// </summary>
    Weak,
    /// <summary>
    /// 霸体免推拉
    /// </summary>
    Strong,
}
