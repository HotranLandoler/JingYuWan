using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Game
{
    /// <summary>
    /// 每个单位x尺
    /// </summary>
    public static readonly int ChiPerUnit = 3;

    /// <summary>
    /// 手牌容量
    /// </summary>
    public const int HandCardsCapacity = 8;

    public static int GetDistance(Transform a, Transform b)
    {
        return ChiPerUnit * (int)Mathf.Abs(a.position.x - b.position.x);
    }
}
