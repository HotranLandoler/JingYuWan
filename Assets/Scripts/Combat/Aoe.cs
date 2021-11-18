using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aoe : MonoBehaviour
{
    /// <summary>
    /// 创建者
    /// </summary>
    public Character Caster { get; set; }

    public float Range { get; set; }

    /// <summary>
    /// 剩余时间
    /// </summary>
    public int ResTime { get; set; }

    public HashSet<Character> charactersInRange { get; set; }
}
