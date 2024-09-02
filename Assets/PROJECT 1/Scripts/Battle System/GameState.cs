using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project1
{
    public abstract class GameState
    {
        public abstract void EnterState(TurnSystem turnSystem);  // 상태 진입 시 호출
        public abstract void UpdateState(TurnSystem turnSystem); // 상태 업데이트 시 호출
        public abstract void ExitState(TurnSystem turnSystem);   // 상태 종료 시 호출
    }
}
