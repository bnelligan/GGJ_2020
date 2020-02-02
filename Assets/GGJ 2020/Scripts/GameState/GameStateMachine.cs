using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameStateMachine : MonoBehaviour
{
    protected GameState State;

    public void SetState(GameState state)
    {
        State = state;
        StartCoroutine(State.RepairState());
    }
}
