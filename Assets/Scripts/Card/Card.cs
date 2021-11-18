using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    [SerializeField]
    private CardData data;
    public CardData Data => data;

    [SerializeField]
    private Image iconImage;

    [SerializeField]
    private Text titleText;

    [SerializeField]
    private Text descText;

    public void Set(CardData data)
    { 
        this.data = data;
        iconImage.sprite = data.Icon;
        titleText.text = data.Title;
        descText.text = data.Desc;
    }

    private void Start()
    {
        if (data) Set(data);
    }
}
