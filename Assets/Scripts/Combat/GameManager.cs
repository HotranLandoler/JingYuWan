using JYW.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Character player;
    public Character Player => player;

    [SerializeField]
    private Character enemy;
    public Character Enemy => enemy;

    [SerializeField]
    private CardsManager cardsManager;

    [SerializeField]
    private UiManager uiManager;

    private CombatManager combatManager;

    private void Awake()
    {
        combatManager = new CombatManager();
    }

    // Start is called before the first frame update
    void Start()
    {
        cardsManager.CardSelected.AddListener(OnCardSelected);
        cardsManager.CardDeselected.AddListener(OnCardDeselected);
        uiManager.PlayButtonClicked.AddListener(PlayerPlayCard);
        StartCoroutine(GameMain());
    }

    private IEnumerator GameMain()
    {
        yield return new WaitForSeconds(2f);
        Debug.Log("GameStart");
        player.CurrentHealth -= 15;
        yield return cardsManager.ShowCards();
        uiManager.NextRoundButton.FadeIn();
        //yield return new WaitForSeconds(4f);
        //yield return cardsManager.ClearCards();
        //yield return null;
    }

    private IEnumerator DoPlayCard(CardData data, Character attacker, Character defender)
    {
        cardsManager.DropSelectedCard();
        yield return cardsManager.ClearCards();
        yield return uiManager.ShowCardText(data);
        combatManager.PlayCard(data, attacker, defender);        
    }

    private void PlayerPlayCard()
    {
        var card = cardsManager.SelectedCard;
        if (card == null)
        {
            Debug.LogError("Selected is null");
            return;
        }
        if (!combatManager.CanPlayCard(card.Data, player, enemy))
        {
            Debug.LogError("Can't play card");
            return;
        }
        StartCoroutine(DoPlayCard(card.Data, player, enemy));
    }

    private void OnCardSelected()
    {
        uiManager.PlayCardButton.FadeIn();
    }

    private void OnCardDeselected()
    {
        uiManager.PlayCardButton.FadeOut();
    }
}
