using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifiableStat
{
    /// <summary>
    /// 未受修正的值
    /// </summary>
    public float BaseValue { get; }
    /// <summary>
    /// 考虑修正的值
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
    //百分比修正
    private float pctMod = 1f;
    //值修正
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
