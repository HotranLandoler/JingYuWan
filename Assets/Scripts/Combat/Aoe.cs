using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aoe : MonoBehaviour
{
    /// <summary>
    /// ������
    /// </summary>
    public Character Caster { get; set; }

    public float Range { get; set; }

    /// <summary>
    /// ʣ��ʱ��
    /// </summary>
    public int ResTime { get; set; }

    public HashSet<Character> charactersInRange { get; set; }
}
