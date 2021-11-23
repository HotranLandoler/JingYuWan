using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JYW.Buffs
{
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

        //public void Enable(Character character) =>
        //    Data.OnAdded(character);

        public void Tick(Character character)
        {
            DurationTimer--;
            Data.OnTick(character, Level);
            ValueChanged?.Invoke();
        }

        //public void Disable(Character character) =>
        //    Data.OnRemoved(character);

        public void AddLevel(int add = 1)
        {
            Level += add;
            DurationTimer = Duration;
            ValueChanged?.Invoke();
        }

        public void OnTakeDamage(Character character)
        {
            Data.OnBeHurt(character);
        }
    }
}