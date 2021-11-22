using JYW.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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

    //private AudioSource audioSource;

    private Character currentRounder;

    private bool buttonClickable = false;

    //public event UnityAction CharacterDistChanged;

    private void Awake()
    {       
        combatManager = new CombatManager();
        
        //audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        player.ChantCompleted += OnChantCompleted;
        enemy.ChantCompleted += OnChantCompleted;
        player.MoveRequested += CharacterMove;
        enemy.MoveRequested += CharacterMove;
    }

    private void OnDisable()
    {
        player.ChantCompleted -= OnChantCompleted;
        enemy.ChantCompleted -= OnChantCompleted;
        player.MoveRequested -= CharacterMove;
        enemy.MoveRequested -= CharacterMove;
    }

    // Start is called before the first frame update
    void Start()
    {
        cardsManager.CardSelected.AddListener(OnCardSelected);
        cardsManager.CardDeselected.AddListener(OnCardDeselected);
        uiManager.PlayButtonClicked.AddListener(PlayerPlayCard);
        uiManager.NextButtonClicked.AddListener(NextRound);
        StartCoroutine(GameMain());
    }

    private IEnumerator GameMain()
    {
        yield return new WaitForSeconds(1.5f);
        //Debug.Log("GameStart");
        //player.CurrentHealth -= 15;
        //yield return cardsManager.ShowCards();
        //uiManager.NextRoundButton.FadeIn();
        currentRounder = enemy;
        StartCoroutine(ProcessRound());
        //yield return new WaitForSeconds(4f);
        //yield return cardsManager.ClearCards();
        //yield return null;
    }

    private IEnumerator DoPlayCard(CardData data, Character attacker, Character defender)
    {
        buttonClickable = false;
        if (currentRounder == player)
        {
            OnCardDeselected();
            StartCoroutine(cardsManager.DropSelectedCard());
        }
        else
            cardsManager.DropAiCard(data);
        yield return uiManager.ShowCardText(data);
        if(data.performSound) AudioPlayer.Instance.PlaySound(data.performSound);
        attacker.StopChant();
        //yield return cardsManager.ClearCards();
        combatManager.PlayCard(data, attacker, defender);
        buttonClickable = true;
    }

    private void PlayerPlayCard()
    {
        if (!buttonClickable) return;
        var card = cardsManager.SelectedCard;
        if (card == null)
        {
            Debug.LogError("Selected is null");
            return;
        }
        if (!combatManager.CanPlayCard(card.Data, player, enemy, out string msg))
        {
            if (msg != null)
                uiManager.ShowWarning(msg);
            //Debug.LogError("Can't play card");
            return;
        }
        StartCoroutine(DoPlayCard(card.Data, player, enemy));
    }

    private IEnumerator ProcessRound()
    {       
        if (currentRounder == player)
        {            
            yield return cardsManager.ClearCards();
        }  
        //前一回合结束
        currentRounder.Buffs.Tick();
        currentRounder = currentRounder == player ? enemy : player;
        yield return currentRounder.ProcessChant();
        //本回合开始
        //回复能量
        currentRounder.CurrentEnergy += Game.EnergyRecoverPerRound;
        if (currentRounder == player)
        {
            yield return cardsManager.ShowCards();
            uiManager.NextRoundButton.FadeIn();
            buttonClickable = true;
            currentRounder.StartRound();
            yield break;
        }
        cardsManager.GenerateAiCards();
        var card = cardsManager.GetAiDecision(enemy, player);
        if (card != null)
            yield return DoPlayCard(card, enemy, player);
        yield return new WaitForSeconds(0.5f);
        yield return ProcessRound();
    }

    private void NextRound()
    {
        if (!buttonClickable) return;
        uiManager.NextRoundButton.FadeOut();
        StartCoroutine(ProcessRound());
    }

    private void OnCardSelected()
    {
        uiManager.PlayCardButton.FadeIn();
    }

    private void OnCardDeselected()
    {
        uiManager.PlayCardButton.FadeOut();
    }

    private void OnChantCompleted(Character character)
    {
        if (!combatManager.IsTargetInRange(character.CurrentChant.TargetCard, 
            character, character.CurrentChant.Target))
            return;
        combatManager.PerformEffects(character.CurrentChant.Effects,
            character, character.CurrentChant.Target);
    }

    private void CharacterMove(Character character, IEnumerator doMove)
    {       
        StartCoroutine(DoCharacterMove(character, doMove));
    }

    private IEnumerator DoCharacterMove(Character character, IEnumerator doMove)
    {
        buttonClickable = false;
        yield return doMove;
        buttonClickable = true;
        UpdateCharacterFace();
    }

    private void UpdateCharacterFace()
    {
        player.TurnTo(enemy);
        enemy.TurnTo(player);
    }
}
