using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct DamageInfo
{
    /// <summary>
    /// 攻击发出者
    /// </summary>
    public Character Attacker;

    /// <summary>
    /// 受到伤害者
    /// </summary>
    public Character Defender;

    public EffectType Tag;

    public DamageType Type;

    /// <summary>
    /// 一般来自子弹飞行方向或aoe中心指向角色位置
    /// </summary>
    public Direction DamageDir;

    /// <summary>
    /// 最终会心几率，经历所有流程后决定是否会心
    /// </summary>
    public float CriticalRate;

    /// <summary>
    /// 最终命中率
    /// </summary>
    public float HitRate;

    /// <summary>
    /// 伤害类型
    /// </summary>
    public enum EffectType
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
