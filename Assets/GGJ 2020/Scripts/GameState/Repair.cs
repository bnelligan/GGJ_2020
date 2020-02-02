using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Repair : GameState
{
    
    public Repair(GameStateController gsController) : base(gsController)
    {
        
    }

    public override IEnumerator RepairState()
    {
        GSController.SetState(new Battle(GSController));
        yield return new WaitForSeconds(2f);
    }



}
