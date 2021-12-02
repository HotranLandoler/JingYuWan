using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public struct DamageInfo
{
    private static string space = " ";
    private static string format = "0.#";

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

    public DamageTag Tag { get; }

    //public DamageType Type { get; }

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

    public DamageInfo(float damage, bool critic, DamageTag tag)
    {
        Attacker = null;
        Defender = null;
        Damage = damage;
        IsCritical = critic;
        Tag = tag;
    }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        if (IsCritical)
        {
            sb.Append(Game.Critic).Append(space);           
        }
        sb.Append(Damage.ToString(format));
        return sb.ToString();
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

/// <summary>
/// �˺�����
/// </summary>
public enum DamageTag
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
