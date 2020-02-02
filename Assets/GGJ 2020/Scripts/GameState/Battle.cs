using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battle : GameState
{

    public Battle(GameStateController gsController) : base(gsController)
    {

    }

    public override IEnumerator RepairState()
    {
        GSController.SetState(new Repair(GSController));
        yield return new WaitForSeconds(2f);
    }



}
