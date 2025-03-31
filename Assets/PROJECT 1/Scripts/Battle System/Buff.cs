using System;
using UnityEngine;
using Project1;
using ProJect1;

namespace Project1
{
    public class Buff
    {
        public string buffName;        // ���� �̸�
        public int remainingTurns;     // ���� ���� ��
        public float attackBoost;      // ���ݷ� ������
        public float defenseBoost;     // ���� ������
        public Type exclusiveCharacter;  // Ư�� ĳ���� ���� (������ null)

        public Buff(string name, int duration, float atkBoost, float defBoost, Type exclusiveCharacter = null)
        {
            buffName = name;
            remainingTurns = duration;
            attackBoost = atkBoost;
            defenseBoost = defBoost;
            this.exclusiveCharacter = exclusiveCharacter;
        }

        // ���� ȿ�� ����
        public void ApplyEffect(BaseUnit unit)
        {
            unit.damageIncreased += attackBoost;  // ���ݷ� ����
            unit.damageReduction -= defenseBoost; // �޴� ���� ����
            Debug.Log(unit.name + "���� " + buffName + " ���� ����! (�� ��: " + remainingTurns + ")");
        }

        // ���� ���� �� ȿ�� ���󺹱�
        public void RemoveEffect(BaseUnit unit)
        {
            unit.damageIncreased -= attackBoost;  // ���ݷ� ����
            unit.damageReduction += defenseBoost; // �޴� ���� ����
            Debug.Log(unit.name + "�� " + buffName + " ������ ������!");

            if(exclusiveCharacter != null && unit.GetType() == exclusiveCharacter)
            {
                if (unit is FayePlayerControl faye)
                {
                    faye.DecreaseBuffPower();
                }
            }

            // BuffUI ������Ʈ
            BuffUI buffUI = unit.GetComponent<BuffUI>();
            if (buffUI != null)
            {
                buffUI.UpdateBuffTurn(0); // ������ �������� 0���� �ʱ�ȭ
            }
        }
    }
}

