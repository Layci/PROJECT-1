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
        public Type exclusiveCharacter;  // 특정 캐릭터 전용 (없으면 null)

        public Buff(string name, int duration, float atkBoost, float defBoost, Type exclusiveCharacter = null)
        {
            buffName = name;
            remainingTurns = duration;
            attackBoost = atkBoost;
            defenseBoost = defBoost;
            this.exclusiveCharacter = exclusiveCharacter;
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

            if(exclusiveCharacter != null && unit.GetType() == exclusiveCharacter)
            {
                if (unit is FayePlayerControl faye)
                {
                    faye.DecreaseBuffPower();
                }
            }

            // BuffUI 업데이트
            BuffUI buffUI = unit.GetComponent<BuffUI>();
            if (buffUI != null)
            {
                buffUI.UpdateBuffTurn(0); // 버프가 없어지면 0으로 초기화
            }
        }
    }
}

