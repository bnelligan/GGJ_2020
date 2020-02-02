using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameState
{
    protected GameStateController GSController;

    public GameState(GameStateController gsController)
    {
        GSController = gsController;
    }

    public virtual IEnumerator RepairState()
    {
        yield break;
    }

    public virtual IEnumerator BattleState()
    {
        yield break;
    }

    public virtual IEnumerator GameOverState()
    {
        yield break;
    }

}
