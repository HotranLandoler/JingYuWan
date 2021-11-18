using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Effect
{
    public enum Type
    {
        Damage,
        Control,
        Push,
        Drag,
    }

    public Type type;

    public float value;
}
