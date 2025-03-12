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

        public Buff(string name, int duration, float atkBoost, float defBoost)
        {
            buffName = name;
            remainingTurns = duration;
            attackBoost = atkBoost;
            defenseBoost = defBoost;
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

        }
    }
}

