using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifiableStat
{
    /// <summary>
    /// δ��������ֵ
    /// </summary>
    public float BaseValue { get; }
    /// <summary>
    /// ����������ֵ
    /// </summary>
    public float FinalValue
    {
        get
        {
            if (!cachedFinalValue.HasValue)
            {
                cachedFinalValue = (BaseValue + valueMod) * pctMod;               
            }
            return cachedFinalValue.Value;
        }
    }
    //�ٷֱ�����
    private float pctMod = 1f;
    //ֵ����
    private float valueMod;

    private float? cachedFinalValue;

    public ModifiableStat(float value)
    {
        BaseValue = value;
    }

    public void AddPercentMod(float pct)
    {
        pctMod += pct;
        cachedFinalValue = default;
    }
    public void AddValueMod(float value)
    {
        valueMod += value;
        cachedFinalValue = default;
    }
}
