using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Sect/Sect")]
public class Sect : ScriptableObject
{
    [SerializeField]
    private string sectName;
    public string Name => sectName;

    [SerializeField]
    private Sprite sectIcon;
    public Sprite Icon => sectIcon;

    [SerializeField]
    private Sprite baseSprite;
    public Sprite BaseSprite => baseSprite;

    [SerializeField]
    private RuntimeAnimatorController animator;
    public RuntimeAnimatorController Animator => animator;

    [SerializeField]
    private CardData[] cardsSet;
    public IEnumerable<CardData> CardsSet => cardsSet;
}
