using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlacedInfo : ScriptableObject
{
    [SerializeField]
    private new string name;

    [SerializeField]
    private bool hasDuration;

    [SerializeField]
    private int duration;

    [SerializeField]
    private PlacedObject prefab;
    public PlacedObject Prefab => prefab;

    [SerializeField]
    private BuffInfo relatedBuff;
    public BuffInfo RelatedBuff => relatedBuff;

    [SerializeField]
    private Type type;
    public Type ActiType => type;

    //[System.Serializable]
    //public struct ActivateEffect
    //{
    //    private enum Type
    //    {
    //        None,
    //        MoveToPos,
    //        TeleportToPos,
    //    }
    //}
    public enum Type
    {
        None,
        MoveToPos,
        TeleportToPos,
    }
}
