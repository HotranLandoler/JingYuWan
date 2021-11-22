using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public abstract class Effect
{
    public abstract void Perform(Character attacker, Character target, CardData data);
    //public enum Type
    //{
    //    Damage,
    //    Control,
    //    Push,
    //    Drag,
    //    AddBuff,
    //}

    //public Type type;

    //public float value1;
    //public float value2;

    //[ContextMenuItem("AddCondition/Buff", nameof(EffectAddBuffCondition))]
    [SerializeField]
    [SerializeReference]
    [SerializeReferenceButton]
    private List<Condition> conditions;
    public ICollection<Condition> Conditions => conditions;
 
    private void EffectAddBuffCondition() =>
        conditions.Add(new ConditionTypes.BuffCondition());

    [ContextMenu("AddCondition/Control")]
    private void AddControlCondition() =>
        conditions.Add(new ConditionTypes.ControlCondition());
    //public void Perform(Character attacker, Character target)
    //{
    //    if (type == Type.Damage)
    //    {
    //        target.CurrentHealth -= value1;
    //    }
    //    //type switch
    //    //{
    //    //    Type.Damage => target.CurrentHealth -= value,
    //    //    _ => ,
    //    //};

    //}
}
