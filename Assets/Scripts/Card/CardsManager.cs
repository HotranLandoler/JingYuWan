using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Pool;


public class CardsManager : MonoBehaviour
{
    private static WaitForSeconds waitForCardPlaceInterval;
    private static WaitForSeconds waitForCardPlace;

    [SerializeField]
    private Card cardPrefab;

    [Tooltip("ÿ�ſ��ƵĿ��")]
    [SerializeField]
    private float cardWidth = 320f;

    [Tooltip("���Ƽ��")]
    [SerializeField]
    private float cardSpacing = 50f;

    [Tooltip("���Ƴ�ʼ���˶�ʱ��")]
    [SerializeField]
    private float cardPlaceTime = 0.6f;

    [Tooltip("��ʼ��ʱÿ���ƽ������ʱ��")]
    [SerializeField]
    private float cardPlaceInterval = 0.4f;

    [Tooltip("ѡ�п���ʱ���Ŵ�С")]
    [SerializeField]
    private float cardActiveScale = 1.5f;

    [Tooltip("�����ڳ����˶�ʱ��")]
    [SerializeField]
    private float cardActionTime = 0.4f;

    [SerializeField]
    private RectTransform cardStartPos;

    [SerializeField]
    private RectTransform cardActivePos;

    [SerializeField]
    private RectTransform cardDiscardPos;

    private List<Card> cardUis = new List<Card>(Game.HandCardsCapacity);

    private Card _selectedCard;
    /// <summary>
    /// ��ǰѡ�е���
    /// </summary>
    public Card SelectedCard 
    {
        get => _selectedCard;
        private set
        {
            _selectedCard = value;
        }
    }

    public bool HandCardsInteractable { get; set; } = false;

    public UnityEvent CardSelected;
    public UnityEvent CardDeselected;

    private ObjectPool<Card> cardPool;

    private void Awake()
    {
        cardPool = new ObjectPool<Card>(
            createFunc: CreatePooledCard, 
            actionOnGet: OnCardTakenFromPool, 
            actionOnRelease: OnCardReturnedToPool, 
            actionOnDestroy: OnDestroyPooledCard,
            defaultCapacity: Game.HandCardsCapacity, 
            maxSize: 20);
        waitForCardPlaceInterval = new WaitForSeconds(cardPlaceInterval);
        waitForCardPlace = new WaitForSeconds(cardPlaceTime);
    }

    private void OnDestroy()
    {
        cardPool.Clear();
    }

    public IEnumerator ShowCards(CardsHolder holder)
    {
        holder.GenerateRandomCards(Game.HandCardsNum);

        var datas = holder.Cards;
        foreach (var c in datas)
        {
            Card card = CreateCard(c);
            cardUis.Add(card);
        }
        yield return PlaceCards();

        //if (playerDrawPile.GetCount() < handCardsNum)
        //    playerDiscardPile.MoveTo(playerDrawPile);
        //CardData[] datas = playerDrawPile.GetRandomCards(handCardsNum);
        //foreach (var c in datas)
        //{
        //    Card card = CreateCard(c);
        //    cardUis.Add(card);
        //}
        //yield return PlaceCards();
    }

    private Card CreateCard(CardData data)
    {
        var card = cardPool.Get();
        card.Set(data, this);
        //����Anchor Preset
        card.rectTransform.anchorMin = Vector2.zero;
        card.rectTransform.anchorMax = Vector2.zero;
        //����λ�õ�
        card.rectTransform.rotation = Quaternion.Euler(0f, 0f, -90f);
        card.rectTransform.anchoredPosition = cardStartPos.anchoredPosition;
        return card;
    }

    public IEnumerator ClearCards()
    {
        if (cardUis.Count == 0) yield break;
        foreach (var card in cardUis)
        {
            card.RotateTo(new Vector3(0f, 0f, -90f), cardPlaceTime);
            card.MoveTo(cardStartPos.anchoredPosition, cardPlaceTime);
            yield return waitForCardPlaceInterval;
        }
        foreach (var card in cardUis)
        {
            cardPool.Release(card);
        }
        cardUis.Clear();
    }

    public IEnumerator DropSelectedCard(CardsHolder holder)
    {
        HandCardsInteractable = false;
        //SelectedCard.Clickable = false;
        SelectedCard.ToggleToolTip(false);
        var time = cardPlaceTime;
        SelectedCard.ScaleTo(1f, time);
        SelectedCard.RotateTo(new Vector3(0f, 0f, 90f), time);
        SelectedCard.MoveTo(cardDiscardPos.anchoredPosition, time);
        yield return waitForCardPlace;
        cardPool.Release(SelectedCard);
        cardUis.Remove(SelectedCard);

        holder.Drop(SelectedCard.Data);
        //DropCard(SelectedCard.Data, playerDrawPile, playerDiscardPile);       
        
        SelectedCard = null;
        //CardDeselected?.Invoke();
        //yield return PlaceCards();
    }

    public IEnumerator AddPlayerCard(CardData card, CardsHolder holder)
    {
        HandCardsInteractable = false;
        holder.Add(card);
        cardUis.Add(CreateCard(card));
        yield return PlaceCards();
        HandCardsInteractable = true;
    }

    public IEnumerator PlaceCards()
    {
        Vector2[] pos = CalcuCardPositions();
        for (int i = 0; i < cardUis.Count; i++)
        {
            cardUis[i].RotateTo(Vector3.zero, cardPlaceTime);
            cardUis[i].MoveTo(pos[i], cardPlaceTime);
            cardUis[i].AssignedPos = pos[i];
            yield return waitForCardPlaceInterval;
        }
        HandCardsInteractable = true;
        //foreach (var c in Cards)
        //{
        //    c.Clickable = true;
        //}
    }

    //public void GenerateAiCards()
    //{
    //    //aiDrawPile.AddCards(aiCards.ToArray());
    //    aiCards.Clear();
    //    if (aiDrawPile.GetCount() < handCardsNum)
    //        aiDiscardPile.MoveTo(aiDrawPile);
    //    CardData[] datas = aiDrawPile.GetRandomCards(handCardsNum);
    //    foreach (var data in datas)
    //    {
    //        aiCards.Add(data);
    //    }
    //    //for (int i = 0; i < handCardsNum; i++)
    //    //{
    //    //    //���ƿ����ѡȡ
    //    //    CardData data = cardDataSet[Random.Range(0, cardDataSet.Length)];
    //    //    aiCards.Add(data);
    //    //}
    //}

    //public CardData GetAiDecision(Character agent, Character target)
    //    => aiEngine.Decide(aiCards, agent, target);
    
    //public void OnCardClicked(Card card)
    //{
    //    if (SelectedCard == card)
    //    {
    //        card.rectTransform.DOScale(1f, 0.4f);
    //        card.rectTransform.DOAnchorPos(pos[0].anchoredPosition, 0.5f);
    //        SelectedCard = null;
    //        return;
    //    }
    //    SelectedCard = card;
    //    card.rectTransform.DOScale(1.5f, 0.4f);
    //    card.rectTransform.DOAnchorPos(cardActivePos.anchoredPosition, 0.5f);
    //}

    public void OnCardTouched(Card card)
    {
        if (!HandCardsInteractable) return;
        if (SelectedCard == card)
        {
            DeselectCard();
            CardDeselected?.Invoke();
        }
        else SelectCard(card);
    }
    

    //��������λ��
    private Vector2[] CalcuCardPositions()
    {
        //Debug.Log(Screen.width);
        if (cardUis.Count == 0)
            return null;
        if (cardUis.Count == 1)
            return new Vector2[] { new Vector2(Screen.width / 2f, 0f) };
        Vector2[] pos = new Vector2[cardUis.Count];
        //�������ƹ�ռ���
        float rangeWidth = cardUis.Count * cardWidth - (cardUis.Count - 1) * cardSpacing;
        //��˵�λ��
        float startX = (Screen.width - rangeWidth) / 2f;
        //���ſ���λ��
        pos[0] = new Vector2(startX + cardWidth / 2f, 0f);
        //��������λ�ÿ��Ǽ��
        for (int i = 1; i < cardUis.Count; i++)
        {
            pos[i] = new Vector2(pos[i - 1].x + cardWidth - cardSpacing, 0f);
        }
        return pos;
    }

    private void SelectCard(Card card)
    {
        if (SelectedCard)
        {
            DeselectCard();
        }
        SelectedCard = card;
        PlaceOnTable(card);
        if (SelectedCard.Data.toolTips.Length != 0)
            SelectedCard.ToggleToolTip(true);
        CardSelected?.Invoke();
    }

    private void DeselectCard()
    {
        SelectedCard.ToggleToolTip(false);
        BackToHand(SelectedCard);
        SelectedCard = null;       
    }

    private void BackToHand(Card card)
    {
        card.MoveTo(card.AssignedPos, cardActionTime);
        card.ScaleTo(1f, cardActionTime);
    }

    private void PlaceOnTable(Card card)
    {
        card.MoveTo(cardActivePos.anchoredPosition, cardActionTime);
        card.ScaleTo(cardActiveScale, cardActionTime);
    }

    private Card CreatePooledCard() => Instantiate(cardPrefab, transform);

    private void OnCardTakenFromPool(Card instance) => 
        instance.gameObject.SetActive(true);

    private void OnCardReturnedToPool(Card instance) =>
        instance.gameObject.SetActive(false);

    private void OnDestroyPooledCard(Card instance) =>
        Destroy(instance.gameObject);
}
