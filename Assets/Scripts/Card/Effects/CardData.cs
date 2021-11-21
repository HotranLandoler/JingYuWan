using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Effect Adding
/// </summary>
public partial class CardData : ScriptableObject
{
    [ContextMenu("AddEffect/Damage")]
    public void AddDamage()
    {
        Effects.Add(new EffectTypes.DamageEffect());
    }

    [ContextMenu("AddEffect/AddBuffToTarget")]
    public void AddBuffTarget() =>
        Effects.Add(new EffectTypes.AddBuffEffect());

    [ContextMenu("AddEffect/AddBuffToSelf")]
    public void AddBuffSelf() =>
        Effects.Add(new EffectTypes.AddBuffSelfEffect());
}
