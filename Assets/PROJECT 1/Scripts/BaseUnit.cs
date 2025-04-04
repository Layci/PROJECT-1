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
        
        public int buffTrun;                  // ���� ���� ��
        public bool buff = false;             // ���� ���� Ȯ�� ������

        public List<Buff> activeBuffs = new List<Buff>();

        public void AddBuff(Buff newBuff)
        {
            Buff existingBuff = activeBuffs.Find(buff => buff.buffName == newBuff.buffName);

            if (existingBuff != null)
            {
                // �� ������ ���� �������� ���ϸ� �������� ����
                if (newBuff.attackBoost <= existingBuff.attackBoost && newBuff.defenseBoost <= existingBuff.defenseBoost)
                {
                    Debug.Log($"{newBuff.buffName} ������ �̹� �� �����ϰ� ���� ���̹Ƿ� ���õ�.");
                    return;
                }
                // ���� ���� ���� (����Ʈ������ ����)
                existingBuff.RemoveEffect(this);
                activeBuffs.Remove(existingBuff);
            }

            buffTrun = newBuff.remainingTurns;
            BuffTurnUI.instance.curbuff = newBuff.remainingTurns;
            
            Debug.Log("���� Ȱ��ȭ �� �ݿ�");

            // ���ο� ���� �߰�
            activeBuffs.Add(newBuff);
            newBuff.ApplyEffect(this);
            Debug.Log($"{newBuff.buffName} ������ ����Ǿ����ϴ�! (���ݷ� ����: {newBuff.attackBoost}, ���� ����: {newBuff.defenseBoost})");
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
                Debug.Log($"{buff.remainingTurns} ����������");
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
