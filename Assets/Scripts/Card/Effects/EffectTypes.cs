using System;
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

        public override bool AllowDodge => true;

        public override void Perform(Character attacker, Character target, CardData data)
        {
            target.Buffs.AddBuff(AddBuffToTarget);
        }
    }

    [System.Serializable]
    public class AddBuffSelfEffect : Effect
    {
        [SerializeField]
        private BuffInfo AddBuffToSelf;

        public override bool AllowDodge => false;

        public override void Perform(Character attacker, Character target, CardData data)
        {
            attacker.Buffs.AddBuff(AddBuffToSelf);
        }
    }

    [System.Serializable]
    public class RemoveBuffSelf : Effect
    {
        [SerializeField]
        private BuffInfo RemoveFromSelf;

        public override bool AllowDodge => false;

        public override void Perform(Character attacker, Character target, CardData data)
        {
            attacker.Buffs.RemoveBuff(buff => buff.Data == RemoveFromSelf);
        }
    }

    [System.Serializable]
    public class DamageEffect : Effect
    {
        [SerializeField]
        private float damage;

        public override bool AllowDodge => true;

        public override void Perform(Character attacker, Character target, CardData data)
        {
            if (CombatManager.CheckDodge(data, attacker, target))
            {
                target.OnDodge();
                return;
            }
            target.TakeDamage(CombatManager.CalcuDamage(damage, attacker, target));
        }
    }

    [System.Serializable]
    public class DamageSequence : Effect
    {
        [SerializeField]
        private float[] damages;

        public override bool AllowDodge => true;

        public override void Perform(Character attacker, Character target, CardData data)
        {
            DamageInfo[] damageInfos =
                Array.ConvertAll(damages, damage => CombatManager.CalcuDamage(damage, attacker, target));
            target.TakeDamgeSequence(damageInfos);
        }
    }

    [System.Serializable]
    public class StartChant : Effect
    {
        [SerializeField]
        private AudioClip chantSound;

        [SerializeField]
        private bool moveChant = false;

        [SerializeField]
        private int duration = 1;

        [SerializeField]
        private bool allowSkip = false;

        [SerializeField]
        [SerializeReference]
        [SerializeReferenceButton]
        private List<Effect> effectsOnChant;

        public override bool AllowDodge => false;

        public override void Perform(Character attacker, Character target, CardData data)
        {
            if (chantSound) AudioPlayer.Instance.PlaySound(chantSound);
            attacker.StartChant(new Chant(data, target, effectsOnChant, duration, moveChant, allowSkip));
        }
    }

    [System.Serializable]
    public class Sound : Effect
    {
        [SerializeField]
        private AudioClip sound;

        public override bool AllowDodge => false;

        public override void Perform(Character attacker, Character target, CardData data)
        {
            AudioPlayer.Instance.PlaySound(sound);
        }
    }

    [System.Serializable]
    public class PushTarget : Effect
    {
        [SerializeField]
        private int pushDist;

        public override bool AllowDodge => true;

        public override void Perform(Character attacker, Character target, CardData data)
        {
            Direction pushDir = attacker.transform.position.x <= target.transform.position.x ? 
                Direction.R : Direction.L;
            target.MoveRequest((int)pushDir * pushDist, MoveType.Fast, true);
        }
    }

    [System.Serializable]
    public class DragTarget : Effect
    {
        public override bool AllowDodge => true;

        public override void Perform(Character attacker, Character target, CardData data)
        {
            float dist = attacker.transform.position.x - target.transform.position.x;
            int chi = Game.Unit2Chi(dist);
            if (attacker.transform.position.x < target.transform.position.x)
                chi += 2;
            else chi -= 2;
            target.MoveRequest(chi, MoveType.Fast, true);
        }
    }

    [System.Serializable]
    public class MoveSelf : Effect
    {
        [SerializeField]
        private int selfMoveDist;

        [Tooltip("向前？或向后")]
        [SerializeField]
        private bool forward = true;

        [SerializeField]
        private MoveType type;

        public override bool AllowDodge => false;

        public override void Perform(Character attacker, Character target, CardData data)
        {
            Direction faceDir = attacker.transform.position.x <= target.transform.position.x ?
                Direction.R : Direction.L;
            int x = (int)faceDir * selfMoveDist;
            if (!forward) x *= -1;
            attacker.MoveRequest(x, type);
        }
    }

    [System.Serializable]
    public class AddSelfStatus : Effect
    {
        [SerializeField]
        private float addHealth;

        [SerializeField]
        private float addEnergy;

        public override bool AllowDodge => false;

        public override void Perform(Character attacker, Character target, CardData data)
        {
            attacker.AddHealth(addHealth);
            attacker.CurrentEnergy += addEnergy;
        }
    }

    [Serializable]
    public class PlaceObject : Effect
    {
        [SerializeField]
        private PlacedInfo placed;

        [SerializeField]
        private bool placeUnderfoot = true;

        [SerializeField]
        [SerializeReference]
        [SerializeReferenceButton]
        private List<Effect> effectsOnPlaced;

        public override bool AllowDodge => false;

        public override void Perform(Character attacker, Character target, CardData data)
        {
            attacker.PlaceObject(placed, placeUnderfoot, effectsOnPlaced);
        }
    }

    [Serializable]
    public class ActivatePlaced : Effect
    {
        [SerializeField]
        private PlacedInfo placed;

        public override bool AllowDodge => false;

        public override void Perform(Character attacker, Character target, CardData data)
        {
            attacker.ActivatePlaced(placed);
        }
    }

    [Serializable]
    public class GetCard : Effect
    {
        [SerializeField]
        private CardData card;

        public override bool AllowDodge => false;

        public override void Perform(Character attacker, Character target, CardData data)
        {
            attacker.RequestCard(card);
        }
    }

    //解控
    [Serializable]
    public class RemoveSelfControl : Effect
    {
        [SerializeField]
        private ControlType removeControlType;

        public override bool AllowDodge => false;

        public override void Perform(Character attacker, Character target, CardData data)
        {
            attacker.Buffs.RemoveBuff(buff => buff.Data.controlType != ControlType.None && 
                buff.Data.controlType <= removeControlType);
        }
    }
}

