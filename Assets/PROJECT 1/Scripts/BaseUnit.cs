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

        public List<Buff> activeBuffs = new List<Buff>();

        public void AddBuff(Buff newBuff)
        {
            activeBuffs.Add(newBuff);
            newBuff.ApplyEffect(this);
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
                if (buff.remainingTurns <= 0)
                {
                    buff.RemoveEffect(this);
                }
            }

            RemoveExpiredBuffs();
        }
    }
}
