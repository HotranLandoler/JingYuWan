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

    public void OnAdded() { }

    public void OnTick() { }

    public void OnRemoved() { }

    public void OnBeHurt(DamageInfo info) { }
}
