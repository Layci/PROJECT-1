using Project1;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProJect1
{
    public class BaseUnit : MonoBehaviour
    {
        public float maxHealth;               // �ִ� ü��
        public float curHealth;               // ���� ü��
        public float moveSpeed;               // �̵� �ӵ�
        public float unitSpeed;               // ���� �ӵ�(�� ���� ����)
        public float attackRange;             // ���� �Ÿ�
        public float damageReduction = 1f;    // ���� ����
        public float damageIncreased = 1;     // ���� ����

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
