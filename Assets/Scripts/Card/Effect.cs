using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public abstract class Effect
{
    public abstract void Perform(Character attacker, Character target);
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

    [SerializeField]
    private Condition[] conditions;
    public Condition[] Conditions => conditions;


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
