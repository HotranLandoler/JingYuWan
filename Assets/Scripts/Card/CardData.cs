using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card")]
public partial class CardData : ScriptableObject
{
    public int Id;

    public Sprite Icon;

    public string Title;

    [TextArea(2,3)]
    public string Desc;

    public int Cost;

    public int Range;

    public Type type;

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
}
