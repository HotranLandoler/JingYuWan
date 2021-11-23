using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Game
{
    /// <summary>
    /// ÿ����λx��
    /// </summary>
    public static readonly int ChiPerUnit = 3;

    public static readonly string Critic = "����";

    public static readonly string Dodge = "����";

    public static readonly string NoEnoughEnergy = "���ֵ����";

    public static readonly string OutOfRange = "Ŀ���ڷ�Χ֮��";

    public static readonly string CantUseMagic = "���������޷��˹�";

    public static readonly string CantMove = "�����Ṧ";

    public static readonly string Controlled = "�ܿ�״̬�޷�ʩչ";

    /// <summary>
    /// ��������
    /// </summary>
    public const int HandCardsCapacity = 8;

    ///// <summary>
    ///// ÿ�غϻظ������ֵ
    ///// </summary>
    //public const int EnergyRecoverPerRound = 10;

    /// <summary>
    /// �����x����루��λ�ߣ�
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static int GetDistance(Transform a, Transform b)
    {
        return Unit2Chi(Mathf.Abs(a.position.x - b.position.x));
    }

    /// <summary>
    /// ��׼��λת��
    /// </summary>
    /// <param name="unit"></param>
    /// <returns></returns>
    public static int Unit2Chi(float unit) => ChiPerUnit * (int)unit;

    /// <summary>
    /// ��ת��׼��λ
    /// </summary>
    /// <param name="chi"></param>
    /// <returns></returns>
    public static float Chi2Unit(int chi) => (float)chi / ChiPerUnit; 
}
