using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public struct DamageInfo
{
    private static string space = " ";
    private static string format = "0.#";

    /// <summary>
    /// 攻击发出者
    /// </summary>
    public Character Attacker;

    /// <summary>
    /// 受到伤害者
    /// </summary>
    public Character Defender;

    public float Damage { get; }

    public bool IsCritical { get; }

    public DamageTag Tag { get; }

    //public DamageType Type { get; }

    ///// <summary>
    ///// 一般来自子弹飞行方向或aoe中心指向角色位置
    ///// </summary>
    //public Direction DamageDir;

    ///// <summary>
    ///// 最终会心几率，经历所有流程后决定是否会心
    ///// </summary>
    //public float CriticalRate;

    ///// <summary>
    ///// 最终命中率
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
/// 内外功伤害类型
/// </summary>
public enum DamageType
{
    /// <summary>
    /// 外功伤害
    /// </summary>
    Phys,
    /// <summary>
    /// 内功伤害
    /// </summary>
    Magic,
}

/// <summary>
/// 伤害类型
/// </summary>
public enum DamageTag
{
    /// <summary>
    /// 普通
    /// </summary>
    Normal,
    /// <summary>
    /// dot伤害
    /// </summary>
    Dot,
    /// <summary>
    /// 反弹伤害
    /// </summary>
    Thorn
}
