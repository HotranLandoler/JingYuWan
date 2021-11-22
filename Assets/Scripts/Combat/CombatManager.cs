using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager
{
    public bool IsTargetInRange(CardData card, Character attacker, Character defender)
    {
        if (card.Range == 0) return true;
        if (card.type != CardData.Type.Move &&
            Game.GetDistance(attacker.transform, defender.transform) > card.Range)
        {
            return false;
        }
        return true;
    }

    public bool CanPlayCard(CardData card, Character attacker, Character defender, out string message)
    {
        message = null;
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
        if (card.type == CardData.Type.Move && !attacker.CanMove)
        {
            message = Game.CantMove;
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

    public void PerformEffects(IEnumerable<Effect> effects, Character attacker, Character defender, CardData card = null)
    {
        foreach (var effect in effects)
        {
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
    }

    public static DamageInfo CalcuDamage(float baseDamage, Character attacker, Character target)
    {
        if (attacker == null)
            return new DamageInfo(baseDamage, false);
        bool critic = false;
        float damage = baseDamage;
        float rand = Random.Range(0f, 1f);
        if (rand <= attacker.Critic)
        {
            critic = true;
            damage = baseDamage * attacker.CriticDamage;
        }
        return new DamageInfo(damage, critic);
    }
}
