using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Game
{
    /// <summary>
    /// 每个单位x尺
    /// </summary>
    public static readonly int ChiPerUnit = 3;

    public static readonly string Critic = "会心";

    public static readonly string Dodge = "闪避";

    public static readonly string NoEnoughEnergy = "神机值不足";

    public static readonly string OutOfRange = "目标在范围之外";

    public static readonly string CantUseMagic = "经脉受损，无法运功";

    public static readonly string CantMove = "被封轻功";

    public static readonly string Controlled = "受控状态无法施展";

    /// <summary>
    /// 手牌容量
    /// </summary>
    public const int HandCardsCapacity = 8;

    ///// <summary>
    ///// 每回合回复的神机值
    ///// </summary>
    //public const int EnergyRecoverPerRound = 10;

    /// <summary>
    /// 两物的x轴距离（单位尺）
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static int GetDistance(Transform a, Transform b)
    {
        return Unit2Chi(Mathf.Abs(a.position.x - b.position.x));
    }

    /// <summary>
    /// 标准单位转尺
    /// </summary>
    /// <param name="unit"></param>
    /// <returns></returns>
    public static int Unit2Chi(float unit) => ChiPerUnit * (int)unit;

    /// <summary>
    /// 尺转标准单位
    /// </summary>
    /// <param name="chi"></param>
    /// <returns></returns>
    public static float Chi2Unit(int chi) => (float)chi / ChiPerUnit; 
}
