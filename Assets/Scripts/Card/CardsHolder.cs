using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CardsHolder
{
    private CardDeck drawPile;
    private CardDeck discardPile;

    private List<CardData> cards = new(Game.HandCardsCapacity);
    public IEnumerable<CardData> Cards => cards;

    public CardsHolder(IEnumerable<CardData> data)
    {
        drawPile = new(data);
        discardPile = new();
    }

    /// <summary>
    /// �س�����
    /// </summary>
    /// <param name="handCardsNum"></param>
    public void GenerateRandomCards(int handCardsNum)
    {
        if (drawPile.GetCount() < handCardsNum)
            discardPile.MoveTo(drawPile);
        cards.Clear();
        cards.AddRange(drawPile.GetRandomCards(handCardsNum));
    }

    /// <summary>
    /// �����������������ƶѻ��Ƴ�
    /// </summary>
    /// <param name="data"></param>
    public void Drop(CardData data)
    {
        cards.Remove(data);
        if (data.dropType == CardData.DropType.Normal)
        {
            discardPile.AddCards(data);
        }
        if (data.dropType == CardData.DropType.Normal || data.dropType == CardData.DropType.Expendable)
            drawPile.RemoveCards(data);
    }

    public void Add(CardData data)
    {
        cards.Add(data);
    }
}
