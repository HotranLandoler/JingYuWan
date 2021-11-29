using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIEngine
{
    public CardData Decide(IEnumerable<CardData> cards, Character agent, Character target)
    {
        if (agent.IsCurrChantValid())
            return null;
        foreach (var card in cards)
        {
            if (CombatManager.CanPlayCard(card, agent, target, out _))
            {
                if (card.GetDesire(agent, target) != 0f) return card;
            }
        }
        return null;
    }
}
