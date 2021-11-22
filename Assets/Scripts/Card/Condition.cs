using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Condition
{
    public abstract bool IsSatisfied(Character character, Character target);
    //public static Dictionary<Type, string> UnSatisfiedMessage = new()
    //{
    //    { Type.NotInAir, "浮空时无法施展" },

    //};

    //public Type type;
    //public float val;

    //public bool IsSatisfied(Character character, Character target)
    //{
    //    return type switch
    //    {
    //        Type.NotInAir => character.IsInAir,
    //        Type.NeedWeapon => character.HasWeapon,
    //        Type.InRange => Game.GetDistance(character.transform, target.transform) <= val,
    //        _ => false
    //    };
    //}

    //public enum Type
    //{
    //    NotInAir,
    //    NeedWeapon,
    //    InRange,
    //}
}
