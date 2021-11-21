using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectTypes
{
    [System.Serializable]
    public class AddBuffEffect : Effect
    {
        [SerializeField]
        private BuffInfo AddBuffToTarget;

        public override void Perform(Character attacker, Character target)
        {
            target.Buffs.AddBuff(AddBuffToTarget);
        }
    }

    [System.Serializable]
    public class AddBuffSelfEffect : Effect
    {
        [SerializeField]
        private BuffInfo AddBuffToSelf;

        public override void Perform(Character attacker, Character target)
        {
            attacker.Buffs.AddBuff(AddBuffToSelf);
        }
    }

    [System.Serializable]
    public class DamageEffect : Effect
    {
        [SerializeField]
        private float damage;

        public override void Perform(Character attacker, Character target)
        {
            target.TakeDamage(new DamageInfo(damage, true));
        }
    }
}

