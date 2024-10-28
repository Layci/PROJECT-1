using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project1
{
    public abstract class GameState
    {
        public abstract void EnterState(TurnSystem turnSystem);
        public abstract void UpdateState(TurnSystem turnSystem);
        public abstract void ExitState(TurnSystem turnSystem);
    }

}
