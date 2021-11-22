using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Chant
{
    public Character Target { get; }

    /// <summary>
    /// 可移动读条
    /// </summary>
    public bool MoveChant { get; }

    /// <summary>
    /// 所属技能
    /// </summary>
    public CardData TargetCard { get; }

    public IEnumerable<Effect> Effects { get; }

    private int process = 0;
    public int Process
    {
        get => process;
        set
        {
            if (value > Duration)
                value = Duration;
            if (value != process)
            {
                process = value;
                ValueChanged?.Invoke();
            }           
        }
    }

    public int Duration { get; }

    public bool IsCompleted { get; set; }

    public event UnityAction ValueChanged;

    public Chant(CardData data, Character target, IEnumerable<Effect> effects, int duration = 1, bool moveChant = false)
    {
        Target = target;
        TargetCard = data;
        Effects = effects;
        MoveChant = moveChant;
        Duration = duration;
    }
}
