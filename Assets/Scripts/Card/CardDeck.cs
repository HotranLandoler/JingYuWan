using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CardDeck
{
    private List<CardData> cards;

    public CardDeck()
    {
        cards = new List<CardData>();
    }
    public CardDeck(IEnumerable<CardData> cards)
    {
        this.cards = new List<CardData>(cards);
    }

    public int GetCount() => cards.Count;

    /// <summary>
    /// ����Ƶ��ƶ�
    /// </summary>
    /// <param name="card"></param>
    public void AddCards(params CardData[] cards)
    {
        this.cards.AddRange(cards);
    }

    public void RemoveCards(params CardData[] cards)
    {
        foreach (var c in cards)
        {
            this.cards.Remove(c);
        }
    }

    /// <summary>
    /// ������ת������һ�ƶ�
    /// </summary>
    /// <param name="cardSet"></param>
    public void MoveTo(CardDeck deck)
    {
        if (deck == null)
            throw new System.ArgumentNullException(nameof(deck));
        foreach (var card in cards)
        {
            deck.AddCards(card);
        }
        cards.Clear();
    }

    /// <summary>
    /// ȡ��һ���ƣ��������ж��ƶ������㹻
    /// </summary>
    /// <returns>һ�鿨��ID</returns>
    public CardData[] GetRandomCards(int count)
    {
        if (cards.Count == 0) return null;
        if (cards.Count < count)
            throw new System.Exception("���ƶ�û���㹻����");
        HashSet<int> cardIdSet = new HashSet<int>();
        CardData[] rand = new CardData[count];
        for (int i = 0; i < count; i++)
        {
            int index = Random.Range(0, cards.Count);
            while (cardIdSet.Contains(index))
            {
                //�ظ����ٴ����
                index = Random.Range(0, cards.Count);
            }
            cardIdSet.Add(index);
            rand[i] = cards[index];
            //cards.RemoveAt(index);
        }
        return rand;
    }


}
