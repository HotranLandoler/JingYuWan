using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
using JYW.UI;
using JYW.UI.ToolTip;

public class Card : MonoBehaviour, IPointerDownHandler, IToolTipable
{
    private CardsManager _cardsManager;

    public RectTransform rectTransform { get; private set; }

    public Vector2 AssignedPos { get; set; }

    [SerializeField]
    private CardData data;
    public CardData Data => data;

    [SerializeField]
    private Image iconImage;

    [SerializeField]
    private Text titleText;

    [SerializeField]
    private Text descText;

    [SerializeField]
    private Text costText;

    [SerializeField]
    private Text rangeText;

    [SerializeField]
    private Image extraImage;

    [SerializeField]
    private UiButton toolTipButton;

    public ICollection<TipInfo> Tips => data.toolTips;

    //public bool Clickable { get; set; } = false;

    private Tweener moveTween;

    private Tweener scaleTween;

    private Tweener rotateTween;

    public void Set(CardData data, CardsManager cardsManager)
    {      
        this._cardsManager = cardsManager;
        this.data = data;
        iconImage.sprite = data.Icon;
        titleText.text = data.Title;
        descText.text = data.Desc;
        costText.text = data.Cost.ToString();
        rangeText.text = data.Range.ToString();
        extraImage.enabled = data.isExtra;
        //Clickable = false;
        if (!rectTransform) rectTransform = GetComponent<RectTransform>();
        rectTransform.localScale = Vector3.one;
    }

    public Tweener MoveTo(Vector2 pos, float duration)
    {
        moveTween?.Kill();
        moveTween = rectTransform.DOAnchorPos(pos, duration).SetEase(Ease.OutCubic);
        return moveTween;
    }

    public Tweener ScaleTo(float endScale, float duration)
    {
        scaleTween?.Kill();
        scaleTween = rectTransform.DOScale(endScale, duration);
        return scaleTween;
    }

    public Tweener RotateTo(Vector3 endRotate, float duration)
    {
        rotateTween?.Kill();
        rotateTween = rectTransform.DORotate(endRotate, duration);
        return rotateTween;
    }

    //private void Start()
    //{
    //    if (data) Set(data);
    //}

    //public void OnPointerEnter(PointerEventData eventData)
    //{
    //    if (!Clickable) return;
    //    rectTransform.DOScale(1.5f, 0.5f);
    //}

    //public void OnPointerExit(PointerEventData eventData)
    //{
    //    if (!Clickable) return;
    //    rectTransform.DOScale(1f, 0.4f);
    //}

    public void ToggleToolTip(bool show)
    {
        if (show)
            toolTipButton.FadeIn();
        else toolTipButton.FadeOut();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //if (!Clickable) return;
        _cardsManager.OnCardTouched(this);
    }
}
