using JYW.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Cinemachine;

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

    [SerializeField]
    private CinemachineVirtualCamera followCamera;

    [SerializeField]
    private CinemachineTargetGroup targetGroupPlayer;

    [SerializeField]
    private CinemachineTargetGroup targetGroupEnemy;

    private CombatManager combatManager;

    //private AudioSource audioSource;

    private Character currentRounder;

    private bool buttonClickable = false;

    //private PlacedInfo posSelecting;
    //private IEnumerable<Effect> selectPosEffects;

    //public event UnityAction CharacterDistChanged;

    private void Awake()
    {       
        combatManager = new CombatManager();
        
        //audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        player.HealthChanged += CheckGameWin;
        enemy.HealthChanged += CheckGameWin;
        player.ChantCompleted += OnChantCompleted;
        enemy.ChantCompleted += OnChantCompleted;
        player.MoveRequested += CharacterMove;
        enemy.MoveRequested += CharacterMove;
        player.CardRequested += AddCard;
        enemy.CardRequested += AddCard;
        player.PlaceRequested += SelectPos;
    }

    private void OnDisable()
    {
        player.HealthChanged -= CheckGameWin;
        enemy.HealthChanged -= CheckGameWin;
        player.ChantCompleted -= OnChantCompleted;
        enemy.ChantCompleted -= OnChantCompleted;
        player.MoveRequested -= CharacterMove;
        enemy.MoveRequested -= CharacterMove;
        player.CardRequested -= AddCard;
        enemy.CardRequested -= AddCard;
        player.PlaceRequested -= SelectPos;
    }

    // Start is called before the first frame update
    void Start()
    {
        cardsManager.CardSelected.AddListener(OnCardSelected);
        cardsManager.CardDeselected.AddListener(OnCardDeselected);
        uiManager.PlayButtonClicked.AddListener(PlayerPlayCard);
        uiManager.NextButtonClicked.AddListener(NextRound);
        //uiManager.PosSubmited += ReceivePos;
        StartCoroutine(GameMain());
    }

    private void Update()
    {
        if (player.transform.position.y < -5)
            uiManager.ShowGameOver(false);
        else if (enemy.transform.position.y < -5)
            uiManager.ShowGameOver(true);
    }

    private void CheckGameWin()
    {
        if (player.CurrentHealth <= 0)
        {
            uiManager.ShowGameOver(false);
            StopAllCoroutines();
        }
        else if (enemy.CurrentHealth <= 0)
        {
            uiManager.ShowGameOver(true);
            StopAllCoroutines();
        }           
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
        if (!data.isExtra)
            attacker.HasPlayedNonExtra = true;
        buttonClickable = false;
        Coroutine coroutine = null;
        if (currentRounder == player)
        {
            OnCardDeselected();
            coroutine = StartCoroutine(cardsManager.DropSelectedCard());
        }
        else
            cardsManager.DropAiCard(data);
        yield return uiManager.ShowCardText(data);
        if(data.performSound) AudioPlayer.Instance.PlaySound(data.performSound);
        attacker.StopChant();
        //if (coroutine != null) yield return coroutine;
        //yield return cardsManager.ClearCards();
        attacker.OnUseSkill(data);
        combatManager.PlayCard(data, attacker, defender);
        //if (!hit) defender.OnDodge();
        yield return cardsManager.PlaceCards();
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
        if (!CombatManager.CanPlayCard(card.Data, player, enemy, out string msg))
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
        uiManager.OnEnemyRound();
        currentRounder.EndRound();
        if (currentRounder == player)
        {            
            yield return cardsManager.ClearCards();
        }       
        //前一回合结束
        currentRounder.Buffs.Tick();
        currentRounder.TickPlaceds();

        currentRounder = currentRounder == player ? enemy : player;
       
        followCamera.Follow = currentRounder == player ? targetGroupPlayer.transform : targetGroupEnemy.transform;
        yield return currentRounder.ProcessChant();
        //本回合开始
        //currentRounder.CurrentEnergy += Game.EnergyRecoverPerRound;
        if (currentRounder == player)
        {
            yield return cardsManager.ShowCards();
            uiManager.NextRoundButton.FadeIn();
            buttonClickable = true;
            currentRounder.StartRound();
            uiManager.OnPlayerRound();
            yield break;
        }
        cardsManager.GenerateAiCards();
        currentRounder.StartRound();
        var card = cardsManager.GetAiDecision(enemy, player);
        if (card != null)
            yield return DoPlayCard(card, enemy, player);
        yield return new WaitForSeconds(1f);
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
        if (!CombatManager.IsTargetInRange(character.CurrentChant.TargetCard, 
            character, character.CurrentChant.Target))
            return;
        combatManager.PerformEffects(character.CurrentChant.Effects,
            character, character.CurrentChant.Target, character.CurrentChant.TargetCard);
        //if (hit == false) character.CurrentChant.Target.OnDodge();
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

    private void AddCard(Character character, CardData card)
    {
        if (character == player)
            StartCoroutine(cardsManager.AddPlayerCard(card));
        else cardsManager.AddAiCard(card);
    }

    private void SelectPos(PlacedInfo info, IEnumerable<Effect> effects)
    {
        StartCoroutine(DoSelectPos(info, effects));       
    }

    private IEnumerator DoSelectPos(PlacedInfo info, IEnumerable<Effect> effects)
    {
        cardsManager.HandCardsInteractable = false;
        //记录原相机目标
        Transform follow = followCamera.Follow;
        //selectPosEffects = effects;
        //posSelecting = info;
        yield return uiManager.WaitForPosSelecting();
        //还原相机跟随
        followCamera.Follow = follow;
        player.PlaceObjectImmediate(info, uiManager.SelectedPos.Value);
        combatManager.PerformEffects(effects, player, enemy, null);      
        cardsManager.HandCardsInteractable = true;
    }

    //private void ReceivePos(Vector2 pos)
    //{
    //    cardsManager.HandCardsInteractable = true;
    //    player.PlaceObjectImmediate(posSelecting, pos);
    //    combatManager.PerformEffects(selectPosEffects, player, enemy, null);
    //}
}
