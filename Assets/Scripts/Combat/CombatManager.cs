using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager
{
    public static bool IsTargetInRange(CardData card, Character attacker, Character defender)
    {
        if (card.Range == 0) return true;
        if ((card.type == CardData.Type.Phys || card.type == CardData.Type.Magic) &&
            Game.GetDistance(attacker.transform, defender.transform) > card.Range)
        {
            return false;
        }
        return true;
    }

    public static bool CanPlayCard(CardData card, Character attacker, Character defender, out string message)
    {
        message = null;
        if (!card.isExtra && attacker.HasPlayedNonExtra)
        {
            message = Game.CantPlayNonExtra;
            return false;
        }
        if (card.Cost > attacker.CurrentEnergy)
        {
            message = Game.NoEnoughEnergy;
            return false;
        }
        if (!IsTargetInRange(card, attacker, defender))
        {
            message = Game.OutOfRange;
            return false;
        }
        if (card.type == CardData.Type.Magic && !attacker.CanUseMagic)
        {
            message = Game.CantUseMagic;
            return false;
        }
        //if (card.type == CardData.Type.Move && !attacker.CanMove())
        //{
        //    message = Game.CantMove;
        //    return false;
        //}
        if (attacker.ControlledType > card.requireControl)
        {
            message = Game.Controlled;
            return false;
        }
        if ((card.type == CardData.Type.Phys || card.type == CardData.Type.Magic) && defender.Invisible)
        {
            message = Game.NoTarget;
            return false;
        }
        foreach (var condition in card.conditions)
        {
            if (!condition.IsSatisfied(attacker, defender))
                return false;
        }
        return true;
    }

    public void PlayCard(CardData card, Character attacker, Character defender)
    {
        attacker.CurrentEnergy -= card.Cost;
        PerformEffects(card.Effects, attacker, defender, card);
    }

    public void PerformEffects(IEnumerable<Effect> effects, Character attacker, Character defender, CardData card)
    {
        bool dodge = CheckDodge(card, attacker, defender);
        if (dodge) defender.OnDodge();
        foreach (var effect in effects)
        {
            if (dodge && effect.AllowDodge) continue;
            if (effect.Conditions.Count > 0)
            {
                bool canPerform = true;
                foreach (var condition in effect.Conditions)
                {
                    if (!condition.IsSatisfied(attacker, defender))
                    {
                        canPerform = false;
                        break;
                    }
                }
                if (!canPerform) continue;
            }

            effect.Perform(attacker, defender, card);
        }
        //return true;
    }

    public static DamageInfo CalcuDamage(float baseDamage, Character attacker, Character target, CardData.Type type)
    {
        //if (attacker == null)
        //    return new DamageInfo(baseDamage, false);
        bool critic = false;
        float damage = baseDamage;
        if (attacker != null)
        {
            //计算会心
            float rand = Random.Range(0f, 1f);
            if (rand <= attacker.Critic.FinalValue)
            {
                critic = true;
                damage = baseDamage * attacker.CriticDamage.FinalValue;
            }
        }       
        //计算减伤
        float damageReduce = 0f;
        damageReduce += target.DamageRedu.FinalValue;
        if (type == CardData.Type.Magic)
            damageReduce += target.MagicDamageRedu.FinalValue;
        damage *= (1 - damageReduce);
        return new DamageInfo(damage, critic);
    }

    /// <summary>
    /// 是否被闪避
    /// </summary>
    /// <param name="card"></param>
    /// <param name="attacker"></param>
    /// <param name="defender"></param>
    /// <returns></returns>
    public static bool CheckDodge(CardData card, Character attacker, Character defender)
    {
        if (card == null) return false;
        if (card.type == CardData.Type.Phys)
        {
            if (Random.Range(0f, 1f) < defender.DodgeChance.FinalValue)
                return true;
        }
        return false;
    }
}
