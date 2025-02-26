using Project1;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProJect1
{
    public class Buff
    {
        public string buffName; // ���� �̸�
        public float effectValue; // ���� ȿ����
        public int duration; // ���� �� ��
        public Action<BaseCharacterControl> ApplyEffect; // ȿ�� ����
        public Action<BaseCharacterControl> RemoveEffect; // ȿ�� ����

        public Buff(string name, float value, int turns, Action<BaseCharacterControl> apply, Action<BaseCharacterControl> remove)
        {
            buffName = name;
            effectValue = value;
            duration = turns;
            ApplyEffect = apply;
            RemoveEffect = remove;
        }

        public void Apply(BaseCharacterControl target)
        {
            ApplyEffect?.Invoke(target);
            Debug.Log($"{target.unitName}���� {buffName} ���� ����! (��: {duration})");
        }

        public void Remove(BaseCharacterControl target)
        {
            RemoveEffect?.Invoke(target);
            Debug.Log($"{target.unitName}�� {buffName} ���� ����!");
        }
    }
}
