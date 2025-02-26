using Project1;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProJect1
{
    public class Buff
    {
        public string buffName; // 버프 이름
        public float effectValue; // 버프 효과량
        public int duration; // 지속 턴 수
        public Action<BaseCharacterControl> ApplyEffect; // 효과 적용
        public Action<BaseCharacterControl> RemoveEffect; // 효과 제거

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
            Debug.Log($"{target.unitName}에게 {buffName} 버프 적용! (턴: {duration})");
        }

        public void Remove(BaseCharacterControl target)
        {
            RemoveEffect?.Invoke(target);
            Debug.Log($"{target.unitName}의 {buffName} 버프 종료!");
        }
    }
}
