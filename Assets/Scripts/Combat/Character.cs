using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Character : MonoBehaviour
{
    public event UnityAction HealthChanged;
    public event UnityAction EnergyChanged;

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

    private void Awake()
    {
        currentHealth = maxHealth;
        currentEnergy = maxEnergy;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

/// <summary>
/// 面向，以右为正
/// </summary>
public enum Direction
{
    R = 1,
    L = -1
}