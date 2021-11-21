using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using DG.Tweening;
using System.Text;

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

    [SerializeField]
    private Canvas canvas;

    [SerializeField]
    private Text damageTextPrefab;

    [SerializeField]
    private RectTransform damageTextStartPos;

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

    private void Awake()
    {
        currentHealth = maxHealth;
        currentEnergy = maxEnergy;
    }

    public void TakeDamage(DamageInfo damage)
    {
        CurrentHealth -= damage.Damage;
        StringBuilder sb = new StringBuilder();
        if (damage.IsCritical) sb.Append("会心 ");
        sb.Append(damage.Damage.ToString());
        var text = Instantiate(damageTextPrefab, canvas.transform);
        text.rectTransform.anchoredPosition = damageTextStartPos.anchoredPosition;
        text.text = sb.ToString();
        text.rectTransform.DOAnchorPosY(damageTextStartPos.anchoredPosition.y + 100, 1f);
        text.DOFade(0f, 1f);
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
    R = 1,
    L = -1
}