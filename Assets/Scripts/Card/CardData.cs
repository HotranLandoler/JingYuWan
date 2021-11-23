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
}
