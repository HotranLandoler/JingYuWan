using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Character : MonoBehaviour
{
    public event UnityAction HealthChanged;
    public event UnityAction EnergyChanged;

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

    public BuffInfo[] test;

    [SerializeField]
    private float maxHealth = 100f;
    public float MaxHealth => maxHealth;

    private float currentHealth;
    public float CurrentHealth
    {
        get => currentHealth;
        set
        {
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
            if (currentEnergy != value)
            {
                currentEnergy = value;
                EnergyChanged?.Invoke();
            }             
        }
    }

    /// <summary>
    /// ����
    /// </summary>
    public bool IsInAir { get; }
    /// <summary>
    /// δ����е
    /// </summary>
    public bool HasWeapon { get; }
    /// <summary>
    /// δ������
    /// </summary>
    public bool CanUseMagic { get; }
    /// <summary>
    /// δ���������ϼ������
    /// </summary>
    public bool CanMove { get; }

    private void Awake()
    {
        currentHealth = maxHealth;
        currentEnergy = maxEnergy;
    }

    // Start is called before the first frame update
    void Start()
    {
        foreach (var buff in test)
        {
            Buffs.AddBuff(buff, 3);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

/// <summary>
/// ��������Ϊ��
/// </summary>
public enum Direction
{
    R = 1,
    L = -1
}