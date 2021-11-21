using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct DamageInfo
{
    /// <summary>
    /// ����������
    /// </summary>
    public Character Attacker;

    /// <summary>
    /// �ܵ��˺���
    /// </summary>
    public Character Defender;

    public float Damage { get; }

    public bool IsCritical { get; }

    //public EffectType Tag;

    //public DamageType Type;

    ///// <summary>
    ///// һ�������ӵ����з����aoe����ָ���ɫλ��
    ///// </summary>
    //public Direction DamageDir;

    ///// <summary>
    ///// ���ջ��ļ��ʣ������������̺�����Ƿ����
    ///// </summary>
    //public float CriticalRate;

    ///// <summary>
    ///// ����������
    ///// </summary>
    //public float HitRate;

    public DamageInfo(float damage, bool critic)
    {
        Attacker = null;
        Defender = null;
        Damage = damage;
        IsCritical = critic;
    }

    /// <summary>
    /// �˺�����
    /// </summary>
    public enum EffectType
    {
        /// <summary>
        /// ��ͨ
        /// </summary>
        Normal,
        /// <summary>
        /// dot�˺�
        /// </summary>
        Dot,
        /// <summary>
        /// �����˺�
        /// </summary>
        Thorn
    }
}

/// <summary>
/// ���⹦�˺�����
/// </summary>
public enum DamageType
{
    /// <summary>
    /// �⹦�˺�
    /// </summary>
    Phys,
    /// <summary>
    /// �ڹ��˺�
    /// </summary>
    Magic,
}
