using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JYW.UI.ToolTip;

[CreateAssetMenu(fileName = "New Card", menuName = "Card")]
public class CardData : ScriptableObject, IUtility
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

    [Tooltip("��ɫ���صȼ�<=Xʱ����ʹ��")]
    public ControlType requireControl = ControlType.Stuck;

    [Tooltip("�Ǹ�����")]
    public bool isExtra = false;

    [SerializeReference]
    [SerializeReferenceButton]
    public List<Condition> conditions;

    [SerializeReference]
    [SerializeReferenceButton]
    public List<Effect> Effects;

    public AudioClip performSound;

    public TipInfo[] toolTips;

    public float GetDesire(Character attacker, Character target)
    {
        if (Id == 20) return 0f;

        foreach (var effect in Effects)
        {
            bool valid = true;
            foreach (var condition in effect.Conditions)
            {
                if (!condition.IsSatisfied(attacker, target))
                {
                    valid = false;
                    break;
                }
            }
            if (valid)
            {
                switch (effect)
                {
                    case EffectTypes.RemoveSelfControl:
                        if (attacker.ControlledType <= ControlType.Slow)
                            return 0f;
                        break;
                    case EffectTypes.StopChant:
                        if (target.CurrentChant == null || target.CurrentChant.IsCompleted)
                            return 0f;
                        break;
                    default:
                        break;
                }
                //if (effect is EffectTypes.RemoveSelfControl)
                //    if (attacker.ControlledType == ControlType.None)
                //        return 0f;
            }
        }
        return 1f;
    }

    public enum Type
    {
        /// <summary>
        /// �⹦
        /// </summary>
        Phys,
        /// <summary>
        /// �ڹ�
        /// </summary>
        Magic,
        /// <summary>
        /// �Ṧ
        /// </summary>
        Move,
        /// <summary>
        /// ��״̬
        /// </summary>
        Buff
    }

    public enum DropType
    {
        Normal,
        /// <summary>
        /// ���ޣ�������������ƶ�
        /// </summary>
        Unlimited,
        /// <summary>
        /// ���ģ�������Ƴ�
        /// </summary>
        Expendable,
        /// <summary>
        /// ���⣬�򲻴�����Ƴ�
        /// </summary>
        Special,
    }
}
