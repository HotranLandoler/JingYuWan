using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JYW.UI.ToolTip;

[CreateAssetMenu(fileName = "New Card", menuName = "Card")]
public class CardData : ScriptableObject
{
    public int Id;

    public Sprite Icon;

    public string Title;

    [TextArea(2,3)]
    public string Desc;

    public int Cost;

    public int Range;

    public Type type;

    public DropType dropType;

    [Tooltip("角色被控等级<=X时可以使用")]
    public ControlType requireControl = ControlType.Stuck;

    [Tooltip("是附加牌")]
    public bool isExtra = false;

    [SerializeReference]
    [SerializeReferenceButton]
    public List<Condition> conditions;

    [SerializeReference]
    [SerializeReferenceButton]
    public List<Effect> Effects;

    public AudioClip performSound;

    public TipInfo[] toolTips;

    public enum Type
    {
        /// <summary>
        /// 外功
        /// </summary>
        Phys,
        /// <summary>
        /// 内功
        /// </summary>
        Magic,
        /// <summary>
        /// 轻功
        /// </summary>
        Move,
        /// <summary>
        /// 加状态
        /// </summary>
        Buff
    }

    public enum DropType
    {
        Normal,
        /// <summary>
        /// 无限，打出不进入弃牌堆
        /// </summary>
        Unlimited,
        /// <summary>
        /// 消耗，打出后移除
        /// </summary>
        Expendable,
        /// <summary>
        /// 特殊，打不打出都移除
        /// </summary>
        Special,
    }
}
