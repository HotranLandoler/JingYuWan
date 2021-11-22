using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public class Character : MonoBehaviour
{
    private static readonly float chantWaitTime = 1f;

    public static readonly int NormalMoveDist = 4;
    public static readonly int SkillMoveDist = 15;

    public event UnityAction HealthChanged;
    public event UnityAction EnergyChanged;

    public event UnityAction<DamageInfo> DamageTaken;
    public event UnityAction ChantStarted;
    //public event UnityAction ChantProcessed;
    public event UnityAction<Character> ChantCompleted;
    public event UnityAction ChantStopped;

    public event UnityAction<Character, IEnumerator> MoveRequested;
    public event UnityAction RoundStarted;

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

    private Chant currentChant;
    public Chant CurrentChant => currentChant;

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
    public float Critic => critic;

    [SerializeField]
    private float criticDamage = 1.75f;
    public float CriticDamage => criticDamage;

    /// <summary>
    /// 浮空
    /// </summary>
    public bool IsInAir { get; } = false;
    /// <summary>
    /// 未被缴械
    /// </summary>
    public bool HasWeapon { get; } = true;
    /// <summary>
    /// 未被封内
    /// </summary>
    public bool CanUseMagic { get; } = true;
    /// <summary>
    /// 未被锁足以上级别控制
    /// </summary>
    public bool CanMove { get; } = true;

    public ControlType ControlledType { get; private set; } = ControlType.None;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentHealth = maxHealth;
        currentEnergy = maxEnergy;
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

    public void StartRound() =>
        RoundStarted?.Invoke();

    public void TakeDamage(DamageInfo damage)
    {
        CurrentHealth -= damage.Damage;
        DamageTaken?.Invoke(damage);
    }

    public void TakeDamgeSequence(DamageInfo[] damages)
    {
        //Debug.Log(damages.Length);
        StartCoroutine(DoTakeDamageSequence(damages));
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
        ChantStarted?.Invoke();
    }

    public IEnumerator ProcessChant()
    {
        if (currentChant == null || currentChant.IsCompleted) yield break;
        currentChant.Process++;
        if (currentChant.Process >= currentChant.Duration)
        {
            currentChant.IsCompleted = true;
            ChantCompleted?.Invoke(this);
        }
        yield return new WaitForSeconds(chantWaitTime);
        //else ChantProcessed?.Invoke();
    }

    public void MoveRequest(int x, MoveType moveType)
    {
        if (currentChant != null && !currentChant.IsCompleted
            && !currentChant.MoveChant)
        {
            //打断读条
            currentChant.IsCompleted = true;
            ChantStopped?.Invoke();
        }
        MoveRequested?.Invoke(this, DoMove(x, moveType));
    }

    public void StopChant()
    {
        if (currentChant != null && !currentChant.IsCompleted)
        {
            //打断读条
            currentChant.IsCompleted = true;
            ChantStopped?.Invoke();
        }
    }

    public void TurnTo(Character target)
    {
        var dir = transform.position.x - target.transform.position.x;
        if (dir <= 0)
            spriteRenderer.flipX = false;
        else spriteRenderer.flipX = true;
    }

    private IEnumerator DoMove(int x, MoveType moveType)
    {
        var moveTime = moveType switch
        {
            MoveType.Normal => 0.8f,
            MoveType.Fast => 0.5f,
            _ => throw new System.NotImplementedException(),
        };
        yield return transform.DOMoveX(transform.position.x + Game.Chi2Unit(x), moveTime).WaitForCompletion();
    }

    private void OnBuffAdded(Buff buff)
    {
        if (buff.Data.controlType != ControlType.None)
            ControlledType = buff.Data.controlType;
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
/// 面向，以右为正
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
    Fast
}