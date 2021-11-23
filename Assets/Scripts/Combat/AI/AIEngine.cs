using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIEngine
{
    public CardData Decide(IList<CardData> cards, Character agent, Character target)
    {
        foreach (var card in cards)
        {
            if (CombatManager.CanPlayCard(card, agent, target, out _))
                return card;
        }
        return null;
    }
}
