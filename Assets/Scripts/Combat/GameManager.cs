using JYW.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Cinemachine;

public class GameManager : MonoBehaviour
{
    private static WaitForSeconds waitForGameStart = new WaitForSeconds(1.5f);
    private static WaitForSeconds waitForRoundEnd = new WaitForSeconds(0.8f);
    private static WaitForSeconds waitForHalfS = new WaitForSeconds(0.5f);

    [SerializeField]
    private SectSelection sectSelect;

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

    //private CombatManager combatManager;

    private AIEngine aiEngine;

    private ActionHandler actionHandler;
    //private AudioSource audioSource;

    private Character currentRounder;

    private bool buttonClickable = false;

    public bool Paused { get; private set; } = false;

    //private PlacedInfo posSelecting;
    //private IEnumerable<Effect> selectPosEffects;

    //public event UnityAction CharacterDistChanged;

    private void Awake()
    {
        actionHandler = new ActionHandler(this);
        aiEngine = new AIEngine();
    }

    private void OnEnable()
    {
        player.HealthChanged += CheckGameWin;
        enemy.HealthChanged += CheckGameWin;
        player.CardRequested += AddCard;
        enemy.CardRequested += AddCard;
        player.PlaceRequested += SelectPos;
    }

    private void OnDisable()
    {
        player.HealthChanged -= CheckGameWin;
        enemy.HealthChanged -= CheckGameWin;
        player.CardRequested -= AddCard;
        enemy.CardRequested -= AddCard;
        player.PlaceRequested -= SelectPos;
    }

    // Start is called before the first frame update
    void Start()
    {
        //按门派初始化角色
        player.Init(sectSelect.SectA, enemy, actionHandler);
        enemy.Init(sectSelect.SectB, player, actionHandler);

        cardsManager.CardSelected.AddListener(OnCardSelected);
        cardsManager.CardDeselected.AddListener(OnCardDeselected);
        uiManager.PlayButtonClicked.AddListener(PlayerPlayCard);
        uiManager.NextButtonClicked.AddListener(NextRound);
        uiManager.MenuButton.button.onClick.AddListener(TogglePause);
        uiManager.ResumeButton.onClick.AddListener(TogglePause);
        //uiManager.PosSubmited += ReceivePos;

        StartCoroutine(GameMain());
    }

    private void Update()
    {
        //TODO
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
        yield return waitForGameStart;

        currentRounder = enemy;
        StartCoroutine(ProcessRound());
    }

    private IEnumerator DoPlayCard(CardData data, Character attacker, Character defender)
    {
        if (!data.isExtra)
            attacker.HasPlayedNonExtra = true;
        buttonClickable = false;
        //Coroutine coroutine = null;
        if (currentRounder == player)
        {
            OnCardDeselected();
            StartCoroutine(cardsManager.DropSelectedCard(player.CardsHolder));
        }
        else
            enemy.CardsHolder.Drop(data);
        yield return uiManager.ShowCardText(data);
        if(data.performSound) AudioPlayer.Instance.PlaySound(data.performSound);
        attacker.StopChant();
        //if (coroutine != null) yield return coroutine;
        //yield return cardsManager.ClearCards();
        attacker.OnUseSkill(data);
        CombatManager.PlayCard(data, attacker, defender);
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
            yield return cardsManager.ShowCards(player.CardsHolder);
            uiManager.NextRoundButton.FadeIn();
            buttonClickable = true;
            currentRounder.StartRound();
            uiManager.OnPlayerRound();
            yield break;
        }
        enemy.CardsHolder.GenerateRandomCards(Game.HandCardsNum);
        currentRounder.StartRound();
        while (true)
        {
            var card = aiEngine.Decide(enemy.CardsHolder.Cards, enemy, player);
            if (card == null) break;
            yield return DoPlayCard(card, enemy, player);
            yield return waitForHalfS;
        }
        //var card = aiEngine.Decide(enemy.CardsHolder.Cards, enemy, player);
        //if (card != null)
            //yield return DoPlayCard(card, enemy, player);
        yield return waitForRoundEnd;
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

    //private void OnChantCompleted(Character character)
    //{
    //    if (!CombatManager.IsTargetInRange(character.CurrentChant.TargetCard, 
    //        character, character.CurrentChant.Target))
    //        return;
    //    CombatManager.PerformEffects(character.CurrentChant.Effects,
    //        character, character.CurrentChant.Target, character.CurrentChant.TargetCard);
    //    //if (hit == false) character.CurrentChant.Target.OnDodge();
    //}

    public void UpdateCharacterFace()
    {
        player.TurnToTarget();
        enemy.TurnToTarget();
    }

    private void AddCard(Character character, CardData card)
    {
        if (character == player)
            StartCoroutine(cardsManager.AddPlayerCard(card, player.CardsHolder));
        //else cardsManager.AddAiCard(card);
        else enemy.CardsHolder.Add(card);
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
        CombatManager.PerformEffects(effects, player, enemy, null);      
        cardsManager.HandCardsInteractable = true;
    }

    private void TogglePause()
    {
        if (Paused)
        {
            Paused = false;
            //Time.timeScale = 1f;            
        }
        else
        {
            Paused = true;
            //Time.timeScale = 0f;
        }
        uiManager.ShowPause(Paused);
    }
}
