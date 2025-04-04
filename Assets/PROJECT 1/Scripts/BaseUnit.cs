using Project1;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProJect1
{
    public class BaseUnit : MonoBehaviour
    {
        public float maxHealth;               // 최대 체력
        public float curHealth;               // 현재 체력
        public float moveSpeed;               // 이동 속도
        public float unitSpeed;               // 유닛 속도(턴 순서 관련)
        public float attackRange;             // 공격 거리
        public float damageReduction = 1f;    // 피해 감소
        public float damageIncreased = 1;     // 피해 증가
        
        public int buffTrun;                  // 남은 버프 턴
        public bool buff = false;             // 버프 적용 확인 연산자

        public List<Buff> activeBuffs = new List<Buff>();

        public void AddBuff(Buff newBuff)
        {
            Buff existingBuff = activeBuffs.Find(buff => buff.buffName == newBuff.buffName);

            if (existingBuff != null)
            {
                // 새 버프가 기존 버프보다 약하면 적용하지 않음
                if (newBuff.attackBoost <= existingBuff.attackBoost && newBuff.defenseBoost <= existingBuff.defenseBoost)
                {
                    Debug.Log($"{newBuff.buffName} 버프가 이미 더 강력하게 적용 중이므로 무시됨.");
                    return;
                }
                // 기존 버프 제거 (리스트에서도 삭제)
                existingBuff.RemoveEffect(this);
                activeBuffs.Remove(existingBuff);
            }

            buffTrun = newBuff.remainingTurns;
            BuffTurnUI.instance.curbuff = newBuff.remainingTurns;
            
            Debug.Log("버프 활성화 턴 반영");

            // 새로운 버프 추가
            activeBuffs.Add(newBuff);
            newBuff.ApplyEffect(this);
            Debug.Log($"{newBuff.buffName} 버프가 적용되었습니다! (공격력 증가: {newBuff.attackBoost}, 방어력 증가: {newBuff.defenseBoost})");
            buff = true;
        }



        public void RemoveExpiredBuffs()
        {
            activeBuffs.RemoveAll(buff => buff.remainingTurns <= 0);
        }

        public void OnTurnStart()
        {
            foreach (var buff in activeBuffs)
            {
                buff.remainingTurns--;
                buffTrun = buff.remainingTurns;
                Debug.Log($"{buff.remainingTurns} 남은버프턴");
                if (buff.remainingTurns <= 0)
                {
                    BuffIconUI buffIconUI = GetComponent<BuffIconUI>();
                    buffIconUI.buffPower = 0;
                    buff.RemoveEffect(this);
                }
            }

            RemoveExpiredBuffs();
        }
    }
}
