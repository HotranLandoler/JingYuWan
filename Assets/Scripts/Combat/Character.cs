using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using JYW.Buffs;

public class Character : MonoBehaviour
{
    private Sect sect;
    public Sect Sect => sect;

    private static readonly float chantWaitTime = 1f;

    public static readonly int NormalMoveDist = 4;
    public static readonly float NormalMoveSpeed = 2f;
    public static readonly float SkillMoveSpeed = 6f;

    public event UnityAction Initialized;
    public event UnityAction HealthChanged;
    public event UnityAction EnergyChanged;

    public event UnityAction<DamageInfo> DamageTaken;
    public event UnityAction ChantStarted;
    public event UnityAction<Character> ChantCompleted;
    public event UnityAction ChantStopped;

    //public event UnityAction<Character, IEnumerator> MoveRequested;
    public event UnityAction<Character, CardData> CardRequested;

    public event UnityAction RoundStarted;
    public event UnityAction RoundEnded;

    public event UnityAction Dodged;

    public event UnityAction<bool> InvisibleSet;

    public event UnityAction<PlacedInfo, IEnumerable<Effect>> PlaceRequested;

    private Character target;

    private BuffHolder buffHolder;
    public BuffHolder Buffs
    {
        get
        {
            if (buffHolder == null)
                buffHolder = new BuffHolder(this);
            return buffHolder;
        }
    }

    private CardsHolder cardsHolder;
    public CardsHolder CardsHolder => cardsHolder;

    private Chant currentChant;
    public Chant CurrentChant => currentChant;

    private ActionHandler actions;

    [SerializeField]
    private float maxHealth = 100f;
    public float MaxHealth => maxHealth;

    private float currentHealth;
    public float CurrentHealth
    {
        get => currentHealth;
        private set
        {
            if (value > maxHealth)
                value = maxHealth;
            if (currentHealth != value)
            {
                currentHealth = value;
                HealthChanged?.Invoke();
            }               
        }
    }

    [SerializeField]
    private float maxEnergy = 100f;
    public float MaxEnergy => maxEnergy;

    private float currentEnergy;
    public float CurrentEnergy
    {
        get => currentEnergy;
        set
        {
            if (value > maxEnergy)
                value = maxEnergy;
            if (currentEnergy != value)
            {
                currentEnergy = value;
                EnergyChanged?.Invoke();
            }             
        }
    } 

    [SerializeField]
    private float critic = 0.2f;
    public ModifiableStat Critic { get; private set; }

    [SerializeField]
    private float criticDamage = 1.75f;
    public ModifiableStat CriticDamage { get; private set; }

    [SerializeField]
    private float energyRecover = 10;
    public ModifiableStat EnergyRecover { get; private set; }

    [SerializeField]
    private float dodge = 10;
    public ModifiableStat DodgeChance { get; private set; }

    public ModifiableStat DamageRedu { get; private set; }
    public ModifiableStat MagicDamageRedu { get; private set; }
    public ModifiableStat Atk { get; private set; }

    /// <summary>
    /// ????????????
    /// </summary>
    public int SkipChantChance { get; set; } = 0;

    /// <summary>
    /// ????????????????????
    /// </summary>
    public bool HasPlayedNonExtra { get; set; } = false;

    [SerializeField]
    private PlacedObject placedPrefab;

    public bool Invisible { get; private set; } = false;
    /// <summary>
    /// ????
    /// </summary>
    public bool IsInAir { get; } = false;
    /// <summary>
    /// ????????
    /// </summary>
    public bool HasWeapon { get; } = true;
    /// <summary>
    /// ????????
    /// </summary>
    public bool CanUseMagic { get; } = true;
    ///// <summary>
    ///// ????????????????????
    ///// </summary>
    //public bool CanMove { get; } = true;

    /// <summary>
    /// ????????????
    /// </summary>
    public ControlType ControlledType { get; private set; } = ControlType.None;

    private SpriteRenderer spriteRenderer;
    private Animator animator;

    private Dictionary<PlacedInfo, PlacedObject> placeds = new(3);

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
        currentEnergy = maxEnergy;
        Critic = new ModifiableStat(critic);
        CriticDamage = new ModifiableStat(criticDamage);
        EnergyRecover = new ModifiableStat(energyRecover);
        DodgeChance = new ModifiableStat(dodge);
        DamageRedu = new ModifiableStat(0f);
        MagicDamageRedu = new ModifiableStat(0f);
        Atk = new ModifiableStat(1f);
    }

    private void OnEnable()
    {
        Buffs.BuffAdded += OnBuffAdded;
        Buffs.BuffRemoved += OnBuffRemoved;
    }

    private void OnDisable()
    {
        Buffs.BuffAdded -= OnBuffAdded;
        Buffs.BuffRemoved -= OnBuffRemoved;
    }

    /// <summary>
    /// ??????????????????
    /// </summary>
    /// <param name="sect"></param>
    public void Init(Sect sect, Character target, ActionHandler actionHandler)
    {
        this.sect = sect;
        this.target = target;
        cardsHolder = new CardsHolder(sect.CardsSet);
        spriteRenderer.sprite = sect.BaseSprite;
        animator.runtimeAnimatorController = sect.Animator;
        actions = actionHandler;
        Initialized?.Invoke();
    }

    public void StartRound()
    {
        CurrentEnergy += EnergyRecover.FinalValue;
        Buffs.OnStartRound();
        HasPlayedNonExtra = false;
        RoundStarted?.Invoke();
    }

    public void EndRound() =>
        RoundEnded?.Invoke();

    public void AddHealth(float add) =>
        CurrentHealth += add;

    public void OnUseSkill(CardData data)
    {
        Buffs.RemoveBuff(buff => buff.Data.RemoveOnUseSkill);
    }

    public void TakeDamage(DamageInfo damage)
    {
        CurrentHealth -= damage.Damage;
        Buffs.OnTakeDamage(damage);
        DamageTaken?.Invoke(damage);
    }

    public void TakeDamgeSequence(DamageInfo[] damages)
    {
        //Debug.Log(damages.Length);
        StartCoroutine(DoTakeDamageSequence(damages));
    }

    public void PlaceObject(PlacedInfo placed, bool underfoot, IEnumerable<Effect> effects)
    {
        if (placeds.TryGetValue(placed, out PlacedObject obj))
        {
            //??????????
            Destroy(obj.gameObject);
            placeds.Remove(placed);
        }
        if (underfoot)
        {
            var pos = new Vector2(transform.position.x, 0f);
            PlaceObjectImmediate(placed, pos);
            if (placed.RelatedBuff) Buffs.AddBuff(placed.RelatedBuff);
            return;
        }
        PlaceRequested?.Invoke(placed, effects);

    }

    public void PlaceObjectImmediate(PlacedInfo placed, Vector2 pos)
    {       
        var placedObject = Instantiate(placed.Prefab, pos, Quaternion.identity);
        placedObject.Set(placed);
        placeds.Add(placed, placedObject);
    }

    public void ActivatePlaced(PlacedInfo placed)
    {
        if (placeds.TryGetValue(placed, out PlacedObject obj))
        {
            obj.Activate(this);
            if (placed.RelatedBuff) Buffs.RemoveBuff(buff => buff.Data == placed.RelatedBuff);
            Destroy(obj.gameObject);
            placeds.Remove(placed);
        }
    }

    public void TickPlaceds()
    {
        foreach (var placed in placeds)
        {
            placed.Value.Tick();
        }
        //TODO
    }

    public void RequestCard(CardData card)
    {
        CardRequested?.Invoke(this, card);
    }

    public void OnDodge() => Dodged?.Invoke();

    public void ToggleInvisible(bool invisible)
    {
        Invisible = invisible;
        target?.StopChant();
        InvisibleSet?.Invoke(invisible);
    }

    private IEnumerator DoTakeDamageSequence(DamageInfo[] damages)
    {
        foreach (var damage in damages)
        {
            TakeDamage(damage);
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void StartChant(Chant chant)
    {
        currentChant = chant;
        if (chant.AllowSkip && SkipChantChance > 0)
        {
            SkipChantChance--;
            ChantCompleted?.Invoke(this);
            return;
        }       
        ChantStarted?.Invoke();
    }

    public IEnumerator ProcessChant()
    {
        if (currentChant == null || currentChant.IsCompleted) yield break;
        currentChant.Process++;
        if (currentChant.Process >= currentChant.Duration)
        {
            currentChant.IsCompleted = true;
            actions.OnChantCompleted(this);
            ChantCompleted?.Invoke(this);
        }
        yield return new WaitForSeconds(chantWaitTime);
        //else ChantProcessed?.Invoke();
    }

    public bool CanMove()
    {
        if (ControlledType >= ControlType.Stuck)
        {
            return false;
        }
        if (Buffs.HasBuff(buff => buff.Data.lockMove)) return false;
        return true;
    }

    /// <summary>
    /// ????????
    /// </summary>
    /// <param name="x"></param>
    /// <param name="moveType"></param>
    /// <param name="isPassive">??????????????</param>
    /// <exception cref="System.NotImplementedException"></exception>
    public bool MoveRequest(int x, MoveType moveType, bool isPassive = false)
    {
        if (ControlledType >= ControlType.Stuck && !isPassive)
        {
            //????????????????????
            return false;
        }
        if (isPassive)
        {
            //??????????
            if (Buffs.HasBuff(buff => buff.Data.uncontrolType == UncontrolType.Strong))
                return false;
        }
        if (currentChant != null && !currentChant.IsCompleted
            && !currentChant.MoveChant)
        {
            //????????
            currentChant.IsCompleted = true;
            ChantStopped?.Invoke();
        }
        float dist = Game.Chi2Unit(x);
        float moveSpeed = moveType switch
        {
            MoveType.Normal => NormalMoveSpeed,
            MoveType.Fast => SkillMoveSpeed,
            MoveType.Teleport => 50f,
            _ => throw new System.NotImplementedException(),
        };
        if (moveType == MoveType.Normal && ControlledType == ControlType.Slow)
        {
            //??????????
            dist *= 0.5f;
            //moveSpeed *= 0.5f;
        }
        actions.Move(this, DoMove(dist, Mathf.Abs(dist) / moveSpeed));
        //MoveRequested?.Invoke(this, DoMove(dist, Mathf.Abs(dist) / moveSpeed));
        return true;
    }

    /// <summary>
    /// ??????????????????????
    /// </summary>
    /// <returns></returns>
    public bool IsCurrChantValid()
    {
        if (currentChant == null || currentChant.IsCompleted)
            return false;
        return CombatManager.IsTargetInRange(currentChant.TargetCard,
            this, currentChant.Target);
    }

    public void StopChant()
    {
        if (currentChant != null && !currentChant.IsCompleted)
        {
            //????????
            currentChant.IsCompleted = true;
            ChantStopped?.Invoke();
        }
    }

    public void TurnToTarget()
    {
        var dir = transform.position.x - target.transform.position.x;
        if (dir <= 0)
            spriteRenderer.flipX = false;
        else spriteRenderer.flipX = true;
    }

    private IEnumerator DoMove(float xUnit, float moveTime)
    {
        //var moveTime = moveType switch
        //{
        //    MoveType.Normal => 0.8f,
        //    MoveType.Fast => 0.5f,
        //    _ => throw new System.NotImplementedException(),
        //};
        yield return transform.DOMoveX(transform.position.x + xUnit, moveTime).WaitForCompletion();
    }

    private void OnBuffAdded(Buff buff)
    {
        if (buff.Data.controlType != ControlType.None)
        {
            ControlledType = buff.Data.controlType;
            if (ControlledType >= ControlType.Freeze)
                StopChant();
        }
    }

    private void OnBuffRemoved(Buff buff)
    {
        if (buff.Data.controlType != ControlType.None)
            ControlledType = ControlType.None;
    }

    // Start is called before the first frame update
    //void Start()
    //{
    //    foreach (var buff in test)
    //    {
    //        Buffs.AddBuff(buff, 3);
    //    }
    //}
}

/// <summary>
/// ??????????????
/// </summary>
public enum Direction
{
    None,
    R = 1,
    L = -1
}

public enum MoveType
{
    Normal,
    Fast,
    Teleport,
}
