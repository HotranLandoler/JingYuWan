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

    public void OnAdded() { }

    public void OnTick() { }

    public void OnRemoved() { }

    public void OnBeHurt(DamageInfo info) { }
}
