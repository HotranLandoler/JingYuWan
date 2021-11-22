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
    /// ͼ��
    /// </summary>
    public Sprite Icon;

    /// <summary>
    /// Buff����
    /// </summary>
    public string Name;

    /// <summary>
    /// Buff����ʱ��
    /// </summary>
    public int Duration;

    /// <summary>
    /// ������
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
            {
                character.TakeDamage(CombatManager.CalcuDamage(effect.Val1 * level, null, character));
                
            }
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
            /// ���
            /// </summary>
            UnControllable,
            /// <summary>
            /// ����������
            /// </summary>
            UnControllable_Strong,
            AddCritic,
            AddCriticDamage,
            AddHpPercent,
            ReduceDamage,
            AddDodge,
            ReduceMagicDamage,
            /// <summary>
            /// ��е
            /// </summary>
            Disarm,
            /// <summary>
            /// ����
            /// </summary>
            NoMagic,
            /// <summary>
            /// ��Ĭ
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
        /// ת��ΪBUFF
        /// </summary>
        public BuffInfo convertTarget;
        /// <summary>
        /// �ﵽָ��levelʱת��
        /// </summary>
        public int convertLevel;
    }
}

/// <summary>
/// ���Ƶȼ�
/// </summary>
public enum ControlType
{
    None,
    /// <summary>
    /// ����
    /// </summary>
    Slow,
    /// <summary>
    /// ����
    /// </summary>
    Stuck,
    /// <summary>
    /// ����
    /// </summary>
    Freeze,
    /// <summary>
    /// ѣ��
    /// </summary>
    Daze,
    /// <summary>
    /// ����
    /// </summary>
    Down,
}