using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacedObject : MonoBehaviour
{
    private PlacedInfo placedInfo;

    private int timer;

    //public BuffInfo RelatedBuff { get; private set; }

    public void Set(PlacedInfo placedInfo)
    {
        this.placedInfo = placedInfo;
        //this.RelatedBuff = buff;
    }

    public void Tick()
    {
        timer--;
    }

    public void Activate(Character character)
    {
        MoveType moveType = placedInfo.ActiType switch
        {
            PlacedInfo.Type.MoveToPos => MoveType.Fast,
            PlacedInfo.Type.TeleportToPos => MoveType.Teleport,
            _ => throw new System.NotImplementedException()
        };
        character.MoveRequest(
                Game.Unit2Chi(transform.position.x - character.transform.position.x), moveType);
        //character.transform.position = new Vector2(transform.position.x, character.transform.position.y);
    }
}
