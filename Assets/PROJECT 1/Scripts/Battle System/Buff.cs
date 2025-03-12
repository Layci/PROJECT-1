using System;
using UnityEngine;
using Project1;
using ProJect1;

namespace Project1
{
    public class Buff
    {
        public string buffName;        // 버프 이름
        public int remainingTurns;     // 버프 지속 턴
        public float attackBoost;      // 공격력 증가량
        public float defenseBoost;     // 방어력 증가량

        public Buff(string name, int duration, float atkBoost, float defBoost)
        {
            buffName = name;
            remainingTurns = duration;
            attackBoost = atkBoost;
            defenseBoost = defBoost;
        }

        // 버프 효과 적용
        public void ApplyEffect(BaseUnit unit)
        {
            unit.damageIncreased += attackBoost;  // 공격력 증가
            unit.damageReduction -= defenseBoost; // 받는 피해 감소
            Debug.Log(unit.name + "에게 " + buffName + " 버프 적용! (턴 수: " + remainingTurns + ")");
        }

        // 버프 제거 시 효과 원상복구
        public void RemoveEffect(BaseUnit unit)
        {
            unit.damageIncreased -= attackBoost;  // 공격력 복구
            unit.damageReduction += defenseBoost; // 받는 피해 복구
            Debug.Log(unit.name + "의 " + buffName + " 버프가 해제됨!");

        }
    }
}

