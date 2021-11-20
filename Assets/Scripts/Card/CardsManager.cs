using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Pool;


public class CardsManager : MonoBehaviour
{
    /// <summary>
    /// ��������
    /// </summary>
    private const int handCardsCapacity = 8;

    [SerializeField]
    private CardData[] cardDataSet;

    [SerializeField]
    private Card cardPrefab;

    [Tooltip("��������")]
    [SerializeField]
    private int handCardsNum = 5;

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

    public List<Card> Cards { get; } = new List<Card>(handCardsCapacity);

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
            defaultCapacity: handCardsCapacity, 
            maxSize: 20);
    }

    public IEnumerator ShowCards()
    {
        for (int i = 0; i < handCardsNum; i++)
        {
            //���ƿ����ѡȡ
            CardData data = cardDataSet[Random.Range(0, cardDataSet.Length)];
            var card = cardPool.Get();
            card.Set(data, this);
            //����Anchor Preset
            card.rectTransform.anchorMin = Vector2.zero;
            card.rectTransform.anchorMax = Vector2.zero;
            //����λ�õ�
            card.rectTransform.rotation = Quaternion.Euler(0f, 0f, -90f);
            card.rectTransform.anchoredPosition = cardStartPos.anchoredPosition;
            Cards.Add(card);
        }
        yield return PlaceCards();
    }

    public IEnumerator ClearCards()
    {
        foreach (var card in Cards)
        {
            card.RotateTo(new Vector3(0f, 0f, -90f), cardPlaceTime);
            card.MoveTo(cardStartPos.anchoredPosition, cardPlaceTime);
            yield return new WaitForSeconds(cardPlaceInterval);
        }
        foreach (var card in Cards)
        {
            cardPool.Release(card);
        }
        Cards.Clear();
    }

    private IEnumerator PlaceCards()
    {
        Vector2[] pos = CalcuCardPositions();
        for (int i = 0; i < Cards.Count; i++)
        {
            Cards[i].RotateTo(Vector3.zero, cardPlaceTime);
            Cards[i].MoveTo(pos[i], cardPlaceTime);
            Cards[i].AssignedPos = pos[i];
            yield return new WaitForSeconds(cardPlaceInterval);
        }
        foreach (var c in Cards)
        {
            c.Clickable = true;
        }
    }

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
        if (SelectedCard == card)
        {
            DeselectCard();
            CardDeselected?.Invoke();
        }
        else SelectCard(card);
    }

    public void DropSelectedCard()
    {
        SelectedCard = null;
        CardDeselected?.Invoke();
    }

    //��������λ��
    private Vector2[] CalcuCardPositions()
    {
        Debug.Log(Screen.width);
        if (Cards.Count == 0)
            return null;
        if (Cards.Count == 1)
            return new Vector2[] { new Vector2(Screen.width / 2f, 0f) };
        Vector2[] pos = new Vector2[Cards.Count];
        //�������ƹ�ռ���
        float rangeWidth = Cards.Count * cardWidth - (Cards.Count - 1) * cardSpacing;
        //��˵�λ��
        float startX = (Screen.width - rangeWidth) / 2f;
        //���ſ���λ��
        pos[0] = new Vector2(startX + cardWidth / 2f, 0f);
        //��������λ�ÿ��Ǽ��
        for (int i = 1; i < Cards.Count; i++)
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
        CardSelected?.Invoke();
    }

    private void DeselectCard()
    {
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
