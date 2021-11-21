using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager
{
    public bool CanPlayCard(CardData card, Character attacker, Character defender)
    {
        if (card.Cost > attacker.CurrentEnergy)
        {
            //message = NoEnoughEnergy;
            return false;
        }
        if (card.type == CardData.Type.Magic && !attacker.CanUseMagic) return false;
        if (card.type == CardData.Type.Move && !attacker.CanMove) return false;
        foreach (var condition in card.conditions)
        {
            if (!condition.IsSatisfied(attacker, defender))
                return false;
        }
        return true;
    }

    public void PlayCard(CardData card, Character attacker, Character defender)
    {
        foreach (var effect in card.Effects)
        {
            if (effect.Conditions.Length > 0)
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

            effect.Perform(attacker, defender);
        }
    }
}
