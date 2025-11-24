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

        public bool resetPowerOnExpire; // 

        public Buff(string name, int duration, float atkBoost, float defBoost, Type exclusiveCharacter = null, bool resetPowerOnExpire = true)
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
            BaseCharacterControl player = unit as BaseCharacterControl;

            unit.damageIncreased -= attackBoost;  // 공격력 복구
            unit.damageReduction += defenseBoost; // 받는 피해 복구
            Debug.Log(unit.name + "의 " + buffName + " 버프가 해제됨!");

            // 전용 캐릭터에게만 적용 (ex: Faye 전용 버프)
            if (exclusiveCharacter != null && unit.GetType() == exclusiveCharacter)
            {
                if (player != null && player.ui != null)
                {
                    BuffIconUI iconUI = player.ui.buffIconUI;  // ★ UI 안에 있는 BuffIconUI 가져오기

                    if (iconUI != null)
                    {
                        player.ui.UpdateBuffPower(player.buffPower);
                    }
                }
            }

            // 아군이면 BaseCharacterControl로 캐스팅
            
            if (player != null && player.ui != null)
            {
                // 버프 턴 0으로 감소
                player.buffTrun = 0;

                // UI 업데이트
                player.ui.UpdateBuff();
            }

            /*if (exclusiveCharacter != null && unit.GetType() == exclusiveCharacter)
            {
                if (unit is FayePlayerControl)
                {
                    player.ui.UpdateBuffPower();
                    Debug.Log("FAYE버프 해제");
                }
            }*/

            // 적 UI는 아직 없다면 건드릴 필요 없음
            // 필요 시 enemy UI 생성되면 아래처럼 추가하면 됨:
            /*
            BaseEnemyControl enemy = unit as BaseEnemyControl;
            if (enemy != null && enemy.ui != null)
                enemy.ui.UpdateBuff();
            */




            /*// BuffUI 업데이트
            BuffTurnUI buffUI = unit.GetComponent<BuffTurnUI>();
            if (buffUI != null)
            {
                buffUI.UpdateBuffTurn(0); // 버프가 없어지면 0으로 초기화
            }*/
        }
    }
}

