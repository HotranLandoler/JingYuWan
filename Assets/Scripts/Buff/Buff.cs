using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Buff
{
    public BuffInfo Data { get; }

    public int Duration { get; }

    public int DurationTimer { get; private set; }

    private int level;
    public int Level
    {
        get => level;
        set
        {
            if (value > Data.MaxLevel)
                value = Data.MaxLevel;
            level = value;
        }
    }

    public event UnityAction ValueChanged;
    //public float EffectTimer { get; private set; } = 0f;
    //public event Action Ticked;

    public Buff(BuffInfo data, int duration, int level = 1)
    {
        Data = data;
        Duration = duration;
        DurationTimer = duration;
        Level = level;
    }

    public void Tick(Character character)
    {
        DurationTimer--;
        Data.OnTick(character, Level);
        ValueChanged?.Invoke();
    }

    public void AddLevel(int add = 1)
    {       
        Level += add;
        DurationTimer = Duration;
        ValueChanged?.Invoke();
    }
}
