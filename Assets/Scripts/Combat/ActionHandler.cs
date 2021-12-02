using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionHandler
{
    private GameManager gm;

    public ActionHandler(GameManager gm)
    {
        this.gm = gm;
    }

    public void OnChantCompleted(Character character)
    {
        if (!CombatManager.IsTargetInRange(character.CurrentChant.TargetCard,
            character, character.CurrentChant.Target))
            return;
        CombatManager.PerformEffects(character.CurrentChant.Effects,
            character, character.CurrentChant.Target, character.CurrentChant.TargetCard);
        //if (hit == false) character.CurrentChant.Target.OnDodge();
    }

    public void Move(Character character, IEnumerator doMove)
    {
        gm.StartCoroutine(DoCharacterMove(character, doMove));
    }

    private IEnumerator DoCharacterMove(Character character, IEnumerator doMove)
    {
        //buttonClickable = false;
        yield return doMove;
        //buttonClickable = true;
        gm.UpdateCharacterFace();
    }

    
}
