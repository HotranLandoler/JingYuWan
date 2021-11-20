using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff
{
    public BuffInfo Data { get; }

    public int Duration { get; }

    public int DurationTimer { get; private set; }

    //public float EffectTimer { get; private set; } = 0f;
    //public event Action Ticked;

    public Buff(BuffInfo data, int duration)
    {
        Data = data;
        Duration = duration;
        DurationTimer = duration;
    }

    public void Tick(Character character)
    {
        DurationTimer--;
        Data.OnTick(character);
    }
}
